using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace Modules.VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Инвентарный предмет был снят с груди", typeof(ItemEquipped), typeof(HeadEquipmentSystem))]
    public struct ItemUnequipped : IScenarioCondition
    {
        public InventoryItemConfig ItemConfig;
    }
}