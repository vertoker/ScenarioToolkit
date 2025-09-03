using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRF.Inventory.Scriptables;
using Zenject;

namespace VRF.Inventory.Core
{
    /// <summary>
    /// Контейнер доступных предметов в инвентаре
    /// </summary>
    public class ActiveItemsContainer // : IDisposable
    {
        private readonly List<InventoryItemConfig> items = new();

        // Для изменения списка активных предметов существуют отдельные методы
        public IReadOnlyList<InventoryItemConfig> Items => items;

        public event Action<InventoryItemConfig> OnAddItemInInventory;
        public event Action<InventoryItemConfig> OnRemoveItemFromInventory;

        public ActiveItemsContainer([InjectOptional] InventoryItemConfigList config)
        {
            if (config)
                SetItems(config.Items);
        }

        public void SetItems(IEnumerable<InventoryItemConfig> itemsEnumerable)
        {
            RemoveCurrentItems();
            AddItems(itemsEnumerable);
        }

        public void RemoveCurrentItems()
        {
            if (items.Count == 0) return;
            var tempItems = items.ToArray();
            RemoveItems(tempItems);
        }

        public bool AddItems(IEnumerable<InventoryItemConfig> itemsEnumerable) => itemsEnumerable.All(AddItem);
        public bool RemoveItems(IEnumerable<InventoryItemConfig> itemsEnumerable) => itemsEnumerable.All(RemoveItem);

        // Очень важно регистировать ошибки по изменению объектов в числе активных объектов инвентаря
        // так как все объекты инвентаря уникальные

        // По сути этот класс является некой оболоской над List с внедрением приемуществ HashSet

        public bool AddItem(InventoryItemConfig item)
        {
            if (items.Contains(item))
            {
                Debug.LogWarning($"Can't add item <b>{item.ItemName}</b>, already in inventory");
                return false;
            }

            items.Add(item);
            OnAddItemInInventory?.Invoke(item);
            return true;
        }

        public bool RemoveItem(InventoryItemConfig item)
        {
            if (!items.Remove(item))
            {
                Debug.LogWarning($"Can't remove item <b>{item.ItemName}</b>, can't find in inventory");
                return false;
            }

            OnRemoveItemFromInventory?.Invoke(item);
            return true;
        }
    }
}