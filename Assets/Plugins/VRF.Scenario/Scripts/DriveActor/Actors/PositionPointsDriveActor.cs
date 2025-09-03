using UnityEngine;
using VRF.Scenario.DriveActor.Core;

namespace VRF.Scenario.DriveActor.Actors
{
    public class PositionPointsDriveActor : BaseTransformDriveActor
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        
        protected override void UpdateValue()
        {
            if (!startPoint) return;
            if (!endPoint) return;
            Position = Vector3.LerpUnclamped(GetPosition(startPoint), GetPosition(endPoint), Value);
        }
    }
}