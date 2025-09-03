using Scenario.Base.Components.Actions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Conditions
{
    [ScenarioMeta("Окончание простого таймера", typeof(StartDelayTrigger))]
    public struct DelayTriggerEnded : IScenarioCondition
    {
        [ScenarioMeta("Идентификатор таймера, должен быть уникальным в рамках промежутках проигрывания")]
        public string Name;
    }
}