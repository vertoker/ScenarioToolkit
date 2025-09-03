using System;
using NaughtyAttributes;
using UnityEngine;
using VRF.Players.Controllers.Models.Enums;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [Serializable]
    public class SpeedModel : ISpeedModel
    {
        [field:SerializeField] public ButtonPressMode CrouchBtnMode { get; set; }
        [field:SerializeField] public ButtonPressMode AccelerationBtnMode { get; set; }
        
        [field:SerializeField] public float Speed { get; set; }
        [field:EnableIf(nameof(IsCrouchActive))] [field:AllowNesting]
        [field:SerializeField] public float CrouchSpeed { get; set; }
        [field:EnableIf(nameof(IsAccelerationActive))] [field:AllowNesting]
        [field:SerializeField] public float AccelerationSpeed { get; set; }
        [field:EnableIf(nameof(IsCrouchAccelerationActive))] [field:AllowNesting]
        [field:SerializeField] public float CrouchAccelerationSpeed { get; set; }

        public bool IsCrouchActive => CrouchBtnMode != ButtonPressMode.None;
        public bool IsAccelerationActive => AccelerationBtnMode != ButtonPressMode.None;
        public bool IsCrouchAccelerationActive => IsCrouchActive && IsAccelerationActive;
        public bool Enabled => IsCrouchActive || IsAccelerationActive;

        public SpeedModel(ButtonPressMode crouchBtnMode = ButtonPressMode.Hold,
            ButtonPressMode accelerationBtnMode = ButtonPressMode.Hold, 
            float speed = 4f, 
            float crouchSpeed = 0.8f,
            float accelerationSpeed = 12f, 
            float crouchAccelerationSpeed = 1.6f)
        {
            CrouchBtnMode = crouchBtnMode;
            AccelerationBtnMode = accelerationBtnMode;
            Speed = speed;
            CrouchSpeed = crouchSpeed;
            AccelerationSpeed = accelerationSpeed;
            CrouchAccelerationSpeed = crouchAccelerationSpeed;
        }

        public float GetSpeed(bool isAccelerated, bool isCrouching) => isAccelerated
            ? isCrouching ? CrouchAccelerationSpeed : AccelerationSpeed 
            : isCrouching ? CrouchSpeed : Speed;
        
        public float GetShiftSpeed(bool isAccelerated) => isAccelerated ? AccelerationSpeed : Speed;
        public float GetCrouchSpeed(bool isCrouching) => isCrouching ? CrouchSpeed : Speed;
    }
}