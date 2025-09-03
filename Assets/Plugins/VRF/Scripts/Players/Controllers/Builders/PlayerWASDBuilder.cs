using SimpleUI.Core;
using UnityEngine;
using VRF.BNG_Framework.Scripts.UI;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Core;
using VRF.Inventory.UI;
using VRF.Players.Controllers.Executors;
using VRF.Players.Controllers.Executors.Interaction;
using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Executors.Management;
using VRF.Players.Controllers.Executors.Movement;
using VRF.Players.Controllers.Executors.UI;
using VRF.Players.Controllers.Executors.UI.Inventory;
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
    public class PlayerWASDBuilder : BasePlayerBuilder<PlayerWASDView, SchemeWASD, PlayerWASDConfig>
    {
        // Menu UI
        private readonly ScreensManager manager;
        private readonly MenuToggler<GameMenuScreen> menuToggler;
        private readonly InventoryScreenWASDExecutor inventoryExecutor;

        // Management
        public PlayerModeContainer PlayerModeContainer { get; }
        public IModelExecutor[] ContainerExecutors { get; }
        // Observe
        public MouseExecutor MouseExecutor { get; }
        public ZoomExecutor ZoomExecutor { get; }
        public TeleportExecutor TeleportExecutor { get; }
        //Interact
        public VirtualHandExecutor VirtualHandExecutor { get; }
        public RaycastExecutor RaycastExecutor { get; }
        public HeadEquipmentExecutor HeadEquipmentExecutor{ get; }

        // Walk
        public HeightExecutor HeightExecutor { get; }
        public JumpExecutor JumpExecutor { get; }
        public WalkExecutor WalkExecutor { get; }
        // Fly
        public FlyHeightExecutor FlyHeightExecutor { get; }
        public FlyMovementExecutor FlyMovementExecutor { get; }
        
        public PlayerWASDBuilder(CursorHideoutService hideoutService, MouseSensitivityParameter mouseSensitivity,
            PlayerWASDView view, SchemeWASD controlScheme, PlayerWASDConfig config, 
            InventoryController inventoryController) : base(view, controlScheme, config)
        {
            manager = view.GameUI.Manager;
            
            menuToggler = new MenuToggler<GameMenuScreen>(manager, hideoutService, controlScheme.UI.Menu);
            AddExecutor(menuToggler);
            inventoryExecutor = new InventoryScreenWASDExecutor(manager, view, inventoryController);
            AddExecutor(inventoryExecutor);
            
            MouseExecutor = new MouseExecutor(config.Mouse, view.CameraPivot, mouseSensitivity, 
                controlScheme.Mouse.Delta, controlScheme.Keyboard.GrabbableRotationMode);
            ZoomExecutor = new ZoomExecutor(config.Zoom, view.Camera, controlScheme.Mouse.RightButton);
            VirtualHandExecutor = new VirtualHandExecutor(view, config.VirtualHand, hideoutService,
                controlScheme.Keyboard.Grab, controlScheme.Mouse.RightButton,
                controlScheme.Keyboard.GrabbableRotationMode, controlScheme.Keyboard.GrabbableForwardRotation,
                mouseSensitivity);
            TeleportExecutor = new TeleportExecutor(view.PlayerTeleport, controlScheme.Keyboard.Teleport);
            RaycastExecutor = new RaycastExecutor(view.Raycast);
            HeadEquipmentExecutor = new HeadEquipmentExecutor(view, controlScheme.Keyboard.Equip);
            
            ContainerExecutors = new IModelExecutor[] { MouseExecutor, ZoomExecutor, VirtualHandExecutor, RaycastExecutor, HeadEquipmentExecutor };
            AddExecutors(MouseExecutor, ZoomExecutor, VirtualHandExecutor, TeleportExecutor, RaycastExecutor, HeadEquipmentExecutor);
            
            PlayerModeContainer = new PlayerModeContainer(config.PlayerMode, controlScheme.UI.SwitchMode);

            #region Executors

            HeightExecutor = new HeightExecutor(config.Height, view.PlayerCollider, view.CameraPivot,
                controlScheme.Keyboard.Crouch);
            PlayerModeContainer.Add(HeightExecutor); AddExecutor(HeightExecutor);
            
            JumpExecutor = new JumpExecutor(config.Jump, view.PlayerRigidbody, view.GroundProvider,
                controlScheme.Keyboard.Jump);
            PlayerModeContainer.Add(JumpExecutor); AddExecutor(JumpExecutor);

            WalkExecutor = new WalkExecutor(config.WalkSpeed, view.CameraPivot, view.PlayerRigidbody,
                controlScheme.Keyboard.Movement, controlScheme.Keyboard.Crouch, controlScheme.Keyboard.Acceleration);
            PlayerModeContainer.Add(WalkExecutor); AddExecutor(WalkExecutor);
            
            FlyHeightExecutor = new FlyHeightExecutor(config.Fly, view.PlayerRigidbody, 
                controlScheme.Keyboard.Up, controlScheme.Keyboard.Down, controlScheme.Keyboard.Acceleration);
            PlayerModeContainer.Add(FlyHeightExecutor); AddExecutor(FlyHeightExecutor);
            
            FlyMovementExecutor = new FlyMovementExecutor(config.FlySpeed, view.CameraPivot, view.PlayerRigidbody,
                controlScheme.Keyboard.Movement, null, controlScheme.Keyboard.Acceleration);
            PlayerModeContainer.Add(FlyMovementExecutor); AddExecutor(FlyMovementExecutor);
            
            var walkValidator = new ValidatorExecutor<RigidbodyModel, Rigidbody>(config.WalkRigidbody, view.PlayerRigidbody);
            var flyValidator = new ValidatorExecutor<RigidbodyModel, Rigidbody>(config.FlyRigidbody, view.PlayerRigidbody);
            PlayerModeContainer.Add(PlayerMode.Idle, walkValidator);
            PlayerModeContainer.Add(PlayerMode.Fly, flyValidator);
            
            #endregion
            
            AddExecutor(PlayerModeContainer);
        }

        public override void BuilderInitialize()
        {
            menuToggler.OnOpen += OnMenuOpen;
            menuToggler.OnClose += OnMenuClose;
            
            inventoryExecutor.ItemSelected += InventoryExecutor_OnItemSelected;
            
            base.BuilderInitialize();
            EnableInternal();
        }

        public override void BuilderDispose()
        {
            DisableInternal();
            base.BuilderDispose();
            
            menuToggler.OnOpen -= OnMenuOpen;
            menuToggler.OnClose -= OnMenuClose;
            
            inventoryExecutor.ItemSelected -= InventoryExecutor_OnItemSelected;
        }

        private void OnMenuOpen()
        {
            manager.Close<GameWASDScreen>();
            VRUISystem.Instance.SetCameraCasterActivity(false);
            DisableInternal();
        }

        private void OnMenuClose()
        {
            manager.Open<GameWASDScreen>();
            VRUISystem.Instance.SetCameraCasterActivity(true);
            EnableInternal();
        }

        private void InventoryExecutor_OnItemSelected()
        {
            manager.Close<InventoryScreen>();
            menuToggler.Toggle();
        }

        private void EnableInternal()
        {
            PlayerModeContainer.Enable();
            foreach (var executor in ContainerExecutors)
                executor.Enable();
        }
        private void DisableInternal()
        {
            PlayerModeContainer.Disable();
            foreach (var executor in ContainerExecutors)
                executor.Disable();
        }
    }
}