using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Запускает Stopwatch, нужен для тестирования производительности", typeof(StopStopwatch))]
    public struct StartStopwatch : IScenarioAction
    {
        
    }
}