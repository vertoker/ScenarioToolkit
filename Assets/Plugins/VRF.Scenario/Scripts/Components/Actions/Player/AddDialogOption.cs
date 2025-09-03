using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Dialog;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Добавляет вариант ответа в системе диалогов VRF", typeof(ClearDialog), typeof(DialogService))]
    public struct AddDialogOption : IScenarioAction
    {
        public DialogLineConfig Line;
    }
}