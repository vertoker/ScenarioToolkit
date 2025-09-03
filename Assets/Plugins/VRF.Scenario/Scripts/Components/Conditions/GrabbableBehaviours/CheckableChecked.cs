using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours.Checking;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Checkable был проверен", typeof(Checkable))]
    public struct CheckableChecked : IScenarioCondition
    {
        public Checkable Checkable;
    }
}