using System;
using Mirror;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Items.Views;
using VRF.Utilities;
using VRF.Utilities.Extensions;
using VRF.Utils;

namespace VRF.Components.Items.Modules
{
    public class NetItemReceiverModule : BaseModule
    {
        private ItemObserverModule controlModule;

        // TODO да это инициализация модуля через View, это надо вынести в отдельный класс (и добавить в editor валидацию)
        private void Start()
        {
            if (View is not NetItemView netItemView) return;
            netItemView.NetBehaviour.OnStartClientCallOrSubscribe(OnInitialize);
            return;

            void OnInitialize()
            {
                if (!netItemView.NetBehaviour.isOwned)
                    netItemView.NetInitialize();
            }
        }

        public override void Initialize()
        {
            // нахождение в NetItemView обязательно
            if (View is not NetItemView netItemView)
                throw new NullReferenceException($"{nameof(NetItemReceiverModule)} must be in {nameof(NetItemView)}");

            // Если проинициализировано через Mirror
            if (!netItemView.ControlView)
            {
                // То нужно включить отображение и всё
                netItemView.SetActive(true);
                return;
            }
            
            // Иначе это локальная копия и его нужно проиницилизировать по нормальному
            controlModule = netItemView.ControlView.GetComponent<ItemObserverModule>();
            base.Initialize();
        }

        private void Update()
        {
            if (Initialized)
                transform.SyncFrom(controlModule.transform);
        }

        #region Editor
        public NetItemView ValidateReceiverComponents()
        {
            gameObject.EnsureComponent<NetworkIdentity>();
            gameObject.EnsureComponent<NetBehaviourService>();
            gameObject.EnsureComponent<NetItemView>(out var netView);
            gameObject.EnsureComponent<NetworkTransformUnreliable>();
            
            gameObject.GetComponent<ItemView>().DestroyImmediate(true);
            gameObject.GetComponent<ItemObserverModule>().DestroyImmediate(true);
            
            gameObject.GetComponent<Grabbable>().DestroyImmediate(true);
            gameObject.GetComponent<Rigidbody>().DestroyImmediate(true);
            
            OnValidate();
            VrfRuntimeEditor.SetDirty(gameObject);

            return netView;
        }
        #endregion
    }
}