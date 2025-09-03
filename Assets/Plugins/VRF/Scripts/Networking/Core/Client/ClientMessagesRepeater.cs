using System;
using VRF.Networking.Messages;
using Zenject;

namespace VRF.Networking.Core.Client
{
    /// <summary>
    /// Общий сервис для приёма клиентских сообщений
    /// </summary>
    public class ClientMessagesRepeater : IInitializable, IDisposable
    {
        private readonly VRFNetworkManager networkManager;
        
        // TODO превратить в обобщённый сервис для подписки ивентов по типу
        public event Action<InitNetPlayer_Message> OnInitNetPlayer;

        public ClientMessagesRepeater(VRFNetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        public void Initialize()
        {
            networkManager.RegisterClientMessage<InitNetPlayer_Message>(Receive);
        }
        public void Dispose()
        {
            networkManager.UnregisterClientMessage<InitNetPlayer_Message>();
        }
        
        private void Receive(InitNetPlayer_Message msg) => OnInitNetPlayer?.Invoke(msg);
    }
}