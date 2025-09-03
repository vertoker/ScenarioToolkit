using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Dialog;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Очищает все варианты ответа в системе диалогов VRF", typeof(AddDialogOption), typeof(DialogService))]
    public struct ClearDialog : IScenarioAction
    {
        
    }
}