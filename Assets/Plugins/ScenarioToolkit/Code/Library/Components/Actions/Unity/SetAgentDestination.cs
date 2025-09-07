using Scenario.Core.Model.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Scenario.Components.Actions.Unity
{
    public struct SetAgentDestination : IScenarioAction, IComponentDefaultValues
    {
        public NavMeshAgent Agent;
        public Transform Destination;
        public float Speed;
        public bool DisableAgentAfterDestinationReached;

        public void SetDefault()
        {
            Speed = 3.5f;
        }
    }
}