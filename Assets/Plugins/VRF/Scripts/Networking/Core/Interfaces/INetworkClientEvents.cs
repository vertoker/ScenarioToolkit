using System;

namespace VRF.Networking.Core.Interfaces
{
    public interface INetworkClientEvents
    {
        /// <summary> Вызывается когда стартует клиент </summary>
        public event Action OnClientStartEvent;
        /// <summary> Вызывается когда останавливается клиент </summary>
        public event Action OnClientStopEvent;
        
        /// <summary> Вызывается когда клиент подключается к серверу </summary>
        public event Action OnClientConnectEvent;
        /// <summary> Вызывается когда клиент отключается от сервера </summary>
        public event Action OnClientDisconnectEvent;
    }
}