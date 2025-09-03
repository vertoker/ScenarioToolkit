using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Запускает простой таймер (полностью локальный)", typeof(DelayTriggerEnded))]
    public struct StartDelayTrigger : IScenarioAction, IScenarioOnlyHost
    {
        [ScenarioMeta("Идентификатор таймера, должен быть уникальным в рамках промежутках проигрывания")]
        public string Name;
        [ScenarioMeta("Должен быть больше 0")]
        public float Seconds;
    }
}