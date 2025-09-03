using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.VRBehaviours;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("GrabbableLever достиг определённого индекса", typeof(GrabbableLever))]
    public struct LeverSnapDegreeReached : IScenarioCondition
    {
        public GrabbableLever GrabbableLever;
        public int SnapDegreesIndex;
    }
}