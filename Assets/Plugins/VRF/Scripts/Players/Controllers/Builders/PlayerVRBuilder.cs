using SimpleUI.Anim;
using UnityEngine;
using UnityEngine.XR.Management;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Core;
using VRF.Players.Controllers.Executors.Interaction;
using VRF.Players.Controllers.Executors.UI;
using VRF.Players.Controllers.Executors.UI.Inventory;
using VRF.Players.Controllers.Scriptables;
using VRF.Players.Controllers.Utility;
using VRF.UI.GameMenu;
using Zenject;

namespace VRF.Players.Controllers.Builders
{
    public class PlayerVRBuilder : BasePlayerBuilder<PlayerVRView, SchemeVR, PlayerVRConfig>
    {
        public HandViewExecutor HandViewExecutor { get; }
        
        public PlayerVRBuilder([InjectOptional] PlayerVRView view, SchemeVR controlScheme, PlayerVRConfig config, InventoryController inventoryController) 
            : base(view, controlScheme, config)
        {
            if (!view) return;

            var manager = view.GameUI.Manager;
            var menuToggler = new ScreenToggler<GameMenuScreen>(manager, ControlScheme.XRLeftHand.Menu);
            AddExecutor(menuToggler);

            var inventoryExecutor = new InventoryScreenVRExecutor(manager, view, inventoryController);
            AddExecutor(inventoryExecutor);

            HandViewExecutor = new HandViewExecutor(config.HandView, view);
            AddExecutor(HandViewExecutor);
        }

        public override void BuilderInitialize()
        {
            if (XRGeneralSettings.Instance)
            {
                var manager = XRGeneralSettings.Instance.Manager;
                if (manager && !manager.isInitializationComplete)
                {
                    manager.InitializeLoaderSync();
                    manager.StartSubsystems();
                }
            }
            else
            {
                Debug.LogError("XR is not loaded, please restart app");
            }
            
            base.BuilderInitialize();
        }

        public override void BuilderDispose()
        {
            base.BuilderDispose();
            
            if (XRGeneralSettings.Instance)
            {
                var manager = XRGeneralSettings.Instance.Manager;
                if (manager && manager.isInitializationComplete)
                {
                    manager.StopSubsystems();
                    manager.DeinitializeLoader();
                }
            }
        }
    }
}