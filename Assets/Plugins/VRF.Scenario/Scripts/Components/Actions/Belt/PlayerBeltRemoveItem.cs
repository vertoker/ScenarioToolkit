using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions.Items
{
    [ScenarioMeta("Добавляет предмет ИЗ пояса В инвентарь", typeof(PlayerBeltAddItem))]
    public struct PlayerBeltRemoveItem : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор слота на поясе игрока")]
        public uint SlotID;
        public InventoryItemConfig ItemConfig;
        
        public void SetDefault()
        {
            SlotID = 0;
            ItemConfig = null;
        }
    }
}