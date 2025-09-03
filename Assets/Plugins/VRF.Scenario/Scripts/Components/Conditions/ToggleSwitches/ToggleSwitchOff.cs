using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions.ToggleSwitches
{
    [ScenarioMeta("ToggleSwitch стал Off", typeof(ToggleSwitchOn), typeof(ToggleSwitch))]
    public struct ToggleSwitchOff : IScenarioCondition
    {
        public ToggleSwitch ToggleSwitch;
    }
}