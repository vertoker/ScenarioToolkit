using System;
using System.Collections.Generic;
using Scenario.Core.Systems.States;
using UnityEngine;

namespace Scenario.States
{
    public class HingeLimitsState : IState
    {
        // (default, current)
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public Dictionary<HingeJoint, (Data, Data)> Limits = new();
        
        public struct Data : IEquatable<Data>
        {
            // ReSharper disable once FieldCanBeMadeReadOnly.Global
            public float Min;
            // ReSharper disable once FieldCanBeMadeReadOnly.Global
            public float Max;

            public Data(float min, float max)
            {
                Min = min;
                Max = max;
            }

            public bool Equals(Data other)
            {
                return Min.Equals(other.Min) && Max.Equals(other.Max);
            }
            public override bool Equals(object obj)
            {
                return obj is Data other && Equals(other);
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(Min, Max);
            }
        }

        public void Clear()
        {
            Limits.Clear();
        }
    }
}