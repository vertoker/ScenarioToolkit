using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Была нажата Button", typeof(Button))]
    public struct ButtonClicked : IScenarioCondition
    {
        public Button Button;
    }
}