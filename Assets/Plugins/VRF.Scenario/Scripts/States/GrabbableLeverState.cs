using System;
using System.Collections.Generic;
using Scenario.Core.Systems.States;
using VRF.VRBehaviours;

namespace VRF.Scenario.States
{
    public class GrabbableLeverState : IState
    {
        public Dictionary<GrabbableLever, (LimitsData, LimitsData)> Limits = new();
        public Dictionary<GrabbableLever, (int, int)> TargetAngleIndexes = new();
        
        public struct LimitsData : IEquatable<LimitsData>
        {
            public float MinAngle;
            public float MaxAngle;

            public LimitsData(float minAngle, float maxAngle)
            {
                MinAngle = minAngle;
                MaxAngle = maxAngle;
            }

            public bool Equals(LimitsData other)
            {
                return MinAngle.Equals(other.MinAngle) && MaxAngle.Equals(other.MaxAngle);
            }

            public override bool Equals(object obj)
            {
                return obj is LimitsData other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(MinAngle, MaxAngle);
            }
        }

        public void Clear()
        {
            Limits.Clear();
            TargetAngleIndexes.Clear();
        }
    }
}