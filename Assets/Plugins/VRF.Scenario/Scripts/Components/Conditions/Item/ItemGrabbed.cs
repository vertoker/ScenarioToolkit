using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions.Item
{
    [ScenarioMeta("Если инвентарный предмет был взят", typeof(ItemDropped), typeof(InventorySystem))]
    public struct ItemGrabbed : IScenarioCondition
    {
        public InventoryItemConfig ItemConfig;
    }
}