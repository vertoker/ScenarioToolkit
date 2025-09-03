using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Grabbable вошёл в SnapZone", typeof(SnapZone))]
    public struct GrabbableSnapped : IScenarioCondition
    {
        public SnapZone Zone;
        public Grabbable Grabbable;
    }
}