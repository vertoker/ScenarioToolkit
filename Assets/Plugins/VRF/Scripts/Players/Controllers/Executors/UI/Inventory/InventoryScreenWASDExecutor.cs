using SimpleUI.Core;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Core;
using VRF.Players.Controllers.Executors.UI.Inventory.Subscriptions;

namespace VRF.Players.Controllers.Executors.UI.Inventory
{
    public class InventoryScreenWASDExecutor : InventoryScreenExecutor<PlayerWASDView, ClickItemSelectSubscription>
    {
        protected override Grabber SpawnGrabber => playerView.VirtualHandGrabber;

        public InventoryScreenWASDExecutor(ScreensManager manager, PlayerWASDView playerView, InventoryController inventoryController) 
            : base(manager, playerView, inventoryController)
        {
        }
    }
}