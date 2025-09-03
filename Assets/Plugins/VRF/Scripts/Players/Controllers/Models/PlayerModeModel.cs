using System;
using System.Threading;
using UnityEngine;
using VRF.Players.Controllers.Models.Enums;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [Serializable]
    public class PlayerModeModel : IPlayerModel
    {
        [field:HideInInspector] [field:SerializeField] public bool IdleEnabled { get; set; }
        [field:HideInInspector] [field:SerializeField] public bool FlyEnabled { get; set; }
        [field:HideInInspector] [field:SerializeField] public bool SpectateEnabled { get; set; }
        [field:SerializeField] public PlayerMode ModeOnStart { get; set; }
        
        public bool IsIdle => ModeOnStart == PlayerMode.Idle && IdleEnabled;
        public bool IsFly => ModeOnStart == PlayerMode.Fly && FlyEnabled;
        public bool IsSpectate => ModeOnStart == PlayerMode.Spectate && SpectateEnabled;
        public bool Enabled => IdleEnabled || FlyEnabled || SpectateEnabled;
        
        public PlayerModeModel(bool idleEnabled, bool flyEnabled, bool spectateEnabled)
        {
            IdleEnabled = idleEnabled;
            FlyEnabled = flyEnabled;
            SpectateEnabled = spectateEnabled;
            
            ModeOnStart = idleEnabled ? PlayerMode.Idle 
                : flyEnabled ? PlayerMode.Fly 
                : PlayerMode.Spectate;
        }
        
        public PlayerMode ToggleSwitchMode(PlayerMode activeMode)
        {
            if (!Enabled)
                throw new LockRecursionException();
            
            return activeMode switch
            {
                PlayerMode.Idle => FlyEnabled ? PlayerMode.Fly : ToggleSwitchMode(PlayerMode.Fly),
                PlayerMode.Fly => SpectateEnabled ? PlayerMode.Spectate : ToggleSwitchMode(PlayerMode.Spectate),
                PlayerMode.Spectate => IdleEnabled ? PlayerMode.Idle : ToggleSwitchMode(PlayerMode.Idle),
                _ => throw new ArgumentOutOfRangeException(nameof(activeMode), activeMode, null)
            };
        }

    }
}