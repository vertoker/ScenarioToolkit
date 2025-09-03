using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Игрок вошёл в TriggerZone", typeof(TriggerZone))]
    public struct PlayerEnteredTrigger : IScenarioCondition
    {
        public TriggerZone TriggerZone;
    }
}