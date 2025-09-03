using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Core;
using VRF.Inventory.Equipment;
using VRF.Inventory.Scriptables;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Добавляет callback на ожидание добавление предмета в систему HeadEquipment", 
        typeof(WaitItemUnequip), typeof(HeadEquipmentService))]
    public struct WaitItemEquip : IScenarioAction, IComponentDefaultValues
    {
        public InventoryItemConfig ItemConfig;
        
        public void SetDefault()
        {
            ItemConfig = null;
        }
    }
}