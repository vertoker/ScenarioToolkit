using System.Collections.Generic;
using ScenarioToolkit.Core.Systems.States;
using UnityEngine;

namespace ScenarioToolkit.Library.States
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