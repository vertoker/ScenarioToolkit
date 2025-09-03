using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Инвентарный предмет находится в TriggerZone", typeof(TriggerSystem))]
    public struct ItemEnteredTrigger : IScenarioCondition
    {
        public InventoryItemConfig ItemType;
        public TriggerZone TriggerZone;
    }
}