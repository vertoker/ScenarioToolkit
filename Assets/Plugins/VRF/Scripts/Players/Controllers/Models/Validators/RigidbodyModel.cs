using System;
using UnityEngine;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models.Validators
{
    [Serializable]
    public class RigidbodyModel : IComponentValidator<Rigidbody>
    {
        [field:SerializeField] public bool UseGravity { get; set; } = true;
        [field:SerializeField] public bool IsKinematic { get; set; } = false;
        [field:SerializeField] public float Mass { get; set; } = 10;
        [field:SerializeField] public float Drag { get; set; } = 0.5f;
        [field:SerializeField] public float AngularDrag { get; set; } = 0.05f;
        [field:SerializeField] public CollisionDetectionMode CollisionDetectionMode { get; set; } 
            = CollisionDetectionMode.Continuous;
        [field:SerializeField] public bool FreezeRotation { get; set; } = true;
        
        public void Validate(Rigidbody component)
        {
            if (!component) return;
            component.useGravity = UseGravity;
            component.isKinematic = IsKinematic;
            component.mass = Mass;
            component.linearDamping = Drag;
            component.angularDamping = AngularDrag;
            component.freezeRotation = FreezeRotation;
            component.collisionDetectionMode = CollisionDetectionMode;
        }
    }
}