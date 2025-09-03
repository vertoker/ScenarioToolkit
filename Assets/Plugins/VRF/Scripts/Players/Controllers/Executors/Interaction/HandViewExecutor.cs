using System;
using Cysharp.Threading.Tasks;
using VRF.BNG_Framework.Scripts.Core;
using VRF.BNG_Framework.Scripts.Helpers;
using VRF.Components.Players.Views.Player;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Models;
using Zenject;

namespace VRF.Players.Controllers.Executors.Interaction
{
    public class HandViewExecutor : BaseModelExecutor<HandViewModel>, IInitializable, IDisposable
    {
        private readonly PlayerVRView view;

        public HandViewExecutor(HandViewModel model, [InjectOptional] PlayerVRView view) : base(model)
        {
            this.view = view;
        }

        public void Initialize()
        {
            if (!view) return;
            view.LeftGrabber.onReleaseEvent.AddListener(OnReleaseLeft);
            view.RightGrabber.onReleaseEvent.AddListener(OnReleaseRight);
        }
        public void Dispose()
        {
            if (!view) return;
            view.LeftGrabber.onReleaseEvent.RemoveListener(OnReleaseLeft);
            view.RightGrabber.onReleaseEvent.RemoveListener(OnReleaseRight);
        }

        private void OnReleaseLeft(Grabbable grabbable)
        {
            var skin = view.LeftSkinsHand.ActiveSkin;
            if (skin && skin.HandPhysics)
                OnRelease(skin.HandPhysics);
        }
        private void OnReleaseRight(Grabbable grabbable)
        {
            var skin = view.RightSkinsHand.ActiveSkin;
            if (skin && skin.HandPhysics)
                OnRelease(skin.HandPhysics);
        }
        
        private async void OnRelease(HandPhysics handPhysics)
        {
            if (!Model.DisablePhysicsWhenRelease) return;
            
            handPhysics.DisableHandColliders();
            await UniTask.DelayFrame(Model.DisablePhysicsFrames);
            handPhysics.EnableHandColliders();
        }
    }
}