using UnityEngine;
using VRF.Utilities.Extensions;

namespace VRF.Components.Players.Modules.Net
{
    public class NetPlayerWASDModule : BaseNetPlayerModule
    {
        [Header("Points")]
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        
        protected override void UpdateApply(BaseNetPlayerModule observer)
        {
            if (observer is NetPlayerWASDModule module)
            {
                CameraPivot.SyncFrom(module.CameraPivot, SyncPosition, SyncRotation, SyncScale);
                
                leftHand.SyncFrom(module.leftHand, SyncPosition, SyncRotation, SyncScale);
                rightHand.SyncFrom(module.rightHand, SyncPosition, SyncRotation, SyncScale);
            }
        }
    }
}