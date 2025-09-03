using System;
using NaughtyAttributes;
using SimpleUI.Core;
using UnityEngine;
using UnityEngine.UI;
using VRF.Components.Players.Views.NetPlayer;
using VRF.Networking.Players;
using VRF.Players.Controllers.Executors.Movement;
using Zenject;

namespace VRF.Players.Controllers.Builders.UI
{
    public class SpectatorUIView : UIView
    {
        [SerializeField] private GameObject left;
        [SerializeField] private GameObject right;
        [SerializeField, ReadOnly] private Button leftBtn;
        [SerializeField, ReadOnly] private Button rightBtn;
        
        public Button LeftBtn => leftBtn;
        public Button RightBtn => rightBtn;
        
        public void SetActiveLeft(bool active) => left.SetActive(active);
        public void SetActiveRight(bool active) => right.SetActive(active);
        
        public override void OnValidate()
        {
            base.OnValidate();
            if (left) leftBtn = left.GetComponent<Button>();
            if (right) rightBtn = right.GetComponent<Button>();
        }

        public override Type GetControllerType() => typeof(SpectatorUIController);
    }
    
    public class SpectatorUIController : UIController<SpectatorUIView>, IInitializable, IDisposable
    {
        private readonly NetPlayersContainerService _netPlayersContainer;
        private readonly SpectateExecutor _spectatePlayers;

        public SpectatorUIController(SpectatorUIView view, 
            [InjectOptional] NetPlayersContainerService netPlayersContainer,
            [InjectOptional] PlayerSpectatorWASDBuilder playerBuilder) : base(view)
        {
            _netPlayersContainer = netPlayersContainer;
            if (playerBuilder != null)
                _spectatePlayers = playerBuilder.SpectateExecutor;
        }

        public void Initialize()
        {
            if (_spectatePlayers == null) return;
            if (_netPlayersContainer == null) return;
            
            _netPlayersContainer.OnAddPlayer += OnUpdatePlayers;
            _netPlayersContainer.OnRemovePlayer += OnUpdatePlayers;
            
            if (View.LeftBtn) View.LeftBtn.onClick.AddListener(_spectatePlayers.LastPlayer);
            if (View.RightBtn) View.RightBtn.onClick.AddListener(_spectatePlayers.NextPlayer);
            
            UpdateViewVisible();
        }

        public void Dispose()
        {
            if (_spectatePlayers == null) return;
            if (_netPlayersContainer == null) return;
            
            _netPlayersContainer.OnAddPlayer -= OnUpdatePlayers;
            _netPlayersContainer.OnRemovePlayer -= OnUpdatePlayers;
            
            if (View.LeftBtn) View.LeftBtn.onClick.RemoveListener(_spectatePlayers.LastPlayer);
            if (View.RightBtn) View.RightBtn.onClick.RemoveListener(_spectatePlayers.NextPlayer);
        }

        private void OnUpdatePlayers(BaseNetPlayerView player) => UpdateViewVisible();
        private void UpdateViewVisible()
        {
            var condition = _netPlayersContainer.CanSwitchSpectatePlayers();
            View.SetActiveLeft(condition);
            View.SetActiveRight(condition);
        }
    }
}