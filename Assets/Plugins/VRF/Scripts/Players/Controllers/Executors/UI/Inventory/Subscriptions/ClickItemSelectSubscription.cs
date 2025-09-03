using System;
using VRF.Inventory.UI;

namespace VRF.Players.Controllers.Executors.UI.Inventory.Subscriptions
{
    public class ClickItemSelectSubscription : ItemSelectSubscription
    {
        public ClickItemSelectSubscription(PageIconBind bind, Action<PageIconBind> callback) : base(bind, callback)
        {
        }

        public override void Initialize()
        {
            bind.ViewUI.Button.onClick.AddListener(InvokeCallback);
        }

        public override void Dispose()
        {
            bind.ViewUI.Button.onClick.RemoveListener(InvokeCallback);
        }
    }
}