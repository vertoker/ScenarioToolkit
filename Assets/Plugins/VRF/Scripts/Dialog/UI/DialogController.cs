using System;
using System.Collections.Generic;
using SimpleUI.Core;
using UnityEngine;
using Zenject;

namespace VRF.Dialog.UI
{
    /// <summary> Контроллер для отображения диалогов на DialogView </summary>
    public class DialogController : UIController<DialogView>, IInitializable, IDisposable
    {
        private readonly DialogService service;

        private readonly List<DialogLineConfig> currentDialogLines = new();
        private int currentPage;

        public DialogController([InjectOptional] DialogService service, DialogView view) : base(view)
        {
            this.service = service;
            if (service == null) return;
            
            RenderPage(0);
        }

        public void Initialize()
        {
            if (service == null) return;
            
            service.AddedDialogLine += AddedDialogLine;
            service.RemovedDialogLine += RemovedDialogLine;
            service.ClearedDialogLines += ClearedDialogLines;

            View.UpBtn.onClick.AddListener(DisplayPreviousPage);
            View.DownBtn.onClick.AddListener(DisplayNextPage);

            for (var i = 0; i < View.DialogLineItems.Count; i++)
            {
                var index = i;
                void Action() => SelectDialogLine(index);
                View.DialogLineItems[i].button.onClick.AddListener(Action);
            }
        }

        public void Dispose()
        {
            if (service == null) return;
            
            service.AddedDialogLine -= AddedDialogLine;
            service.RemovedDialogLine -= RemovedDialogLine;
            service.ClearedDialogLines -= ClearedDialogLines;

            View.UpBtn.onClick.RemoveListener(DisplayPreviousPage);
            View.DownBtn.onClick.RemoveListener(DisplayNextPage);

            for (var i = 0; i < View.DialogLineItems.Count; i++)
                View.DialogLineItems[i].button.onClick.RemoveAllListeners();
        }

        private void AddedDialogLine(DialogLineConfig dialogLine)
        {
            currentDialogLines.Add(dialogLine);
            RenderPage(currentPage);
        }

        private void RemovedDialogLine(DialogLineConfig dialogLine)
        {
            currentDialogLines.Remove(dialogLine);
            RenderPage(currentPage);
        }

        private void ClearedDialogLines()
        {
            currentDialogLines.Clear();
            RenderPage(0);

            if (View.CloseMenuAfterClear) // Закрыть диалог
                View.SwitchScreenBtn.Button.onClick.Invoke();
        }

        private void SelectDialogLine(int indexItem)
        {
            var index = Mathf.Min(currentPage * View.ItemsPerPage + indexItem);
            var line = currentDialogLines[index];
            service.SelectDialogLine(line);
        }

        private void DisplayNextPage() => RenderPage(currentPage + 1);
        private void DisplayPreviousPage() => RenderPage(currentPage - 1);

        /// <summary>
        /// Отобразить диалоги в зависимости от количества диалогов на страницу
        /// </summary>
        /// <param name="page">Индекс страницы диалогов</param>
        private void RenderPage(int page)
        {
            var startIndex = page * View.ItemsPerPage;
            var endIndex = Mathf.Min(startIndex + View.ItemsPerPage, currentDialogLines.Count);

            var itemsCounter = 0;
            for (var i = startIndex; i < endIndex; i++)
            {
                var item = View.DialogLineItems[itemsCounter];
                var line = currentDialogLines[i];

                item.text.text = line.DialogLine;
                item.obj.SetActive(true);
                itemsCounter++;
            }

            for (var i = itemsCounter; i < View.ItemsPerPage; i++)
            {
                var item = View.DialogLineItems[i];
                item.obj.SetActive(false);
            }

            currentPage = page;
            RenderButtons();
        }

        /// <summary>Показать возможные кнопки переключения страниц </summary>
        private void RenderButtons()
        {
            var maxPagesCount = Mathf.CeilToInt(currentDialogLines.Count / (float)View.ItemsPerPage);
            View.UpBtn.gameObject.SetActive(currentPage > 0);
            View.DownBtn.gameObject.SetActive(currentPage < maxPagesCount - 1);
        }
    }
}