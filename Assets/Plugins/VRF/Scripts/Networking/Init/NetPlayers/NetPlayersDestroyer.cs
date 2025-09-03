using System;
using VRF.Networking.Core;
using Zenject;

namespace VRF.Networking.Init.NetPlayers
{
    /// <summary>
    /// Деспавнит игроков при Dispose
    /// </summary>
    public class NetPlayersDestroyer : IInitializable, IDisposable
    {
        private readonly VRFNetworkManager networkManager;

        public NetPlayersDestroyer([InjectOptional] VRFNetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }
        
        public void Initialize()
        {
            if (!networkManager) return;
        }
        public void Dispose()
        {
            if (!networkManager) return;
            DespawnPlayers();
        }
        public void DespawnPlayers()
        {
            if (!networkManager) return;
            networkManager.DespawnNetPlayers();
        }
    }
}