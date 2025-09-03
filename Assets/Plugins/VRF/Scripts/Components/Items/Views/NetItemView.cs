using System;
using KBCore.Refs;
using Mirror;
using UnityEngine;
using VRF.Networking.Core.Client;
using VRF.Networking.Messages;
using VRF.Utils;
using Zenject;

namespace VRF.Components.Items.Views
{
    [RequireComponent(typeof(NetBehaviourService))]
    public class NetItemView : BaseView
    {
        [SerializeField, Self] private NetBehaviourService netBehaviour;
        [SerializeField, Child] private Renderer[] renderers = Array.Empty<Renderer>();
        [SerializeField, Child] private Collider[] colliders = Array.Empty<Collider>();
        [SerializeField] private Behaviour[] behaviours = Array.Empty<Behaviour>();

        public ItemView ControlView { get; private set; }
        public NetBehaviourService NetBehaviour => netBehaviour;

        [Inject] private ClientItemsService service;
        
        protected override void OnValidate()
        {
            base.OnValidate();
            SetActive(false);
        }
        private void OnEnable()
        {
            service.OnEnableItem += ReceiveEnable;
            service.OnDisableItem += ReceiveDisable;
        }
        private void OnDisable()
        {
            service.OnEnableItem -= ReceiveEnable;
            service.OnDisableItem -= ReceiveDisable;
        }

        /// <summary>
        /// Локальная инициализация
        /// </summary>
        public void LocalInitialize(ItemView view)
        {
            ControlView = view;
            SetActive(false);
            netBehaviour.OnStartClientCallOrSubscribe(InitializeInternal);
        }
        /// <summary>
        /// Инициализация через Mirror
        /// </summary>
        public void NetInitialize()
        {
            SetActive(true);
            InitializeInternal();
        }

        public void SendEnable()
        {
            NetworkClient.Send(new NetItemEnable_RequestMessage
            {
                AssetHashCode = AssetHashCode,
                RuntimeHashCode = RuntimeHashCode
            });
        }
        public void SendDisable()
        {
            NetworkClient.Send(new NetItemDisable_RequestMessage
            {
                AssetHashCode = AssetHashCode,
                RuntimeHashCode = RuntimeHashCode
            });
        }

        private void ReceiveEnable(NetItemEnable_RequestMessage msg)
        {
            // Предметы должны иметь одинаковые коды, то есть быть одного типа
            if (msg.AssetHashCode != AssetHashCode) return;
            // GetHashCode не синхронизируется по сети
            // А ещё локально сетевой предмет должен быть всегда отключен
            // TODO нужно как-то идентифицировать то, что пакет пришёл от локального отправителя и блочить его
            if (msg.RuntimeHashCode == RuntimeHashCode) return;
            
            SetActive(true);
        }
        private void ReceiveDisable(NetItemDisable_RequestMessage msg)
        {
            if (msg.AssetHashCode != AssetHashCode) return;
            if (msg.RuntimeHashCode == RuntimeHashCode) return;
            SetActive(false);
        }
        
        public void SetActive(bool active)
        {
            foreach (var rend in renderers)
                rend.enabled = active;
            foreach (var col in colliders)
                col.enabled = active;
            foreach (var behaviour in behaviours)
                behaviour.enabled = active;
        }
    }
}