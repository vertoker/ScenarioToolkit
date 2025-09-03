using System;
using UnityEngine;
using Zenject;

namespace VRF.Players.Services
{
    /// <summary>
    /// Сервис для скрытия курсора
    /// </summary>
    public class CursorHideoutService : IInitializable, IDisposable
    {
        // То есть изначально курсор виден, то есть hide = false
        // А для активации сокрытия курсора нужно, чтобы hide = true
        private bool currentFocus, active;
        
        public bool IsShow => active;
        public bool IsHide => active;
        
        public event Action OnHide;
        public event Action OnShow;
        public event Action<bool> OnCursorActive;

        public CursorHideoutService()
        {
            SetActiveInternal(currentFocus && active);
        }

        public void Initialize()
        {
            Application.focusChanged += ApplicationOnFocus;
            ApplicationOnFocus(Application.isFocused);
        }
        public void Dispose()
        {
            Application.focusChanged -= ApplicationOnFocus;
        }

        private void ApplicationOnFocus(bool focus)
        {
            currentFocus = focus;
            SetActiveInternal(currentFocus && active);
        }

        public void Hide() => SetHideoutActive(true);
        public void Show() => SetHideoutActive(false);
        
        public void SetHideoutActive(bool activeHide)
        {
            active = activeHide;
            SetActiveInternal(currentFocus && active);
        }

        private void SetActiveInternal(bool activeHide)
        {
            Cursor.visible = !activeHide;
            Cursor.lockState = activeHide ? CursorLockMode.Locked : CursorLockMode.None;
            
            if (activeHide) 
                OnHide?.Invoke();
            else OnShow?.Invoke();
            
            OnCursorActive?.Invoke(!activeHide);
        }
    }
    // Взято из Виртуальной станции с любовью)
}