using Modules.VRF.Scenario.Components.Conditions;
using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.Inventory.Core;
using VRF.Inventory.Equipment;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.States;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class HeadEquipmentSystem : BaseScenarioStateSystem<HeadEquipmentState>
    {
        private readonly HeadEquipmentService headEquipmentService;

        public HeadEquipmentSystem(SignalBus bus, HeadEquipmentService headEquipmentService) : base(bus)
        {
            this.headEquipmentService = headEquipmentService;
            bus.Subscribe<WaitItemEquip>(WaitItemEquip);
            bus.Subscribe<WaitItemUnequip>(WaitItemUnequip);
        }

        protected override void ApplyState(HeadEquipmentState state)
        {
            foreach (var equipItem in state.WaitEquipItems)
            {
                AddSnapCallback(equipItem);
            }
            foreach (var unequipItem in state.WaitUnequipItems)
            {
                AddDetachCallback(unequipItem);
            }
        }

        private void WaitItemEquip(WaitItemEquip component)
        {
            if (AssertLog.NotNull<WaitItemEquip>(component.ItemConfig, nameof(component.ItemConfig))) return;

            State.WaitEquipItems.Add(component.ItemConfig);
            
            AddSnapCallback(component.ItemConfig);
        }
        private void WaitItemUnequip(WaitItemUnequip component)
        {
            if (AssertLog.NotNull<WaitItemUnequip>(component.ItemConfig, nameof(component.ItemConfig))) return;

            State.WaitUnequipItems.Add(component.ItemConfig);
            
            AddDetachCallback(component.ItemConfig);
        }

        private void AddSnapCallback(InventoryItemConfig itemConfig)
        {
            headEquipmentService.AddSnapCallback(itemConfig, 
                _ =>
                {
                    Bus.Fire(new ItemEquipped { ItemConfig = itemConfig });
                    State.WaitEquipItems.Remove(itemConfig);
                });
        }

        private void AddDetachCallback(InventoryItemConfig itemConfig)
        {
            headEquipmentService.AddDetachCallback(itemConfig, 
                _ =>
                {
                    Bus.Fire(new ItemUnequipped { ItemConfig = itemConfig });
                    State.WaitUnequipItems.Remove(itemConfig);
                });
        }
    }
}