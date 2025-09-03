using System;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Inventory.Scriptables;

namespace VRF.Components.Items.Views
{
    // Бесполезный интерфейс (пока)
    public interface IItem
    {
        public InventoryItemConfig ItemConfig { get; }
        public Grabbable Grabbable { get; }
        
        public void OnSpawn(InventoryItemConfig item);
        public void Disable();
        
        public void OnInitialize();
    }
}