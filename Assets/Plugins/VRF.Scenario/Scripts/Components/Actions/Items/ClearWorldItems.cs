using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions.Items
{
    [ScenarioMeta("Очищает все заспауненные предметы в мире (и засовывает в пул)", typeof(AddItem))]
    public struct ClearWorldItems : IScenarioAction
    {
        
    }
}