using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Останавливает Stopwatch, нужен для тестирования производительности", typeof(StartStopwatch))]
    public struct StopStopwatch : IScenarioAction
    {
        
    }
}