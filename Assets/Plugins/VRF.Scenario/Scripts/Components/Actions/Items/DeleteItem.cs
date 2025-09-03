using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Удаляет предмет из текущего инвентаря", typeof(AddItem))]
    public struct DeleteItem : IScenarioAction, IComponentDefaultValues
    {
        public InventoryItemConfig ItemConfig;
        
        public void SetDefault()
        {
            ItemConfig = null;
        }
    }
}