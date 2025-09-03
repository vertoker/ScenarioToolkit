using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions.ToggleSwitches
{
    [ScenarioMeta("ToggleSwitch стал On", typeof(ToggleSwitchOff), typeof(ToggleSwitch))]
    public struct ToggleSwitchOn : IScenarioCondition
    {
        public ToggleSwitch ToggleSwitch;
    }
}