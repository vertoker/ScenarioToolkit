using System;
using Mirror;
using UnityEngine;

namespace VRF.Utils
{
    /// <summary>
    /// Utility сетевой behaviour для пробрасывания функционала Mirror во внешний мир
    /// </summary>
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetBehaviourService : NetworkBehaviour
    {
        public bool IsClientStarted { get; private set; }
        public event Action OnStartClientEvent;
        public event Action OnStopClientEvent;

        public override void OnStartClient()
        {
            IsClientStarted = true;
            OnStartClientEvent?.Invoke();
        }
        public override void OnStopClient()
        {
            OnStopClientEvent?.Invoke();
            IsClientStarted = false;
        }

        /// <summary>
        /// Особенный метод, который гарантированно вызывается
        /// только после инициализации клиента Mirror
        /// </summary>
        public void OnStartClientCallOrSubscribe(Action action)
        {
            if (IsClientStarted) action.Invoke();
            else OnStartClientEvent += SingleCaller;
            return;

            void SingleCaller()
            {
                OnStartClientEvent -= SingleCaller;
                action?.Invoke();
            }
        }
    }
}