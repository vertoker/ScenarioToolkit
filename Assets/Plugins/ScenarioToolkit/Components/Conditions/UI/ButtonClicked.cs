using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
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