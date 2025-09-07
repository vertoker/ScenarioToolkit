using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using VRF.Scenario.Components.Actions;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Таймер начал свой отчёт", typeof(TimerEnded), typeof(StartTimer))]
    public struct TimerStarted : IScenarioCondition
    {
        [ScenarioMeta("Идентификатор таймера, должен быть уникальным в рамках промежутках проигрывания")]
        public string ID;
    }
}