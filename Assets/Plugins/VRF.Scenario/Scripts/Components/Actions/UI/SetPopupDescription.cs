using Scenario.Core.Model.Interfaces;
using UnityEngine.Serialization;
using VRF.Scenario.UI.ScenarioGame;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    public struct SetPopupDescription : IScenarioAction
    {
        public PopupScreen Popup;
        public string Description;
    }
}