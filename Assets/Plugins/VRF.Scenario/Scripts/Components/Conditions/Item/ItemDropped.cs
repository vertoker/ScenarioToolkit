using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Components.Conditions.Item;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Если инвентарный предмет был выброшен", typeof(ItemGrabbed), typeof(InventorySystem))]
    public struct ItemDropped : IScenarioCondition
    {
        public InventoryItemConfig ItemConfig;
    }
}