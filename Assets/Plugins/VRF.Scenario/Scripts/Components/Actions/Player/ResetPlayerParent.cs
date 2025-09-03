using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает стандартный parent игроку (скорее всего spawnPoint)", typeof(SetPlayerParent))]
    public struct ResetPlayerParent : IScenarioAction, IComponentDefaultValues
    {
        public bool WorldPositionStays;
        
        public void SetDefault()
        {
            WorldPositionStays = true;
        }
    }
}