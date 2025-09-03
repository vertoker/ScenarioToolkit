using System;
using Mirror;
using VRF.Networking.Core;
using Zenject;

namespace VRF.Networking.Init.NetPlayers
{
    /// <summary>
    /// Спавнит игроков при старте сервера
    /// </summary>
    public class NetPlayersSpawner : IInitializable, IDisposable
    {
        private readonly VRFNetworkManager networkManager;
        private bool subscribed;

        public NetPlayersSpawner([InjectOptional] VRFNetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }
        
        public void Initialize()
        {
            if (!networkManager) return;
            subscribed = !NetworkServer.active;
            if (subscribed)
                networkManager.OnServerStartEvent += SpawnPlayers;
            else SpawnPlayers();
        }
        public void Dispose()
        {
            if (!networkManager) return;
            if (subscribed)
                networkManager.OnServerStartEvent -= SpawnPlayers;
        }
        
        public void SpawnPlayers()
        {
            if (!networkManager) return;
            networkManager.SpawnNetPlayers();
        }
    }
}