using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours.Keys;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Keyhole был закрыт", typeof(KeyholeOpened), typeof(Keyhole))]
    public struct KeyholeClosed : IScenarioCondition
    {
        public Keyhole Keyhole;
    }
}