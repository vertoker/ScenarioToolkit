using System;
using VRF.Networking.Messages;
using Zenject;

namespace VRF.Networking.Core.Client
{
    /// <summary>
    /// Клиентский сервис для обработки сообщений, связаных с предметами
    /// Принимает сообщение и вызывает соответсвующее событие
    /// </summary>
    public class ClientItemsService : IInitializable, IDisposable
    {
        private readonly VRFNetworkManager networkManager;
        
        public event Action<NetItemEnable_RequestMessage> OnEnableItem;
        public event Action<NetItemDisable_RequestMessage> OnDisableItem;
        
        public event Action<NetItemToggleBehaviour_Message> OnToggleBehaviourItem;
        public event Action<NetItemToggleBehaviours_Message> OnToggleBehavioursItem;

        public ClientItemsService(VRFNetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        public void Initialize()
        {
            networkManager.RegisterClientMessage<NetItemEnable_RequestMessage>(Receive);
            networkManager.RegisterClientMessage<NetItemDisable_RequestMessage>(Receive);
            
            networkManager.RegisterClientMessage<NetItemToggleBehaviour_Message>(Receive);
            networkManager.RegisterClientMessage<NetItemToggleBehaviours_Message>(Receive);
        }
        public void Dispose()
        {
            networkManager.UnregisterClientMessage<NetItemEnable_RequestMessage>();
            networkManager.UnregisterClientMessage<NetItemDisable_RequestMessage>();
            
            networkManager.UnregisterClientMessage<NetItemToggleBehaviour_Message>();
            networkManager.UnregisterClientMessage<NetItemToggleBehaviours_Message>();
        }
        
        private void Receive(NetItemEnable_RequestMessage msg) => OnEnableItem?.Invoke(msg);
        private void Receive(NetItemDisable_RequestMessage msg) => OnDisableItem?.Invoke(msg);
        private void Receive(NetItemToggleBehaviour_Message msg) => OnToggleBehaviourItem?.Invoke(msg);
        private void Receive(NetItemToggleBehaviours_Message msg) => OnToggleBehavioursItem?.Invoke(msg);
    }
}