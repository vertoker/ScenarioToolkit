using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Добавляет предмет в текущий инвентарь", typeof(DeleteItem))]
    public struct AddItem : IScenarioAction, IComponentDefaultValues
    {
        public InventoryItemConfig ItemConfig;
        
        public void SetDefault()
        {
            ItemConfig = null;
        }
    }
}