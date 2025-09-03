using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("GOTransformStates достиг определённого индекса", typeof(GOTransformStates))]
    public struct TransformStateReached : IScenarioCondition
    {
        public GOTransformStates States;
        public int State;
    }
}