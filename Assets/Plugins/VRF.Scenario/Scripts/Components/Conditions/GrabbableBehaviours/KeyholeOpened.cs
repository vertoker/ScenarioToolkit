using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours.Keys;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Keyhole был открыт", typeof(KeyholeClosed), typeof(Keyhole))]
    public struct KeyholeOpened : IScenarioCondition
    {
        public Keyhole Keyhole;
    }
}