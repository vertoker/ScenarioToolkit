using System;
using UnityEngine;
using VRF.Components.Players.Views.Player;
using VRF.DataSources;
using VRF.Players.Core;
using VRF.Players.Models.Player;
using VRF.Players.Services.Views;
using Zenject;

namespace VRF.Players.Init
{
    public class PlayersLauncher : IInitializable, IDisposable
    {
        private readonly PlayersContainer playersContainer;
        private readonly ContainerDataSource dataSource;
        private readonly SceneViewSpawnerService spawnerHandler;

        private readonly DiContainer diContainer;
        private readonly Transform spawnPoint;
        private readonly DataSourceType[] sources;

        public PlayersLauncher(PlayersContainer playersContainer, ContainerDataSource dataSource,
            SceneViewSpawnerService spawnerHandler, DiContainer diContainer,
            Transform spawnPoint, DataSourceType[] sources)
        {
            this.playersContainer = playersContainer;
            this.dataSource = dataSource;
            
            this.spawnerHandler = spawnerHandler;
            this.diContainer = diContainer;
            
            this.spawnPoint = spawnPoint;
            this.sources = sources;
        }
        
        public void Initialize()
        {
            playersContainer.Initialize(spawnerHandler, diContainer);
            
            spawnerHandler.RegisterParent<BasePlayerView>(spawnPoint);
            spawnerHandler.RegisterParent<PlayerVRView>(spawnPoint);
            spawnerHandler.RegisterParent<PlayerWASDView>(spawnPoint);
            spawnerHandler.RegisterParent<PlayerSpectatorWASDView>(spawnPoint);
            
            var playerModel = dataSource.Load<PlayerModel>(sources);
            playersContainer.UpdatePlayerMode(playerModel.PriorityControlMode);
        }
        public void Dispose()
        {
            spawnerHandler.UnregisterParent<BasePlayerView>();
            spawnerHandler.UnregisterParent<PlayerVRView>();
            spawnerHandler.UnregisterParent<PlayerWASDView>();
            spawnerHandler.UnregisterParent<PlayerSpectatorWASDView>();
            
            playersContainer.Dispose();
        }
    }
}