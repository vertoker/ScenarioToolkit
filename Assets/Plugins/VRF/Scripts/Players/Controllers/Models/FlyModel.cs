using System;
using NaughtyAttributes;
using UnityEngine;
using VRF.Players.Controllers.Models.Enums;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [Serializable]
    public class FlyModel : IPlayerModel
    {
        [field:SerializeField] public ButtonPressMode AccelerationBtnMode { get; set; } = ButtonPressMode.Hold;
        
        [field:EnableIf(nameof(Enabled))] [field:MinValue(0)] [field:AllowNesting] 
        [field:SerializeField] public float UpSpeed { get; set; } = 3;
        [field:EnableIf(nameof(Enabled))] [field:MinValue(0)] [field:AllowNesting] 
        [field:SerializeField] public float UpAccelerationSpeed { get; set; } = 7;
        [field:EnableIf(nameof(Enabled))] [field:MaxValue(0)] [field:AllowNesting] 
        [field:SerializeField] public float DownSpeed { get; set; } = -3;
        [field:EnableIf(nameof(Enabled))] [field:MaxValue(0)] [field:AllowNesting] 
        [field:SerializeField] public float DownAccelerationSpeed { get; set; } = -7;
        
        public bool IsAccelerationActive => AccelerationBtnMode != ButtonPressMode.None;
        public bool Enabled => IsAccelerationActive;
        
        public float GetUpSpeed(bool isAccelerated) => isAccelerated ? UpAccelerationSpeed : UpSpeed;
        public float GetDownSpeed(bool isAccelerated) => isAccelerated ? DownAccelerationSpeed : DownSpeed;
    }
}