using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using VRF.Components.Items.Views;
using VRF.Inventory.Scriptables;
using VRF.Players.Services.Views;
using Zenject;

namespace VRF.Inventory.Core
{
    /// <summary>
    /// Кэширует создание объектов инвентаря и предоставляет интерфейс для их get - set
    /// </summary>
    public class ItemContainerPool : IDisposable
    {
        /// <summary>
        /// Внутренняя структура для удобства и скорости получения кэшированных объектов из пула
        /// </summary>
        private struct Entry
        {
            public readonly Queue<ItemView> CachedViews;
            public readonly HashSet<ItemView> ActiveViews;

            public Entry(InventoryItemConfig item, ItemContainerPool pool)
            {
                var sizeView = item.OverridePoolSize ? item.PoolSize :
                    item.LimitInstances ? item.InstancesCount : 4;

                CachedViews = new Queue<ItemView>(sizeView);
                ActiveViews = new HashSet<ItemView>(item.InstancesCount);

                for (var i = 0; i < sizeView; i++)
                    CachedViews.Enqueue(CreateView());
                return;
                
                ItemView CreateView() => pool.CreateItem(item);
            }
        }

        private readonly Dictionary<InventoryItemConfig, Entry> pool;
        private readonly Queue<ItemViewUI> cachedViewsUI;
        
        private readonly InventoryProjectConfig projectConfig;
        private readonly Transform parentPool;
        private readonly SceneViewSpawnerService spawner;

        // TODO сделать projectConfig опциональным
        public ItemContainerPool(InventoryProjectConfig projectConfig, Transform parentPool,
            SceneViewSpawnerService spawner, [InjectOptional, CanBeNull] InventoryItemMainList itemsConfig)
        {
            this.projectConfig = projectConfig;
            this.parentPool = parentPool;
            this.spawner = spawner;
            
            this.spawner.RegisterParent<ItemView>(this.parentPool);
            this.spawner.RegisterParent<ItemViewUI>(this.parentPool);
            
            if (itemsConfig != null)
            {
                pool = new Dictionary<InventoryItemConfig, Entry>(itemsConfig.Items.Count);
                cachedViewsUI = new Queue<ItemViewUI>(itemsConfig.Items.Count);
                
                foreach (var config in itemsConfig.Items)
                {
                    var itemBind = new Entry(config, this);
                    pool.Add(config, itemBind);
                    cachedViewsUI.Enqueue(CreateItemUI());
                }
            }
            else
            {
                pool = new Dictionary<InventoryItemConfig, Entry>();
                cachedViewsUI = new Queue<ItemViewUI>();
            }
        }
        public void Dispose()
        {
            spawner.UnregisterParent<ItemView>();
            spawner.UnregisterParent<ItemViewUI>();
        }

        /// <summary> Выдаёт (и активирует) ItemView в пул активных объектов </summary>
        public ItemView DequeueItem(InventoryItemConfig item, bool instantEnable = true)
        {
            if (!pool.TryGetValue(item, out var bind))
            {
                bind = new Entry(item, this);
                pool.Add(item, bind);
            }
            
            var queue = bind.CachedViews;
            var view = queue.Count == 0 ? CreateItem(item) : queue.Dequeue();
            if (instantEnable) view.OnSpawn(item);
            return view;
        }
        /// <summary> Выдаёт (и активирует) ItemViewUI в пул активных объектов </summary>
        public ItemViewUI DequeueItemUI(InventoryItemConfig item, bool instantEnable = true)
        {
            var view = cachedViewsUI.Count == 0 ? CreateItemUI() : cachedViewsUI.Dequeue();
            if (instantEnable) view.OnSpawn(item);
            return view;
        }
        
        /// <summary> Засовывает (и деактивирует) ItemView в пул неактивных объектов </summary>
        public void EnqueueItem(ItemView view, bool instantDisable = true)
        {
            if (!pool.TryGetValue(view.ItemConfig, out var bind))
            {
                bind = new Entry(view.ItemConfig, this);
                pool.Add(view.ItemConfig, bind);
            }
            
            if (instantDisable) view.Disable();
            view.transform.SetParent(parentPool);
            
            bind.CachedViews.Enqueue(view);
            bind.ActiveViews.Remove(view);
        }
        /// <summary> Засовывает (и деактивирует) ItemViewUI в пул неактивных объектов </summary>
        public void EnqueueItemUI(ItemViewUI view, bool instantDisable = true)
        {
            if (instantDisable) view.Disable();
            view.transform.SetParent(parentPool);
            cachedViewsUI.Enqueue(view);
        }
        
        /// <summary> Все активные предметы засовывает в пул неактивных предметов (и деактивирует) </summary>
        public void EnqueueActiveItems(bool instantDisable = true)
        {
            var activeItems = pool.Values.SelectMany(e => e.ActiveViews).ToArray();

            foreach (var activeItem in activeItems)
                EnqueueItem(activeItem, instantDisable);
        }

        /// <summary> Регистрирует последний активный предмет (нужен для работы limitInstances) </summary>
        public void RegisterActiveView(ItemView view, bool instantDisable = true)
        {
            var item = view.ItemConfig;
            if (!item.LimitInstances) 
                return;

            var hashset = pool[view.ItemConfig].ActiveViews;
            
            if (item.InstancesCount == hashset.Count)
            {
                using var enumerator = hashset.GetEnumerator();
                enumerator.MoveNext();
                var lastActiveView = enumerator.Current;
                EnqueueItem(lastActiveView, instantDisable);
            }
            
            hashset.Add(view);
        }
        
        // Если в кэше кончились объекты, то создаём новые
        private ItemView CreateItem(InventoryItemConfig item)
        {
            var itemView = spawner.SpawnView(item.ItemView);
            itemView.gameObject.SetActive(false);
            return itemView;
        }
        private ItemViewUI CreateItemUI()
        {
            var itemViewUI = spawner.SpawnView(projectConfig.ViewUI);
            itemViewUI.gameObject.SetActive(false);
            return itemViewUI;
        }
    }
}