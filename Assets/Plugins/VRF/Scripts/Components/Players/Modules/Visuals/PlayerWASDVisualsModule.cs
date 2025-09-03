using System;
using UnityEngine;
using VRF.Players.Hands;
using VRF.Players.Models;

namespace VRF.Components.Players.Modules.Visuals
{
    public class PlayerWASDVisualsModule : BasePlayerVisualsModule
    {
        [SerializeField] private HandSkinsController leftHand;
        [SerializeField] private HandSkinsController rightHand;
        
        protected override void InitializeAppearance(PlayerSpawnConfiguration spawnConfiguration)
        {
            // TODO доделать WASD network visual, IK
            
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
                        if(leftHand.ActiveSkin)
                            leftHand.ActiveSkin.SetNet();
                        if(rightHand.ActiveSkin)
                            rightHand.ActiveSkin.SetNet();
                    }
                    
                    SetActiveModels(true);
                    break;
                case PlayerVisualMode.SkeletonIK:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}