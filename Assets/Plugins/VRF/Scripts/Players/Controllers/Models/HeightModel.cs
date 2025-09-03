using UnityEngine;
using VRF.Players.Controllers.Models.Enums;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [System.Serializable]
    public class HeightModel : IPlayerModel
    {
        [field:SerializeField] public bool Enabled { get; set; } = true;
        [field:SerializeField] public ButtonPressMode CrouchBtnMode { get; set; } = ButtonPressMode.Hold;
        
        [field:SerializeField] public float HeightDown { get; set; } = 0.95f;
        [field:SerializeField] public float HeightUp { get; set; } = 0.5f;
        [field:SerializeField] public float HeightDownCrouch { get; set; } = 0.4f;
        [field:SerializeField] public float StandUpSpeed { get; set; } = 0.05f;
        [field:SerializeField] public float SitDownSpeed { get; set; } = 0.1f;
        [field:SerializeField] public float HeightEpsilon { get; set; } = 0.01f;
        
        public float GetHeightDown(bool crouch) => crouch ? HeightDownCrouch : HeightDown;
    }
}