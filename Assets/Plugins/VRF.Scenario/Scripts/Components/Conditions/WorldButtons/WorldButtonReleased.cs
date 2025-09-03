using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

namespace VRF.Scenario.Components.Conditions.WorldButtons
{
    [ScenarioMeta("Была отжата WorldButton", typeof(WorldButton))]
    public struct WorldButtonReleased : IScenarioCondition
    {
        public WorldButton WorldButton;
    }
}