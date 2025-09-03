using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using SimpleUI.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("ScreenBase закончил операцию открытия/закрытия", typeof(ScreenBase), typeof(ScreenAnimStarted))]
    public struct ScreenAnimEnded : IScenarioCondition
    {
        public ScreenBase Screen;
    }
}