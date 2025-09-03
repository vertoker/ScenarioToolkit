using System;
using UnityEngine;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [Serializable]
    public class JumpModel : IPlayerModel
    {
        [field:SerializeField] public bool Enabled { get; set; } = true;
        [field:SerializeField] public float Force { get; set; } = 15f;
        [field:SerializeField] public float Cooldown { get; set; } = 0.55f;
    }
}