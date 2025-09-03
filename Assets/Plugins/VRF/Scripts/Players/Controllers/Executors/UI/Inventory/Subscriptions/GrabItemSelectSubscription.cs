using System;
using Cysharp.Threading.Tasks;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Inventory.UI;

namespace VRF.Players.Controllers.Executors.UI.Inventory.Subscriptions
{
    public class GrabItemSelectSubscription : ItemSelectSubscription
    {
        private readonly Grabbable grabbable;
        
        public GrabItemSelectSubscription(PageIconBind bind, Action<PageIconBind> callback) : base(bind, callback)
        {
            grabbable = bind.ViewUI.Grabbable;
        }

        public override void Initialize()
        {
            grabbable.OnGrabbed += InvokeCallback;
            grabbable.OnGrabbed += ResetGrabbable;
        }
        
        public override void Dispose()
        {
            grabbable.OnGrabbed -= InvokeCallback;
            grabbable.OnGrabbed -= ResetGrabbable;
        }
        
        private async void ResetGrabbable()
        {
            //нужно подождать, когда предмет будет действительно взят
            await UniTask.Yield();
            
            if (!bind.ViewUI) return;
            // Эта строчка забрала у меня 4 часа жизни, код BNG просто пиздец нечитаемый
            grabbable.BeingHeld = false;
            grabbable.HeldByGrabbers.Clear();
        }
    }
}