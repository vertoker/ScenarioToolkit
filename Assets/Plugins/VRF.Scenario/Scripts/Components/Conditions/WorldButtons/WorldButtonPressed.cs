using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

namespace VRF.Scenario.Components.Conditions.WorldButtons
{
    [ScenarioMeta("Была зажата WorldButton", typeof(WorldButton))]
    public struct WorldButtonPressed : IScenarioCondition
    {
        public WorldButton WorldButton;
    }
}