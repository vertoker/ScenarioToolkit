using System.Collections.Generic;
using Scenario.Core.Systems.States;
using VRF.BNG_Framework.Scripts.Core;

namespace VRF.Scenario.States
{
    public class IsDroppableState : IState
    {
        public Dictionary<Grabbable, bool> Grabbables = new();
        
        public void Clear()
        {
            Grabbables.Clear();
        }
    }
}