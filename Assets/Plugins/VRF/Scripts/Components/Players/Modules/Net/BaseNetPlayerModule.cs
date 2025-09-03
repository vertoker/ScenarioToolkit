using System;
using NaughtyAttributes;
using UnityEngine;
using VRF.Components.Players.Views.NetPlayer;
using VRF.Networking.Models;
using VRF.Utilities.Extensions;

namespace VRF.Components.Players.Modules.Net
{
    public class BaseNetPlayerModule : BaseModule
    {
        [SerializeField] private NetMode netMode = NetMode.None;
        
        [SerializeField] private Transform cameraPivot;
        public Transform CameraPivot => cameraPivot;

        [Space]
        [ShowIf(nameof(IsReceiver))]
        [SerializeField] private bool syncPosition = true;
        [ShowIf(nameof(IsReceiver))]
        [SerializeField] private bool syncRotation = true;
        [ShowIf(nameof(IsReceiver))]
        [SerializeField] private bool syncScale = false;
        
        public bool IsObserver => netMode == NetMode.Observer;
        public bool IsReceiver => netMode == NetMode.Receiver;
        public bool SyncPosition => syncPosition;
        public bool SyncRotation => syncRotation;
        public bool SyncScale => syncScale;
        
        private BaseNetPlayerModule observer;

        public override void Initialize()
        {
            if (!IsReceiver) return;

            if (View is not BaseNetPlayerView view)
                throw new NullReferenceException($"Parent view isn't a {nameof(BaseNetPlayerView)}");

            if (view.IsOwnedByServer) return;
            
            if (!view.PlayerView.TryGetComponent<BaseNetPlayerModule>(out var baseObserver))
                throw new NullReferenceException($"Parent view isn't a {nameof(BaseNetPlayerView)}," +
                                                 $" type is {GetModuleType()}, name is {view.PlayerView.name}");
            
            observer = baseObserver;
            base.Initialize();
        }

        private Type GetModuleType() => GetType().UnderlyingSystemType;

        public void Update()
        {
            if (Initialized)
            {
                CameraPivot.SyncFrom(observer.CameraPivot, SyncPosition, SyncRotation, SyncScale);
                
                UpdateApply(observer);
            }
        }

        protected virtual void UpdateApply(BaseNetPlayerModule observer) { }
    }
}