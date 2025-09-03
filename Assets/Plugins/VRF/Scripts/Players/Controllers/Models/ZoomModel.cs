using System;
using NaughtyAttributes;
using UnityEngine;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [Serializable]
    public class ZoomModel : IPlayerModel
    {
        [field:SerializeField] public bool Enabled { get; set; } = true;
        
        [field:SerializeField] public int MinZoom { get; set; } = 2;
        [field:SerializeField] public int MaxZoom { get; set; } = 6;
        [field:SerializeField] public float DefaultFOV { get; set; } = 75;
        [field:SerializeField] public float EachZoomFOV { get; set; } = 5;
        [field:Range(0, 1)] [field:AllowNesting]
        [field:SerializeField] public float LerpZoom { get; set; } = 0.5f;
    }
}