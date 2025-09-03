using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает State у GOTransformStates", typeof(GOTransformStates))]
    public struct SetTransformState : IScenarioAction
    {
        public GOTransformStates States;
        public int State;
    }
}