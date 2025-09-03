using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions

{    [ScenarioMeta("Предмет на сцене изъят из SnapZone",  typeof(SnapZone))]
    public struct GrabbableUnsnapped : IScenarioCondition
    {
        public SnapZone Zone;
        public Grabbable Grabbable;
    }
}