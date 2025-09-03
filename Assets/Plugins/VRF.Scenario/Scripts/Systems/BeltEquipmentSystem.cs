using System;
using JetBrains.Annotations;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.Components.Items.Views;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Core;
using VRF.Inventory.Equipment;
using VRF.Players.Core;
using VRF.Scenario.Components.Actions.Items;
using VRF.Scenario.States;
using Zenject;
using Object = UnityEngine.Object;

namespace VRF.Scenario.Systems
{
    public enum BeltRemoveType
    {
        Destroy = 0,
        Drop = 1,
    }
    
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class BeltEquipmentSystem : BaseScenarioStateSystem<BeltEquipmentState>
    {
        private readonly PlayersContainer playersContainer;
        private readonly InventoryController inventoryController;
        private BeltEquipmentManager equipmentManager;

        public BeltEquipmentSystem(SignalBus bus, PlayersContainer playersContainer, 
            InventoryController inventoryController) : base(bus)
        {
            this.playersContainer = playersContainer;
            this.inventoryController = inventoryController;
            playersContainer.PlayerChanged += PlayerChanged;
            
            bus.Subscribe<PlayerBeltSetActive>(PlayerBeltSetActive);
            bus.Subscribe<PlayerBeltInsertSlot>(PlayerBeltInsertSlot);
            bus.Subscribe<PlayerBeltDeleteSlot>(PlayerBeltDeleteSlot);
            bus.Subscribe<PlayerBeltDropAll>(PlayerBeltDropAll);
            
            bus.Subscribe<PlayerBeltAddGrabbable>(PlayerBeltAddGrabbable);
            bus.Subscribe<PlayerBeltAddItem>(PlayerBeltAddItem);
            bus.Subscribe<PlayerBeltRemoveGrabbable>(PlayerBeltRemoveGrabbable);
            bus.Subscribe<PlayerBeltRemoveItem>(PlayerBeltRemoveItem);
        }
        
        protected override void ApplyState(BeltEquipmentState state)
        {
            
        }
        private void PlayerChanged()
        {
            var baseView = playersContainer.CurrentValue.View;
            if (baseView is not PlayerVRView vrView) return;
            equipmentManager = vrView.EquipmentManager;
        }

        private void PlayerBeltSetActive(PlayerBeltSetActive component)
        {
            if (AssertLog.NotNull<PlayerBeltSetActive>(equipmentManager, nameof(equipmentManager))) return;
            equipmentManager.SetActive(component.Active);
        }
        private void PlayerBeltInsertSlot(PlayerBeltInsertSlot component)
        {
            if (AssertLog.NotNull<PlayerBeltInsertSlot>(equipmentManager, nameof(equipmentManager))) return;
            equipmentManager.InsertSlot(component.SlotID, component.Height, component.Distance, component.Angle);
        }
        private void PlayerBeltDeleteSlot(PlayerBeltDeleteSlot component)
        {
            if (AssertLog.NotNull<PlayerBeltDeleteSlot>(equipmentManager, nameof(equipmentManager))) return;
            equipmentManager.DeleteSlot(component.SlotID);
        }
        private void PlayerBeltDropAll(PlayerBeltDropAll component)
        {
            if (AssertLog.NotNull<PlayerBeltDropAll>(equipmentManager, nameof(equipmentManager))) return;
            equipmentManager.DropAll();
        }

        private void PlayerBeltAddGrabbable(PlayerBeltAddGrabbable component)
        {
            if (AssertLog.NotNull<PlayerBeltAddGrabbable>(equipmentManager, nameof(equipmentManager))) return;
            if (AssertLog.NotNull<PlayerBeltAddGrabbable>(component.Grabbable, nameof(component.Grabbable))) return;
            var slot = equipmentManager.GetSlotOrDefault(component.SlotID);
            if (AssertLog.NotNull<PlayerBeltAddGrabbable>(slot, nameof(slot))) return;

            if (slot.HasItem())
                PlayerBeltRemoveGrabbable(component.CastToRemove());
            var grabbable = Object.Instantiate(component.Grabbable);
            slot.AddItem(grabbable);
        }
        private void PlayerBeltAddItem(PlayerBeltAddItem component)
        {
            if (AssertLog.NotNull<PlayerBeltAddItem>(equipmentManager, nameof(equipmentManager))) return;
            if (AssertLog.NotNull<PlayerBeltAddItem>(component.ItemConfig, nameof(component.ItemConfig))) return;
            var slot = equipmentManager.GetSlotOrDefault(component.SlotID);
            if (AssertLog.NotNull<PlayerBeltAddItem>(slot, nameof(slot))) return;
            
            if (slot.HasItem())
                PlayerBeltRemoveItem(component.CastToRemove());
            var itemView = inventoryController.TakeItem(component.ItemConfig);
            slot.AddItem(itemView);
        }
        private void PlayerBeltRemoveGrabbable(PlayerBeltRemoveGrabbable component)
        {
            if (AssertLog.NotNull<PlayerBeltRemoveGrabbable>(equipmentManager, nameof(equipmentManager))) return;
            var slot = equipmentManager.GetSlotOrDefault(component.SlotID);
            if (AssertLog.NotNull<PlayerBeltRemoveGrabbable>(slot, nameof(slot))) return;

            if (!slot.HasItem()) return;
            switch (component.RemoveType)
            {
                case BeltRemoveType.Destroy:
                    slot.DestroyItem();
                    break;
                case BeltRemoveType.Drop:
                    slot.DropItem();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void PlayerBeltRemoveItem(PlayerBeltRemoveItem component)
        {
            if (AssertLog.NotNull<PlayerBeltRemoveItem>(equipmentManager, nameof(equipmentManager))) return;
            if (AssertLog.NotNull<PlayerBeltRemoveItem>(component.ItemConfig, nameof(component.ItemConfig))) return;
            var slot = equipmentManager.GetSlotOrDefault(component.SlotID);
            if (AssertLog.NotNull<PlayerBeltRemoveItem>(slot, nameof(slot))) return;
            
            var grabbable = slot.GetItem();
            slot.DropItem();
            if (grabbable && grabbable.TryGetComponent(out ItemView itemView))
                inventoryController.ReturnItem(itemView);
        }
    }
}