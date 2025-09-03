using System;
using UnityEngine;
using VRF.Components.Players.Modules.IK;
using VRF.Players.Hands;
using VRF.Players.Models;

namespace VRF.Components.Players.Modules.Visuals
{
    /// <summary>
    /// Модуль для визуала VR игрока (сетевого и локального)
    /// </summary>
    public class PlayerVRVisualsModule : BasePlayerVisualsModule
    {
        [Header("IK")]
        [SerializeField] private Transform playerController;
        [SerializeField] private Transform headLook;
        [SerializeField] private Transform head;
        [Space]
        [SerializeField] private HandSkinsController leftHand;
        [SerializeField] private HandSkinsController rightHand;
        
        public Transform PlayerController => playerController;
        public Transform HeadLook => headLook;
        public Transform Head => head;
        
        public HandSkinsController LeftHand => leftHand;
        public HandSkinsController RightHand => rightHand;

        protected override void InitializeAppearance(PlayerSpawnConfiguration spawnConfiguration)
        {
            var configuration = PlayerView.IsNetPlayer 
                ? spawnConfiguration.VisualNetworkPlayer
                : spawnConfiguration.VisualLocalPlayer;

            switch (configuration.VisualMode)
            {
                case PlayerVisualMode.NoGraphics:
                    break;
                case PlayerVisualMode.ControllerModels:
                    leftHand.CreateSkin(configuration.HandSkinLeft);
                    rightHand.CreateSkin(configuration.HandSkinRight);
                    
                    if (PlayerView.IsNetPlayer)
                    {
                        if (leftHand.ActiveSkin)
                            leftHand.ActiveSkin.SetNet();
                        if (rightHand.ActiveSkin)
                            rightHand.ActiveSkin.SetNet();
                    }
                    
                    SetActiveModels(true);
                    break;
                case PlayerVisualMode.SkeletonIK:
                    var viewIK = Spawner.SpawnView(configuration.SkeletonIK);
                    viewIK.Initialize(View);
                    
                    if (viewIK.TryGetComponent<BasePlayerIKModule>(out var moduleIK))
                        ModuleIK = moduleIK;
                        
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}