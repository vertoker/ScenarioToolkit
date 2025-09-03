using NaughtyAttributes;
using UnityEngine;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Validators;

namespace VRF.Players.Controllers.Scriptables
{
    [CreateAssetMenu(fileName = nameof(PlayerSpectatorWASDConfig), menuName = "VRF/Player/" + nameof(PlayerSpectatorWASDConfig))]
    public class PlayerSpectatorWASDConfig : BaseLocalPlayerConfig
    {
        [field: SerializeField] public MouseModel Mouse { get; set; } = new();
        [field: SerializeField] public ZoomModel Zoom { get; set; } = new();
        
        [field:Space]
        [field:SerializeField] public PlayerModeModel PlayerMode { get; set; } 
            = new(false, true, true);
        
        [field:Header("Fly")]
        [field:SerializeField] public RigidbodyModel FlyRigidbody { get; set; } = new();
        [field:SerializeField] public SpeedModel FlySpeed { get; set; } = new();
        [field:SerializeField] public FlyModel Fly { get; set; } = new();

        [field:Header("Spectate")]
        [field:SerializeField] public RigidbodyModel SpectateRigidbody { get; set; } = new();
        [field:SerializeField] public SpectateModel Spectate { get; set; } = new();
    }
}