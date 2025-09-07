using Scenario.Core.Model.Interfaces;
using UnityEngine.AI;

// ReSharper disable once CheckNamespace
namespace Scenario.Components.Conditions
{
    public struct AgentReachedDestination : IScenarioCondition
    {
        public NavMeshAgent Agent;
    }
}