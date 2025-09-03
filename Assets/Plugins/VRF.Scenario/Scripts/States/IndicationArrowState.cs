using System.Collections.Generic;
using Scenario.Core.Systems.States;
using VRF.VRBehaviours;

namespace VRF.Scenario.States
{
    public class IndicationArrowState : IState
    {
        public Dictionary<IndicationArrow, Data> Arrows = new();
        
        public class Data : NetworkContinuousData
        {
            public float StartValue;
            public float TargetValue;

            public Data(float startValue, float targetValue, float seconds) : base(seconds)
            {
                StartValue = startValue;
                TargetValue = targetValue;
            }
        }

        public void Clear()
        {
            Arrows.Clear();
        }
    }
}