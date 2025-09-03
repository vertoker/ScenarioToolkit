using System;
using NaughtyAttributes;
using UnityEngine;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [Serializable]
    public class VirtualHandModel : IPlayerModel
    {
        [field:SerializeField] public bool Enabled { get; set; } = true;
        [field:SerializeField] public float GrabberRadius { get; set; } = 0.05f;
        [field:Range(0, 1)]
        [field:SerializeField] public float LerpPoint { get; set; } = 0.8f;
        
        [field:Header("Raycast")]
        [field:Min(0)] [field:HideIf(nameof(ImmutableDistance))] [field:AllowNesting]
        [field:SerializeField] public float MinDistance { get; set; } = 0.5f;
        [field:Min(0)]
        [field:SerializeField] public float MaxDistance { get; set; } = 3f;
        [field:Min(0)] [field:HideIf(nameof(ImmutableDistance))] [field:AllowNesting]
        [field:SerializeField] public float ZoomDelta { get; set; } = 0.25f;
        [field:SerializeField] public bool ImmutableDistance { get; set; } = false;
        [field:SerializeField] public LayerMask Mask { get; set; } = default;
        [field:SerializeField] public QueryTriggerInteraction TriggerInteraction { get; set; }
            = QueryTriggerInteraction.UseGlobal;
        
        public Vector3 GetPositionByRay(Ray ray) => ray.origin + ray.direction.normalized * MaxDistance;
        
        [field:Header("Item Rotation")]
        [field:SerializeField] public float ItemForwardRotationSpeed { get; set; } = 180;
        [field:SerializeField] public float ItemMouseRotationSpeed { get; set; } = 10;
        [field:SerializeField] public float MaxMouseDeltaMagnitude { get; set; } = 5;
        
        [field:Header("Grabbable Overrides")]
        [field:SerializeField] public bool OverrideVelocityForce { get; set; } = false;
        [field:ShowIf(nameof(OverrideVelocityForce))] [field:AllowNesting]
        [field:SerializeField] public float VelocityForce { get; set; } = 500f;
        
        [field:SerializeField] public bool OverrideAngularVelocityForce { get; set; } = false;
        [field:ShowIf(nameof(OverrideAngularVelocityForce))] [field:AllowNesting]
        [field:SerializeField] public float AngularVelocityForce { get; set; } = 10f;
        
        [field:SerializeField] public bool OverrideThrowForceMultiplier { get; set; } = true;
        [field:ShowIf(nameof(OverrideThrowForceMultiplier))] [field:AllowNesting]
        [field:SerializeField] public float ThrowForceMultiplier { get; set; } = 0.6f;
        
        [field:SerializeField] public bool OverrideThrowForceMultiplierAngular { get; set; } = true;
        [field:ShowIf(nameof(OverrideThrowForceMultiplierAngular))] [field:AllowNesting]
        [field:SerializeField] public float ThrowForceMultiplierAngular { get; set; } = 0.8f;
    }
}