using NaughtyAttributes;
using UnityEngine;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [System.Serializable]
    public class HandViewModel : IPlayerModel
    {
        [field:SerializeField] public bool Enabled { get; set; } = true;
        
        [field:SerializeField] public bool DisablePhysicsWhenRelease { get; set; } = true;
        
        [field:AllowNesting] [field:Min(1)] [field:ShowIf(nameof(DisablePhysicsWhenRelease))]
        [field:SerializeField] public int DisablePhysicsFrames { get; set; } = 1;
        
    }
}