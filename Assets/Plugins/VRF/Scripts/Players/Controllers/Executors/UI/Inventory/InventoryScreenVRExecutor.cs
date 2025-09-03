using SimpleUI.Core;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Core;
using VRF.Players.Controllers.Executors.UI.Inventory.Subscriptions;

namespace VRF.Players.Controllers.Executors.UI.Inventory
{
    public class InventoryScreenVRExecutor : InventoryScreenExecutor<PlayerVRView, GrabItemSelectSubscription>
    {
        protected override Grabber SpawnGrabber => playerView.RightGrabber;

        public InventoryScreenVRExecutor(ScreensManager manager, PlayerVRView playerView, InventoryController inventoryController)
            : base(manager, playerView, inventoryController)
        {
        }
    }
}