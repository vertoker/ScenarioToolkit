using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;
using VRF.Inventory.Scriptables;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает новый parent игроку", typeof(ResetPlayerParent))]
    public struct SetPlayerParent : IScenarioAction, IComponentDefaultValues
    {
        public Transform Parent;
        public bool WorldPositionStays;
        
        public void SetDefault()
        {
            Parent = null;
            WorldPositionStays = true;
        }
    }
}