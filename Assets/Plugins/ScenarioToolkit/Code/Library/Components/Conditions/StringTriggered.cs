using Scenario.Base.Components.Actions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Conditions
{
    [ScenarioMeta("Триггер для получения ивентов", typeof(TriggerString))]
    public struct StringTriggered : IScenarioCondition, IComponentDefaultValues
    {
        [ScenarioMeta("Не работает если пустой или состоит только из пробелов")]
        public string String;
        
        public void SetDefault()
        {
            String = string.Empty;
        }
    }
}