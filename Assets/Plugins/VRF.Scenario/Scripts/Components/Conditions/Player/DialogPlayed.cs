using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Dialog;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Выбрать вариант в системе диалогов (нужно, чтобы он был там уже)", 
        typeof(AddDialogOption), typeof(DialogService))]
    public struct DialogPlayed : IScenarioCondition
    {
        public DialogLineConfig Line;
    }
}