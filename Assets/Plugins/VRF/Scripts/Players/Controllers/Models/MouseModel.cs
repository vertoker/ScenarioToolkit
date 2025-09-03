using NaughtyAttributes;
using UnityEngine;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [System.Serializable]
    public class MouseModel : IPlayerModel
    {
        [field:SerializeField] public bool Enabled { get; set; } = true;
        
        [field:MinMaxSlider(-90, 90)] [field:AllowNesting]
        [field:SerializeField] public Vector2 Clamp { get; set; } = new(-89f, 89f);
    }
}