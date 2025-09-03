using System;
using VRF.Components.Players.Views.NetPlayer;
using VRF.Components.Players.Views.PlayerIK;
using VRF.Networking.Core;
using VRF.Players.Services.Views;

namespace VRF.Players.Parents
{
    public class PlayerParentsService : IDisposable
    {
        private readonly ProjectViewSpawnerService spawner;
        private readonly PlayerParentsContainer parents;

        public PlayerParentsService(ProjectViewSpawnerService spawner, PlayerParentsContainer parents)
        {
            this.spawner = spawner;
            this.parents = parents;
            
            Initialize();
        }

        public void Initialize()
        {
            spawner.RegisterParent<BaseNetPlayerView>(parents.NetPlayers);
            spawner.RegisterParent<NetPlayerVRView>(parents.NetPlayers);
            spawner.RegisterParent<NetPlayerWASDView>(parents.NetPlayers);
            
            spawner.RegisterParent<VRFNetworkManager>(parents.MirrorPlayers);
            
            spawner.RegisterParent<BasePlayerIKView>(parents.PlayersIK);
            spawner.RegisterParent<PlayerVRIKView>(parents.PlayersIK);
            spawner.RegisterParent<PlayerWASDIKView>(parents.PlayersIK);
        }

        public void Dispose()
        {
            spawner.UnregisterParent<BaseNetPlayerView>();
            spawner.UnregisterParent<NetPlayerVRView>();
            spawner.UnregisterParent<NetPlayerWASDView>();
            
            spawner.UnregisterParent<VRFNetworkManager>();
            
            spawner.UnregisterParent<BasePlayerIKView>();
            spawner.UnregisterParent<PlayerVRIKView>();
            spawner.UnregisterParent<PlayerWASDIKView>();
        }
    }
}