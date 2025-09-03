using System;
using Mirror;

namespace VRF.Networking.Core.Interfaces
{
    public interface INetworkClientMessages
    {
        public void RegisterClientMessage<T>(Action<T> handler, bool requireAuthentication = true)
            where T : struct, NetworkMessage;
        public void UnregisterClientMessage<T>()
            where T : struct, NetworkMessage;
    }
}