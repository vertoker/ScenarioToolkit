using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Инвентарный предмет вышел из SnapZone", typeof(ItemSnapped), typeof(SnappingSystem))]
    public struct ItemUnsnapped : IScenarioCondition
    {
        public SnapZone Zone;
        public InventoryItemConfig InventoryItemType;
    }
}