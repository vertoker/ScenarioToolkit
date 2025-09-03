using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions.Items
{
    [ScenarioMeta("Добавляет предмет ИЗ инвентаря НА пояс", typeof(PlayerBeltRemoveItem))]
    public struct PlayerBeltAddItem : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор слота на поясе игрока")]
        public uint SlotID;
        [ScenarioMeta("Если свободного предмета в инвентаре не будет, то ничего не произойдёт")]
        public InventoryItemConfig ItemConfig;
        
        public void SetDefault()
        {
            SlotID = 0;
            ItemConfig = null;
        }

        public PlayerBeltRemoveItem CastToRemove()
        {
            return new PlayerBeltRemoveItem
            {
                SlotID = SlotID,
                ItemConfig = ItemConfig,
            };
        }
    }
}