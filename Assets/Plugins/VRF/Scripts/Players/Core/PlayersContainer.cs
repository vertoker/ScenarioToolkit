using System;
using System.Collections.Generic;
using UnityEngine;
using VRF.Components.Players.Views.Player;
using VRF.Identities.Core;
using VRF.Players.Controllers.Builders;
using VRF.Players.Models;
using VRF.Players.Scriptables;
using VRF.Players.Services.Views;
using Zenject;

namespace VRF.Players.Core
{
    public class PlayersContainer
    {
        public readonly struct PlayerKey
        {
            public readonly PlayerIdentityConfig Identity;
            public readonly PlayerControlModes Mode;

            public PlayerKey(PlayerIdentityConfig identity, PlayerControlModes mode)
            {
                Identity = identity;
                Mode = mode;
            }
        }
        
        private readonly IdentityService identityService;
        
        private SceneViewSpawnerService spawnerHandler;
        private DiContainer container;
        private PlayerIdentityConfig identity;
        
        private readonly Dictionary<PlayerKey, PlayerControlModeData> modeData = new();
        
        public PlayerKey CurrentKey { get; private set; }
        public PlayerControlModeData CurrentValue { get; private set; }
        public event Action PlayerChanged;
        
        public PlayersContainer(IdentityService identityService)
        {
            this.identityService = identityService;
        }
        
        public void Initialize(SceneViewSpawnerService newSpawnerHandler, DiContainer sceneContainer)
        {
            spawnerHandler = newSpawnerHandler;
            container = sceneContainer;
            identity = identityService.SelfIdentity;
        }
        public void Dispose()
        {
            spawnerHandler = null;
            container = null;
            identity = null;
            
            CurrentKey = new PlayerKey();
            CurrentValue = new PlayerControlModeData();
            PlayerChanged = null;
            modeData.Clear();
        }
        
        // Фишка со сменой Identity отключена из-за сложности поддержки этой фишки в Runtime
        // Единственный способ сменить личность - перезайти под другим аккаунтом
        // А так можно менять режим игрока динамически
        
        public void UpdatePlayerMode(PlayerControlModes mode)
        {
            if (CurrentKey.Mode == mode || mode == 0) return;

            BasePlayerView prevView = null;
            
            if (modeData.TryGetValue(CurrentKey, out var data))
            {
                prevView = data.View;
                prevView.gameObject.SetActive(false);
                data.Builder.BuilderDispose();
            }

            CurrentKey = new PlayerKey(identity, mode);
            
            if (!modeData.ContainsKey(CurrentKey))
                CreateNewPlayer(mode);

            CurrentValue = modeData[CurrentKey];
            
            PlayerChanged?.Invoke();
            
            var view = CurrentValue.View;
            view.gameObject.SetActive(true);
            CurrentValue.Builder.BuilderInitialize();
            
            if (prevView)
            {
                var viewTransform = view.transform;
                var prevViewTransform = prevView.transform;
                
                viewTransform.position = prevViewTransform.position;
                viewTransform.rotation = prevViewTransform.rotation;
            }
        }

        private void CreateNewPlayer(PlayerControlModes mode)
        {
            var spawnConfiguration = identity.Appearance.GetConfiguration(mode);
            var localPlayerView = spawnerHandler.SpawnView(spawnConfiguration.Player);
            
            Debug.Log($"Create local {localPlayerView.ViewType.Name}");
            localPlayerView.Construct(CurrentKey.Identity, CurrentKey.Mode);
            localPlayerView.BindView(container, spawnConfiguration);
            localPlayerView.Initialize();

            var builder = (IPlayerBuilder)container.Resolve(localPlayerView.GetBuilderType);
            var newData = new PlayerControlModeData(localPlayerView, builder);
            modeData.Add(CurrentKey, newData);
        }
    }
}