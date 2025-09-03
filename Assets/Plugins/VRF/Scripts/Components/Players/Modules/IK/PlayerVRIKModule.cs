using UnityEngine;
using VRF.Components.Players.Modules.Visuals;
using VRF.Players.Hands;
using VRF.Utilities.Extensions;

namespace VRF.Components.Players.Modules.IK
{
    public class PlayerVRIKModule : BasePlayerIKModule
    {
        [Header("Receive")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform head;
        [SerializeField] private Transform headLook;
        [SerializeField] private Transform leftController;
        [SerializeField] private Transform rightController;
        [Space]
        [SerializeField] private HandPoserUpdater leftHand;
        [SerializeField] private HandPoserUpdater rightHand;

        public Transform Player => player;
        public Transform Head => head;
        public Transform HeadLook => headLook;
        public Transform LeftController => leftController;
        public Transform RightController => rightController;
        
        protected override void UpdateIK(BasePlayerVisualsModule baseModule)
        {
            var module = (PlayerVRVisualsModule)baseModule;
            
            module.PlayerController.SyncPosTo(player, true, false, true);
            //module.PlayerController.SyncRotTo(player);
            
            module.HeadLook.SyncPosTo(headLook);
            module.HeadLook.SyncRotTo(headLook);
            
            module.Head.SyncPosTo(head);
            module.Head.SyncRotTo(head);

            if (module.LeftHand.ActiveSkin)
            {
                var skin = module.LeftHand.ActiveSkin;
                leftHand.SyncFrom(skin.HandPoserUpdater, SyncPosition, SyncRotation, SyncScale);
                skin.IKPoint.SyncPosTo(leftController);
                skin.IKPoint.SyncRotTo(leftController);
            }

            if (module.RightHand.ActiveSkin)
            {
                var skin = module.RightHand.ActiveSkin;
                rightHand.SyncFrom(skin.HandPoserUpdater, SyncPosition, SyncRotation, SyncScale);
                skin.IKPoint.SyncPosTo(rightController);
                skin.IKPoint.SyncRotTo(rightController);
            }
        }
    }
}