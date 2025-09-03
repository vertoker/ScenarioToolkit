using System;
using System.Collections.Generic;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Items.Views;
using VRF.Inventory.Scriptables;
using VRF.Utils;

namespace VRF.Inventory.Core
{
    /// <summary>
    /// Контроллер, отвечающий за взятие предмета рукой и последующей обработкой этого события
    /// </summary>
    public class InventoryController
    {
        private readonly ItemContainerPool itemContainerPool;
        private readonly Dictionary<ItemView, ItemDropSubscription> handDroppedItems = new();
        
        public event Action<IItem> OnGrabItem;
        public event Action<IItem> OnDropItem;

        public InventoryController(ItemContainerPool itemContainerPool)
        {
            this.itemContainerPool = itemContainerPool;
        }
        
        public void SpawnItemInHand(InventoryItemConfig itemConfig, Transform spawnPoint, Grabber grabber)
        {
            // Вместо спауна объекта, достаёт его из пула предметов
            var spawnedItem = itemContainerPool.DequeueItem(itemConfig, false);
            
            // постановка объекта
            var spawnedItemTransform = spawnedItem.transform;
            spawnedItemTransform.SetParent(spawnPoint);
            spawnedItemTransform.position = spawnPoint.position;
            spawnedItemTransform.rotation = spawnPoint.rotation;
            
            spawnedItem.OnSpawn(itemConfig);
            var spawnedItemGrabbable = spawnedItem.Grabbable;
            grabber.GrabGrabbable(spawnedItemGrabbable);
            
            itemContainerPool.RegisterActiveView(spawnedItem);
            spawnedItem.OnInitialize();

            var subscription = new ItemDropSubscription(spawnedItem, OnItemDropped);
            subscription.Initialize();
            handDroppedItems.Add(spawnedItem, subscription);
            
            OnGrabItem?.Invoke(spawnedItem);
            
            Debug.Log($"{itemConfig.ItemName}");
        }

        public ItemView TakeItem(InventoryItemConfig itemConfig, bool instantEnable = true)
        {
            var spawnedItem = itemContainerPool.DequeueItem(itemConfig, instantEnable);
            spawnedItem.OnSpawn(itemConfig);
            
            itemContainerPool.RegisterActiveView(spawnedItem);
            spawnedItem.OnInitialize();

            return spawnedItem;
        }
        public void ReturnItem(ItemView itemView, bool instantDisable = true)
        {
            itemContainerPool.EnqueueItem(itemView, instantDisable);
        }

        private void OnItemDropped(ItemView itemView)
        {
            var subscription = handDroppedItems[itemView];
            subscription.Dispose();
            handDroppedItems.Remove(itemView);
            
            OnDropItem?.Invoke(itemView);
            
            if (itemView.ItemConfig.DisableOnRelease)
                itemContainerPool.EnqueueItem(itemView);
        }
        
        private class ItemDropSubscription : Subscription<ItemView>
        {
            public ItemDropSubscription(ItemView bind, Action<ItemView> callback) : base(bind, callback)
            {
            }

            public override void Initialize()
            {
                bind.Grabbable.OnDropped += InvokeCallback;
            }

            public override void Dispose()
            {
                bind.Grabbable.OnDropped -= InvokeCallback;
            }
        }
    }
}