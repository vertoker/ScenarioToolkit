using UnityEngine;
using VRF.Players.Hands;
using VRF.Utilities.Extensions;

namespace VRF.Components.Players.Modules.Net
{
    public class NetPlayerVRModule : BaseNetPlayerModule
    {
        [Header("Points")]
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform player;
        
        [Space]
        [SerializeField] private HandSkinsController leftHandFingers;
        [SerializeField] private HandSkinsController rightHandFingers;
        
        protected override void UpdateApply(BaseNetPlayerModule observer)
        {
            if (observer is NetPlayerVRModule module)
            {
                CameraPivot.SyncFrom(module.CameraPivot, SyncPosition, SyncRotation, SyncScale);
                
                player.SyncFrom(module.player, SyncPosition, SyncRotation, SyncScale);
                leftHand.SyncFrom(module.leftHand, SyncPosition, SyncRotation, SyncScale);
                rightHand.SyncFrom(module.rightHand, SyncPosition, SyncRotation, SyncScale);

                if (leftHandFingers.ActiveSkin && module.leftHandFingers.ActiveSkin)
                {
                    var observerLeft = module.leftHandFingers.ActiveSkin.HandPoserUpdater;
                    var receiverLeft = leftHandFingers.ActiveSkin.HandPoserUpdater;
                    receiverLeft.SyncFrom(observerLeft);
                }
                if (rightHandFingers.ActiveSkin && module.rightHandFingers.ActiveSkin)
                {
                    var observerRight = module.rightHandFingers.ActiveSkin.HandPoserUpdater;
                    var receiverRight = rightHandFingers.ActiveSkin.HandPoserUpdater;
                    receiverRight.SyncFrom(observerRight);
                }
            }
        }
    }
}