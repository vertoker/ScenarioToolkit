using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;
using VRF.VRBehaviours.Checking;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Checkable вошёл в TriggerZone", typeof(Checkable), typeof(TriggerZone))]
    public struct GrabbableEnteredTrigger : IScenarioCondition
    {
        public Grabbable Grabbable;
        public TriggerZone TriggerZone;
    }
}