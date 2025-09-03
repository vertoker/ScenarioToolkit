using System;
using Mirror;

namespace VRF.Networking.Core.Interfaces
{
    public interface INetworkServerMessages
    {
        public void RegisterServerMessage<T>(Action<NetworkConnectionToClient, T> handler, bool requireAuthentication = true)
            where T : struct, NetworkMessage;
        public void UnregisterServerMessage<T>()
            where T : struct, NetworkMessage;
    }
}