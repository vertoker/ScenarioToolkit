using System;
using SimpleUI.Core;
using UnityEngine;
using VRF.Players.Models;
using VRF.Players.Scriptables;
using VRF.UI.Settings.Core.Interfaces;
using VRF.Utilities;
using Zenject;

namespace VRF.UI.Settings.Core
{
    public abstract class BaseParameterView : UIView
    {
        [SerializeField] private PlayerControlModes activeModes;

        /// <summary>
        /// Включить/Отключить объект по состоянию того, должен ли
        /// он отображаться с учётом текущего локального режима
        /// </summary>
        /// <param name="controlModes">Активные режимы</param>
        public void SetEnabled(PlayerControlModes controlModes)
        {
            var activeVR = activeModes.IsVR() && controlModes.IsVR();
            var activeWASD = activeModes.IsWASD() && controlModes.IsWASD();
            var active = activeVR || activeWASD;
            gameObject.SetActive(active);
        }
    }
    
    /// <summary>
    /// Базовая реализация параметра, отвечает за связь с основным контроллером и активацией себя
    /// </summary>
    public abstract class BaseParameterController<TView> : UIController<TView>,
        IInitializable, IDisposable, ISettingsParameter where TView : BaseParameterView
    {
        /// <summary>
        /// Базовый контроллер
        /// </summary>
        protected readonly SettingsParametersController Controller;

        protected BaseParameterController(SettingsParametersController controller, TView view) : base(view)
        {
            Controller = controller;
        }
        
        public virtual void Initialize()
        {
            View.SetEnabled(Controller.ControlModes);
            Controller.Register(this);
        }
        public virtual void Dispose()
        {
            Controller.Unregister(this);
        }
        
        public abstract void SetupModel(IReadOnlySettingsModel model);
    }
}