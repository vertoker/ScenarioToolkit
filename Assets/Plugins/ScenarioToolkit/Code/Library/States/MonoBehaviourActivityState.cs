using System.Collections.Generic;
using Scenario.Core.Systems.States;
using UnityEngine;

namespace Scenario.States
{
    public class MonoBehaviourActivityState : IState
    {
        public Dictionary<MonoBehaviour, bool> MonoBehaviours = new();
        
        public void Clear()
        {
            MonoBehaviours.Clear();
        }
    }
}