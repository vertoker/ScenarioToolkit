using System.Collections.Generic;
using Scenario.Core.Systems.States;
using VRF.VRBehaviours;

namespace VRF.Scenario.States
{
    public class TransformStatesState : IState
    {
        public Dictionary<GOTransformStates, (int, int)> States = new();
        
        public void Clear()
        {
            States.Clear();
        }
    }
}