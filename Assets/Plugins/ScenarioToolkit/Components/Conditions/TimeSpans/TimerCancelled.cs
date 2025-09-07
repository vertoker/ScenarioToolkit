using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Таймер был отменён до его завершения", typeof(TimerCancelled))]
    public struct TimerCancelled : IScenarioCondition
    {
        [ScenarioMeta("Идентификатор таймера, должен быть уникальным в рамках промежутках проигрывания")]
        public string ID;
    }
}