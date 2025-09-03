using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Сбрасывает graphics volume игроку до null", typeof(SetPlayerVolume))]
    public struct ResetPlayerVolume : IScenarioAction
    {
        
    }
}