using UnityEngine;
using VRF.Components.Items.Views;
using VRF.Inventory.Core;
using VRF.Inventory.Equipment;
using VRF.Inventory.Scriptables;

namespace VRF.Test
{
    public class TestEquipmentService
    {
        private readonly HeadEquipmentService headEquipmentService;
        private readonly InventoryItemConfig itemConfig;

        public TestEquipmentService(HeadEquipmentService headEquipmentService, InventoryItemConfig itemConfig)
        {
            this.headEquipmentService = headEquipmentService;
            this.itemConfig = itemConfig;
        }

        public void AddSnapCallback()
        {
            Debug.Log($"{nameof(AddSnapCallback)} {itemConfig.ItemName}");
            headEquipmentService.AddSnapCallback(itemConfig, OnSnap);
        }
        public void AddDetachCallback()
        {
            Debug.Log($"{nameof(AddDetachCallback)} {itemConfig.ItemName}");
            headEquipmentService.AddDetachCallback(itemConfig, OnDetach);
        }
        
        private void OnSnap(IItem item)
        {
            Debug.Log($"{nameof(OnSnap)} {item.ItemConfig.ItemName}");
        }
        private void OnDetach(IItem item)
        {
            Debug.Log($"{nameof(OnDetach)} {item.ItemConfig.ItemName}");
        }
    }
}