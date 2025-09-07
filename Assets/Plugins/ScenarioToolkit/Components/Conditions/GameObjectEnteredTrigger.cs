using UnityEngine;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    public struct GameObjectEnteredTrigger : IScenarioCondition
    {
        public GameObject GameObject;
        public TriggerZone TriggerZone;
    }
}