using System;
using SimpleUI.Anim;
using SimpleUI.Core;
using UnityEngine.InputSystem;
using Zenject;

namespace VRF.Players.Controllers.Executors.UI
{
    public class ScreenToggler<TScreen> : IInitializable, IDisposable where TScreen : ScreenBase
    {
        public event Action<bool> OnActive;
        public event Action OnOpen;
        public event Action OnClose;
        
        private readonly ScreensManager manager;
        private readonly InputAction btn;
        private readonly AnimParameters onOpen;
        private readonly AnimParameters onClose;
        private readonly AnimParameters onBackUntil;

        public ScreenToggler(ScreensManager manager, InputAction btn, 
            AnimParameters onOpen = null, AnimParameters onClose = null, AnimParameters onBackUntil = null)
        {
            this.manager = manager;
            this.btn = btn;
            
            this.onOpen = onOpen ?? AnimParameters.OnlyOpen;
            this.onClose = onClose ?? AnimParameters.OnlyClose;
            this.onBackUntil = onBackUntil ?? AnimParameters.NoAnim;
        }
        
        public virtual void Initialize()
        {
            btn.Enable();
            btn.performed += ScreenPerformed;
        }
        public virtual void Dispose()
        {
            btn.performed -= ScreenPerformed;
            btn.Disable();
        }
        
        private void ScreenPerformed(InputAction.CallbackContext ctx)
        {
            Toggle();
        }

        public void Toggle()
        {
            if (manager.IsOpened<TScreen>())
            {
                // закрыть
                manager.Close<TScreen>(onClose);
                OnActive?.Invoke(false);
                OnClose?.Invoke();
            }
            else if (manager.InStack<TScreen>())
            {
                // вернуться на экран
                manager.BackUntil<TScreen>(false, onBackUntil);
                // TODO пока так, но для текущих меню это не надо
            }
            else
            {
                // открыть
                manager.Open<TScreen>(onOpen);
                OnOpen?.Invoke();
                OnActive?.Invoke(true);
            }
        }
    }
}