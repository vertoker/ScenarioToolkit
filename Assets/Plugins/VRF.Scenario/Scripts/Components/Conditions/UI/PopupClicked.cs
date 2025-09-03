using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.UI.ScenarioGame;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Была нажата Button на PopupScreen", typeof(PopupScreen))]
    public struct PopupClicked : IScenarioCondition
    {
        public PopupScreen Popup;
    }
}