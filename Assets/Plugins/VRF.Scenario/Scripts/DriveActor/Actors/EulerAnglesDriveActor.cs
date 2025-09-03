using UnityEngine;
using VRF.Scenario.DriveActor.Core;

namespace VRF.Scenario.DriveActor.Actors
{
    public class EulerAnglesDriveActor : BaseTransformDriveActor
    {
        [SerializeField] private Axis3 axis = Axis3.AxisX;
        [SerializeField] private float startAngle = 0;
        [SerializeField] private float endAngle = 180;

        protected override void UpdateValue()
        {
            var angles = EulerAngles;
            angles[(int)axis] = Mathf.LerpUnclamped(startAngle, endAngle, Value);
            EulerAngles = angles;
        }
    }
}