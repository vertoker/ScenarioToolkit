using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace VRF.Inventory.Scriptables
{
    /// <summary>
    /// Список предметов, нужен для разных вещей, но
    /// имеет одно прямое использование: фильтрация активных предметов на конкретной сцене
    /// </summary>
    [CreateAssetMenu (fileName  = nameof(InventoryItemConfigList), menuName = "VRF/Inventory/" + nameof(InventoryItemConfigList))]
    public class InventoryItemConfigList : ScriptableObject
    {
        [SerializeField] private List<InventoryItemConfig> inventoryItems = new();
        [SerializeField, HideInInspector] private List<InventoryItemConfig> activeInventoryItems = new();
        
        public IReadOnlyList<InventoryItemConfig> Items => activeInventoryItems;
        
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private List<InventoryItemConfig> cachedItems = new();

        private void OnValidate()
        {
            // Необходима фильтрация по активным объектам
            UpdateActiveItems();
        }

        protected void UpdateItems(IEnumerable<InventoryItemConfig> configs)
        {
            inventoryItems.Clear();
            inventoryItems.AddRange(configs);
            UpdateActiveItems();
        }

        public void UpdateActiveItems()
        {
            foreach (var item in cachedItems.Where(item => item != null))
                item.RemoveUsage(this);

            cachedItems = inventoryItems.ToList();
            activeInventoryItems = inventoryItems.Where(i => i != null && i.Active).ToList();
            
            foreach (var item in inventoryItems.Where(item => item != null))
                item.AddUsage(this);
        }
#endif
    }
}