using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает угол у Arrow", typeof(IndicationArrow))]
    public struct SetArrowValue : IScenarioAction
    {
        public IndicationArrow IndicationArrow;
        public float Value;
        public float Time;
    }
}