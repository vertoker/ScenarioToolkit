using Scenario.Core.Model.Interfaces;
using VRF.VRBehaviours;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    public struct ArrowValueReached : IScenarioCondition
    {
        public IndicationArrow IndicationArrow;
    }
}