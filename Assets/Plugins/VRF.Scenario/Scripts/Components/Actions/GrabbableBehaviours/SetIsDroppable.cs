using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает CanBeDropped у Grabbable", typeof(Grabbable))]
    public struct SetIsDroppable : IScenarioAction
    {
        public Grabbable GrabbableObject;
        public bool IsDroppable;
    }
}