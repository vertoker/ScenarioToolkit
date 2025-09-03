using System;

namespace VRF.Networking.Core.Interfaces
{
    public interface INetworkServerEvents
    {
        /// <summary> Вызывается когда стартует сервер </summary>
        public event Action OnServerStartEvent;
        /// <summary> Вызывается когда останавливается сервер </summary>
        public event Action OnServerStopEvent;
    }
}