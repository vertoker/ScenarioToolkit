using System;
using NaughtyAttributes;
using UnityEngine;

namespace VRF.Components.Players.Views.PlayerIK
{
    /// <summary>
    /// Реализация для пустого view, так как IK всегда зависим
    /// от кого-то и не может существовать сам по себе
    /// </summary>
    public abstract class BasePlayerIKView : BaseView
    {
        // Для удобства прокидывается управляющий компонент
        public BaseView ControlView { get; private set; }
        
        public void Initialize(BaseView controlView)
        {
            ControlView = controlView;
            InitializeInternal();
        }
    }
}