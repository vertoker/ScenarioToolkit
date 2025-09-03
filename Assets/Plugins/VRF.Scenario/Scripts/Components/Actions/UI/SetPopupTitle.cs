using Scenario.Core.Model.Interfaces;
using VRF.Scenario.UI.ScenarioGame;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    public struct SetPopupTitle : IScenarioAction
    {
        public PopupScreen Popup;
        public string Title;
    }
}