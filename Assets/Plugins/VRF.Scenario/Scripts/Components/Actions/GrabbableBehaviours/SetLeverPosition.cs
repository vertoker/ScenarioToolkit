using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает индекс угла у рычага", typeof(GrabbableLever))]
    public struct SetLeverPosition : IScenarioAction
    {
        public GrabbableLever Lever;
        public int Index;
    }
}