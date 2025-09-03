using System;
using System.Collections.Generic;
using SimpleUI.Core;
using UnityEngine;
using VRF.Players.Core;
using VRF.UI.GameMenu;
using Zenject;

namespace VRF.Dialog
{
    /// <summary> Сервис для работы с фразами </summary>
    public class DialogService : IInitializable
    {
        private readonly PlayersContainer playersContainer;

        /// <summary> Данные о фразе </summary>
        private readonly struct Entry
        {
            private readonly DialogLineConfig dialogLine;
            private readonly Action<DialogLineConfig> selectedCallback;

            public Entry(DialogLineConfig dialogLine, Action<DialogLineConfig> selectedCallback = null)
            {
                this.dialogLine = dialogLine;
                this.selectedCallback = selectedCallback;
            }

            public void TryInvoke() => selectedCallback?.Invoke(dialogLine);
        }

        private GameObject dialogBtn;
        private ScreensManagerInstance instance;

        /// <summary> Текущие фразы </summary>
        private readonly Dictionary<int, Entry> dialogLines = new();

        public event Action<DialogLineConfig> SelectedDialogLine;
        public event Action<DialogLineConfig> AddedDialogLine;
        public event Action<DialogLineConfig> RemovedDialogLine;

        public event Action ClearedDialogLines;

        public DialogService(PlayersContainer playersContainer)
        {
            this.playersContainer = playersContainer;
            UpdateInstance();
            this.playersContainer.PlayerChanged += PlayerChanged;
        }

        public void Initialize()
        {
            if(instance == null) return;
            instance.Start();
            var manager = instance.Manager;
            var mainScreen = manager.Find<GameMenuScreen>();
            var mainView = mainScreen.Get<GameMenuView>();
            dialogBtn = mainView.DialogBtn;

            UpdateMenuBtnStatus();
        }

        private void PlayerChanged()
        {
            UpdateInstance();
            Initialize();
        }

        private void UpdateInstance()
        {
            var view = playersContainer.CurrentValue.View;
            instance = view != null ? view.GameUI : null;
        }

        private void SetActiveMenuBtn(bool active)
        {
            dialogBtn.SetActive(active);
        }

        private void UpdateMenuBtnStatus()
        {
            if (dialogBtn.activeSelf)
            {
                if (dialogLines.Count == 0)
                    SetActiveMenuBtn(false);
            }
            else
            {
                if (dialogLines.Count != 0)
                    SetActiveMenuBtn(true);
            }
        }

        /// <summary>
        ///     Добавить фразу в список доступных
        /// </summary>
        /// <param name="dialogLine">Фраза</param>
        /// <param name="action">Действие при выборе фразы</param>
        public void AddDialogLine(DialogLineConfig dialogLine, Action<DialogLineConfig> action = null)
        {
            if (dialogLines.ContainsKey(dialogLine.AssetHashCode))
            {
                Debug.LogWarning($"Can't add duplicate dialog line {dialogLine.DialogLine}");
                return;
            }

            var entry = new Entry(dialogLine, action);
            dialogLines.Add(dialogLine.AssetHashCode, entry);
            AddedDialogLine?.Invoke(dialogLine);
            UpdateMenuBtnStatus();
        }

        /// <summary>
        /// Убрать фразу из списка доступных
        /// </summary>
        /// <param name="dialogLine">Фраза</param>
        /// <returns>Получилось ли убрать фразу</returns>
        public bool RemoveDialogLine(DialogLineConfig dialogLine)
        {
            if (!dialogLines.Remove(dialogLine.AssetHashCode))
                return false;

            UpdateMenuBtnStatus();
            RemovedDialogLine?.Invoke(dialogLine);
            return true;
        }

        /// <summary> Убрать все фразы из списка доступных </summary>
        public void Clear()
        {
            dialogLines.Clear();
            ClearedDialogLines?.Invoke();
            UpdateMenuBtnStatus();
        }

        /// <summary>
        /// Выбрать фразу, например для произношения
        /// </summary>
        /// <param name="dialogLine">Фраза</param>
        /// <param name="clear">Убрать все доступные фразы</param>
        public void SelectDialogLine(DialogLineConfig dialogLine, bool clear = true)
        {
            if (!dialogLines.TryGetValue(dialogLine.AssetHashCode, out var entry))
            {
                Debug.LogWarning("Can't select not added dialog line");
                return;
            }

            if (clear)
                Clear();
            entry.TryInvoke();
            SelectedDialogLine?.Invoke(dialogLine);
        }
    }
}