using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Запускает Stopwatch, нужен для тестирования производительности", typeof(StopStopwatch))]
    public struct StartStopwatch : IScenarioAction
    {
        
    }
}