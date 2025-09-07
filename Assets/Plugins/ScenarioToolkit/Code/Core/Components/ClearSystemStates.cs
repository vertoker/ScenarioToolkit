using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Очищает состояние всех System States " +
                  "(сильно провисает в моменте) (работает по сети)")]
    public struct ClearSystemStates : IScenarioAction
    {
        // TODO может работать некорректно, надо дописать
    }
}