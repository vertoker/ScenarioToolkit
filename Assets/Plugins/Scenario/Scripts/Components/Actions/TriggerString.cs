using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Триггер для посылания ивентов. Нужен для посылания сигналов между сценариями", typeof(StringTriggered))]
    public struct TriggerString : IScenarioAction, IComponentDefaultValues
    { 
        [ScenarioMeta("Не работает если пустой или состоит только из пробелов")]
        public string String;
        
        public void SetDefault()
        {
            String = string.Empty;
        }
    }
}