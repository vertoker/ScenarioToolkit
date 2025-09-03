using System.Collections.Generic;
using Scenario.Core.Systems.States;
using VRF.Inventory.Scriptables;

namespace VRF.Scenario.States
{
    public class HeadEquipmentState : IState
    {
        public HashSet<InventoryItemConfig> WaitEquipItems = new();
        public HashSet<InventoryItemConfig> WaitUnequipItems = new();
        
        public void Clear()
        {
            WaitEquipItems.Clear();
            WaitUnequipItems.Clear();
        }
    }
}