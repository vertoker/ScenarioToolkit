using System;
using System.Collections.Generic;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Items.Views;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Scriptables;
using VRF.Players.Core;
using Zenject;

namespace VRF.Inventory.Equipment
{
    // Работает только для VR
    
    /// <summary> Позволяет использовать игрока как хранилище предметов, доставать и вытаскивать их.
    /// Всё взаимодействие происходит через SnapZone, ничего не принимает без запросов </summary>
    public class HeadEquipmentService : IInitializable, IDisposable
    {
        private readonly PlayersContainer playersContainer;
        private readonly bool active;
        private SnapZone snapZone;
        
        private readonly Dictionary<InventoryItemConfig, Grabbable> items = new();
        
        private readonly Dictionary<InventoryItemConfig, Action<IItem>> onSnapEvents = new();
        private readonly Dictionary<InventoryItemConfig, Action<IItem>> onDetachEvents = new();

        
        public HeadEquipmentService(PlayersContainer playersContainer)
        {
            this.playersContainer = playersContainer;
            active = playersContainer != null;
            if (!active) return;
            
            UpdateSnapZone();
            this.playersContainer.PlayerChanged += PlayerChanged;
        }

        public void Initialize()
        {
            if (!active || !snapZone) return;
            Subscribe();
        }
        public void Dispose()
        {
            if (!active || !snapZone) return;
            Unsubscribe();
        }

        private void PlayerChanged()
        {
            Grabbable heldItem = null;
            if (snapZone)
            {
                heldItem = snapZone.HeldItem;
                snapZone.ReleaseAll();

                Unsubscribe();
            }

            UpdateSnapZone();

            if (snapZone)
            {
                if (heldItem)
                {
                    snapZone.GrabGrabbable(heldItem);
                }

                Subscribe();
            }
        }

        private void UpdateSnapZone()
        {
            var view = playersContainer.CurrentValue.View;
            if (view is PlayerVRView vrView)
                snapZone = vrView.HeadEquipmentZone;
        }

        private void Subscribe()
        {
            snapZone.ExcludeConditions.Add(ExcludeCondition);
            snapZone.OnSnapEvent.AddListener(OnSnap);
            snapZone.OnDetachEvent.AddListener(OnDetach);
        }
        private void Unsubscribe()
        {
            snapZone.OnSnapEvent.RemoveListener(OnSnap);
            snapZone.OnDetachEvent.RemoveListener(OnDetach);
            snapZone.ExcludeConditions.Remove(ExcludeCondition);
        }

        public bool Contains(InventoryItemConfig itemConfig)
        {
            return items.ContainsKey(itemConfig);
        }
        
        private bool ExcludeCondition(Grabbable grabbable)
        {
            var item = grabbable.GetComponent<IItem>();
            if (item == null) return true;
            if (!item.ItemConfig) return true;

            var inStack = items.ContainsKey(item.ItemConfig);
            return inStack 
                ? !onDetachEvents.ContainsKey(item.ItemConfig)
                : !onSnapEvents.ContainsKey(item.ItemConfig);
        }

        private void OnSnap(Grabbable grabbable)
        {
            var item = grabbable.GetComponent<IItem>();
            if (item == null) return;
            
            if (!onSnapEvents.Remove(item.ItemConfig, out var action)) return;
            
            snapZone.ReleaseAll();
            items.TryAdd(item.ItemConfig, grabbable);
            grabbable.gameObject.SetActive(false);
            action?.Invoke(item);
        }
        private void OnDetach(Grabbable grabbable)
        {
            var item = grabbable.GetComponent<IItem>();
            if (item == null) return;

            if (!onDetachEvents.Remove(item.ItemConfig, out var action)) return;
            
            snapZone.ReleaseAll();
            grabbable.gameObject.SetActive(true);
            items.Remove(item.ItemConfig);
            action?.Invoke(item);
        }

        /// <summary> Если предмет положили в SnapZone </summary>
        public void AddSnapCallback(InventoryItemConfig item, Action<IItem> action)
        {
            TryAddCallback(onSnapEvents, item, action);
        }
        /// <summary> Если предмет взяли из SnapZone </summary>
        public void AddDetachCallback(InventoryItemConfig item, Action<IItem> action)
        {
            TryAddCallback(onDetachEvents, item, action);
            //TODO: Если рука находится в снэп зоне, зажата и пуста, предмет обнуяется в снэп зоне (update в BNG)
            if (items.TryGetValue(item, out var grabbable))
                snapZone.GrabGrabbable(grabbable);
            else 
                Debug.LogWarning($"Can't find <b>{item.ItemName}</b> in <b>{nameof(HeadEquipmentService)}</b>");
        }
        
        private void TryAddCallback(Dictionary<InventoryItemConfig, Action<IItem>> events, 
            InventoryItemConfig item, Action<IItem> action)
        {
            if (!events.TryAdd(item, action))
                Debug.LogError($"Can't add duplicate item <b>{item.ItemName}</b>");
        }
    }
}