using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Equipment;
using VRF.Inventory.Scriptables;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Добавляет callback на ожидание взятие предмета из системы HeadEquipment", 
        typeof(WaitItemEquip), typeof(HeadEquipmentService))]
    public struct WaitItemUnequip : IScenarioAction, IComponentDefaultValues
    {
        public InventoryItemConfig ItemConfig;
        
        public void SetDefault()
        {
            ItemConfig = null;
        }
    }
}