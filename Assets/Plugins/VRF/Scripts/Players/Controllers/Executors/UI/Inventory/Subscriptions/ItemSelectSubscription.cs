using System;
using VRF.Inventory.UI;
using VRF.Utils;

namespace VRF.Players.Controllers.Executors.UI.Inventory.Subscriptions
{
    public abstract class ItemSelectSubscription : Subscription<PageIconBind>
    {
        protected ItemSelectSubscription(PageIconBind bind, Action<PageIconBind> callback) : base(bind, callback)
        {
        }
    }
}