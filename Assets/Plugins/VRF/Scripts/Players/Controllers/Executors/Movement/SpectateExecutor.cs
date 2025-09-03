using System;
using System.Collections.Generic;
using System.Linq;
using SimpleUI.Core;
using SimpleUI.Interfaces.Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Components.Players.Modules.IK;
using VRF.Components.Players.Modules.Net;
using VRF.Components.Players.Views.NetPlayer;
using VRF.Networking.Players;
using VRF.Players.Controllers.Builders.UI;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Executors.Movement.Interfaces;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Enums;
using VRF.Utilities;
using VRF.Utilities.Extensions;
using Zenject;

namespace VRF.Players.Controllers.Executors.Movement
{
    public class SpectateExecutor : BaseModelExecutor<SpectateModel>, IPlayerModeExecutor, IInitializable, IDisposable, ITickable
    {
        public event Action<BaseNetPlayerView> PlayerViewChanged;
        
        private readonly Transform cameraPivot;
        private readonly NetPlayersContainerService netContainer;
        private readonly ScreensManager manager;
        
        private BaseNetPlayerView currentView;
        private BaseNetPlayerModule currentNetModule;
        private BasePlayerIKModule currentModuleIK;
        private readonly List<BaseNetPlayerView> views;
        
        private readonly InputAction lastPlayer;
        private readonly InputAction nextPlayer;

        public PlayerMode ExecutableMode => PlayerMode.Spectate;

        public event Action OnUpdate;
        public IReadOnlyList<BaseNetPlayerView> Views => views;

        public SpectateExecutor(SpectateModel model, Transform cameraPivot, 
            NetPlayersContainerService netContainer, ScreensManager manager,
            InputAction lastPlayer, InputAction nextPlayer) : base(model)
        {
            this.cameraPivot = cameraPivot;
            this.netContainer = netContainer;
            this.manager = manager;
            this.lastPlayer = lastPlayer;
            this.nextPlayer = nextPlayer;
            views = new List<BaseNetPlayerView>();
        }

        public override void Enable()
        {
            base.Enable();
            
            lastPlayer.Enable();
            nextPlayer.Enable();
            
            manager.Open<SpectatorScreen>();
            EnableCurrentView();
        }
        public override void Disable()
        {
            base.Disable();
            
            lastPlayer.Disable();
            nextPlayer.Disable();
            
            if (manager.InList<SpectatorScreen>())
                manager.Close<SpectatorScreen>();
            DisableCurrentView();
        }

        public void Initialize()
        {
            lastPlayer.performed += LastPlayer;
            nextPlayer.performed += NextPlayer;

            netContainer.OnAddPlayer += OnAddPlayer;
            netContainer.OnRemovePlayer += OnRemovePlayer;
            
            foreach (var player in netContainer.NetPlayers)
                OnAddPlayer(player);
        }
        public void Dispose()
        {
            lastPlayer.performed -= LastPlayer;
            nextPlayer.performed -= NextPlayer;
            
            netContainer.OnAddPlayer -= OnAddPlayer;
            netContainer.OnRemovePlayer -= OnRemovePlayer;
        }
        public void Tick()
        {
            if (!Enabled) return;
            
            if (currentNetModule)
                cameraPivot.SyncFrom(currentNetModule.CameraPivot);
        }

        private void LastPlayer(InputAction.CallbackContext ctx) => LastPlayer();
        private void NextPlayer(InputAction.CallbackContext ctx) => NextPlayer();

        private void OnAddPlayer(BaseNetPlayerView netPlayer)
        {
            if (netPlayer.IsOwnedByLocalPlayer) return; // preserve self spectate
            views.Add(netPlayer);

            if (!currentView && netPlayer.IsOwnedByServer)
                UpdateCurrentView(netPlayer);

            OnUpdate?.Invoke();
        }
        private void OnRemovePlayer(BaseNetPlayerView netPlayer)
        {
            if (netPlayer.IsOwnedByLocalPlayer) return; // preserve self spectate
            views.Remove(netPlayer);

            if (currentView == netPlayer)
            {
                netPlayer = views.FirstOrDefault(v => v.IsOwnedByServer);
                UpdateCurrentView(netPlayer);
            }
            
            OnUpdate?.Invoke();
        }
        
        private void UpdateCurrentView(BaseNetPlayerView selectedView)
        {
            DisableCurrentView();
            
            currentView = selectedView;
            currentNetModule = currentView ? currentView.GetComponent<BaseNetPlayerModule>() : null;
            currentModuleIK = currentView ? currentView.GetIKModule() : null;
            
            EnableCurrentView();
            
            PlayerViewChanged?.Invoke(currentView);
        }
        private void DisableCurrentView()
        {
            if (!currentModuleIK) return;
            currentModuleIK.HideIK.Show = true;
        }
        private void EnableCurrentView()
        {
            if (!currentModuleIK) return;
            currentModuleIK.HideIK.Show = false;
        }

        private void ChangeSpectatedPlayer(Func<int, int, int> updateIndexPredicate)
        {
            var length = views.Count;
            var currentIndex = views.IndexOf(currentView);
            var indexCounter = currentIndex;

            do
            {
                indexCounter = updateIndexPredicate(indexCounter, length);

                var selectedView = views[indexCounter];
                
                if (selectedView.IsOwnedByServer)
                {
                    UpdateCurrentView(selectedView);
                    break;
                }
                
            } while (indexCounter != currentIndex && currentIndex > 0);
        }
        
        public void LastPlayer()
        {
            if (!Enabled) return;

            ChangeSpectatedPlayer(DecreasePredicate);
            return;

            int DecreasePredicate(int indexCounter, int length)
            {
                indexCounter--;
                if (indexCounter == -1)
                    indexCounter = length - 1;
                return indexCounter;
            }
        }
        public void NextPlayer()
        {
            if (!Enabled) return;

            ChangeSpectatedPlayer(IncreasePredicate);
            return;

            int IncreasePredicate(int indexCounter, int length)
            {
                indexCounter++;
                if (indexCounter == length)
                    indexCounter = 0;
                return indexCounter;
            }
        }
    }
}