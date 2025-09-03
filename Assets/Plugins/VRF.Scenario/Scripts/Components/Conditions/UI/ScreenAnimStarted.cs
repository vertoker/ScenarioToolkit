using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using SimpleUI.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("ScreenBase начал операцию открытия/закрытия", typeof(ScreenBase), typeof(ScreenAnimEnded))]
    public struct ScreenAnimStarted : IScenarioCondition
    {
        public ScreenBase Screen;
    }
}