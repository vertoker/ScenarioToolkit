using System;
using NaughtyAttributes;
using UnityEngine;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Validators;

namespace VRF.Players.Controllers.Scriptables
{
    [CreateAssetMenu(fileName = nameof(PlayerWASDConfig), menuName = "VRF/Player/" + nameof(PlayerWASDConfig))]
    public class PlayerWASDConfig : BaseLocalPlayerConfig
    {
        [field:SerializeField] public MouseModel Mouse { get; set; } = new();
        [field:SerializeField] public ZoomModel Zoom { get; set; } = new();
        [field:SerializeField] public VirtualHandModel VirtualHand { get; set; } = new();
        
        [field:Space]
        [field:SerializeField] public PlayerModeModel PlayerMode { get; set; } 
            = new(true, true, false);
        
        [field:Header("Idle")]
        [field:SerializeField] public RigidbodyModel WalkRigidbody { get; set; } = new();
        [field:SerializeField] public SpeedModel WalkSpeed { get; set; } = new();
        [field:SerializeField] public HeightModel Height { get; set; } = new();
        [field:SerializeField] public JumpModel Jump { get; set; } = new();
        
        [field: Header("Fly")]
        [field: SerializeField] public RigidbodyModel FlyRigidbody { get; set; } = new();
        [field: SerializeField] public SpeedModel FlySpeed { get; set; } = new();
        [field:SerializeField] public FlyModel Fly { get; set; } = new();
    }
}