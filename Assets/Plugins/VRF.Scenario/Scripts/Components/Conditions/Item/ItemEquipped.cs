using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace Modules.VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Инвентарный предмет был одет на грудь", typeof(ItemUnequipped), typeof(HeadEquipmentSystem))]
    public struct ItemEquipped : IScenarioCondition
    {
        public InventoryItemConfig ItemConfig;
    }
}