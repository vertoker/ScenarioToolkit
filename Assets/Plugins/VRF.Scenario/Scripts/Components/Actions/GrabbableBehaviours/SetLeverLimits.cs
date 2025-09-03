using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает лимиты поворота рычага", typeof(GrabbableLever))]
    public struct SetLeverLimits : IScenarioAction
    {
        public GrabbableLever Lever;
        public float MinAngle;
        public float MaxAngle;
    }
}