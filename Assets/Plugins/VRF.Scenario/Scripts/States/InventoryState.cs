using System.Collections.Generic;
using Scenario.Core.Systems.States;
using VRF.Inventory.Scriptables;

namespace VRF.Scenario.States
{
    public class InventoryState : IState
    {
        public HashSet<InventoryItemConfig> Items = new();
        
        public void Clear()
        {
            Items.Clear();
        }
    }
}