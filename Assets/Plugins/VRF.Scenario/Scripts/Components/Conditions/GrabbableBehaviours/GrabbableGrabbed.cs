using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Grabbable был взят", typeof(Grabbable))]
    public struct GrabbableGrabbed : IScenarioCondition
    {
        public Grabbable Value;
    }
}