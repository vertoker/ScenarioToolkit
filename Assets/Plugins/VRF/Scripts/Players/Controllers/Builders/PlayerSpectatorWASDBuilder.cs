using SimpleUI.Core;
using UnityEngine;
using VRF.Components.Players.Views.Player;
using VRF.Networking.Players;
using VRF.Players.Controllers.Executors.Interaction;
using VRF.Players.Controllers.Executors.Management;
using VRF.Players.Controllers.Executors.Movement;
using VRF.Players.Controllers.Executors.UI;
using VRF.Players.Controllers.Executors.Utility;
using VRF.Players.Controllers.Models.Enums;
using VRF.Players.Controllers.Models.Validators;
using VRF.Players.Controllers.Scriptables;
using VRF.Players.Controllers.Utility;
using VRF.Players.Services;
using VRF.Players.Services.Settings;
using VRF.UI.GameMenu;
using VRF.UI.GameMenu.Screens;

namespace VRF.Players.Controllers.Builders
{
    public class PlayerSpectatorWASDBuilder : BasePlayerBuilder<PlayerSpectatorWASDView, 
        SchemeWASD, PlayerSpectatorWASDConfig>
    {
        // Menu UI
        private readonly ScreensManager manager;
        private readonly MenuToggler<GameMenuScreen> menuToggler;
        // Management
        public PlayerModeContainer PlayerModeContainer { get; }
        // Observe
        public MouseExecutor MouseExecutor { get; }
        public ZoomExecutor ZoomExecutor { get; }
        // Spectate
        public SpectateExecutor SpectateExecutor { get; }
        public SpectateModeSwitcher SpectateModeSwitcher { get; }
        // Fly
        public FlyHeightExecutor FlyHeightExecutor { get; }
        public FlyMovementExecutor FlyMovementExecutor { get; }
        
        public PlayerSpectatorWASDBuilder(MouseSensitivityParameter mouseSensitivity,
            NetPlayersContainerService netContainer, CursorHideoutService hideoutService,
            PlayerSpectatorWASDView view, SchemeWASD controlScheme,
            PlayerSpectatorWASDConfig config) : base(view, controlScheme, config)
        {
            manager = view.GameUI.Manager;
            
            menuToggler = new MenuToggler<GameMenuScreen>(manager, hideoutService, controlScheme.UI.Menu);
            AddExecutor(menuToggler);
            
            PlayerModeContainer = new PlayerModeContainer(config.PlayerMode, controlScheme.UI.SwitchMode);

            #region Executors
            
            MouseExecutor = new MouseExecutor(config.Mouse, view.CameraPivot, mouseSensitivity, controlScheme.Mouse.Delta);
            PlayerModeContainer.Add(PlayerMode.Fly, MouseExecutor); AddExecutor(MouseExecutor);
            
            ZoomExecutor = new ZoomExecutor(config.Zoom, view.Camera, controlScheme.Mouse.RightButton);
            PlayerModeContainer.Add(PlayerMode.Fly, ZoomExecutor); AddExecutor(ZoomExecutor);
            
            SpectateExecutor = new SpectateExecutor(config.Spectate, view.CameraPivot, netContainer, manager,
                controlScheme.UI.LastPlayer, controlScheme.UI.NextPlayer);
            PlayerModeContainer.Add(SpectateExecutor); AddExecutor(SpectateExecutor);

            SpectateModeSwitcher = new SpectateModeSwitcher(config.Spectate, PlayerModeContainer, SpectateExecutor);
            AddExecutor(SpectateModeSwitcher);
            
            FlyHeightExecutor = new FlyHeightExecutor(config.Fly, view.PlayerRigidbody, 
                controlScheme.Keyboard.Up, controlScheme.Keyboard.Down, controlScheme.Keyboard.Acceleration);
            PlayerModeContainer.Add(FlyHeightExecutor); AddExecutor(FlyHeightExecutor);
            
            FlyMovementExecutor = new FlyMovementExecutor(config.FlySpeed, view.CameraPivot, view.PlayerRigidbody,
                controlScheme.Keyboard.Movement, null, controlScheme.Keyboard.Acceleration);
            PlayerModeContainer.Add(FlyMovementExecutor); AddExecutor(FlyMovementExecutor);
            
            var flyValidator = new ValidatorExecutor<RigidbodyModel, Rigidbody>(config.FlyRigidbody, view.PlayerRigidbody);
            var spectateValidator = new ValidatorExecutor<RigidbodyModel, Rigidbody>(config.SpectateRigidbody, view.PlayerRigidbody);
            PlayerModeContainer.Add(PlayerMode.Fly, flyValidator);
            PlayerModeContainer.Add(PlayerMode.Spectate, spectateValidator);
            
            #endregion
            
            AddExecutor(PlayerModeContainer);
        }

        public override void BuilderInitialize()
        {
            menuToggler.OnOpen += PlayerModeContainer.Disable;
            menuToggler.OnClose += PlayerModeContainer.Enable;
            PlayerModeContainer.OnEnable += OnEnableContainer;
            PlayerModeContainer.OnDisable += OnDisableContainer;
            
            base.BuilderInitialize();
            
            PlayerModeContainer.Enable();
        }

        public override void BuilderDispose()
        {
            PlayerModeContainer.Disable();
            
            base.BuilderDispose();
            
            menuToggler.OnOpen -= PlayerModeContainer.Disable;
            menuToggler.OnClose -= PlayerModeContainer.Enable;
            PlayerModeContainer.OnEnable -= OnEnableContainer;
            PlayerModeContainer.OnDisable -= OnDisableContainer;
        }

        private void OnEnableContainer(PlayerMode mode)
        {
            if (mode == PlayerMode.Spectate)
            {
                menuToggler.HideoutActivity.Add(1, true);
            }
            else
            {
                manager.Open<GameWASDScreen>();
            }
        }
        private void OnDisableContainer()
        {
            menuToggler.HideoutActivity.Remove(1);
            if (manager.InList<GameWASDScreen>())
                manager.Close<GameWASDScreen>();
        }
    }
}