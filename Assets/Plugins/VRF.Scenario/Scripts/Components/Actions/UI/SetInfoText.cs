using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает текст подсказки на руке (шорткат)", typeof(SetInfo))]
    public struct SetInfoText : IScenarioAction
    {
        public string Text;
    }
}