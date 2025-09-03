using System.Collections.Generic;
using Zenject;
using Mirror;
using System;
using VRF.Players.Scriptables;

namespace VRF.Networking.Core.Server
{
    public class ServerCounterPlayers : IInitializable, IDisposable
    {
        private readonly Dictionary<PlayerIdentityConfig, int> counters = new();
        private readonly VRFNetworkManager networkManager;

        public ServerCounterPlayers(VRFNetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }
        
        public void Initialize()
        {
            networkManager.OnServerClientAuthorized += ClientAuthorized;
            networkManager.OnServerDisconnectEvent += ClientDisconnected;
        }
        public void Dispose()
        {
            networkManager.OnServerClientAuthorized -= ClientAuthorized;
            networkManager.OnServerDisconnectEvent -= ClientDisconnected;
        }

        /// <summary> Проверка на лимит подключений для личности </summary>
        public bool IsValidLimit(PlayerIdentityConfig identity)
        {
            // Отсутствие лимита означает пройденную проверку
            if (!identity.HasLimit) return true;
            // Если счётчик не найден, значит он равен 0, значит для личности есть место
            if (!counters.TryGetValue(identity, out var counter)) return true;
            // Количество подключений под аккаунтом должно быть меньше лимита, основная проверка
            return counter < identity.LimitCount;
        }

        private void ClientAuthorized(NetworkConnectionToClient conn, PlayerIdentityConfig identity)
        {
            if (!counters.TryAdd(identity, 0))
                counters[identity]++;
        }
        private void ClientDisconnected(NetworkConnectionToClient conn, PlayerIdentityConfig identity, NetworkIdentity netPlayer)
        {
            if (counters == null || !identity) return;
            if (!counters.TryGetValue(identity, out var counter)) return;
            
            if (counter == 0)
                counters.Remove(identity);
            else counters[identity] = counter - 1;
        }
    }
}