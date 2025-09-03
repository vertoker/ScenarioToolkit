using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using VRF.Components;
using VRF.Components.Items.Modules;
using VRF.Identities;
using VRF.Inventory.Scriptables;
using VRF.Networking.Core;
using VRF.Players.Scriptables;
using VRF.Players.Services.Views;
using Zenject;
using Object = UnityEngine.Object;

namespace VRF.Networking.Services
{
    /// <summary>
    /// Регистрация всех сетевых предметов у клиента.
    /// Также кастомно реализует спаун предметов, добавляя их в контейнер активных
    /// </summary>
    public class RegisterNetPrefabsService : IInitializable, IDisposable
    {
        private readonly HashSet<GameObject> registeredPrefabs = new();
        
        [CanBeNull] private readonly IdentitiesConfig list;
        private readonly InventoryItemMainList inventoryItems;
        private readonly ViewsNetSourcesContainer netSources;
        private readonly ProjectViewSpawnerService spawnerService;
        private readonly VRFNetworkManager networkManager;

        public RegisterNetPrefabsService([InjectOptional] IdentitiesConfig list, InventoryItemMainList inventoryItems,
            ViewsNetSourcesContainer netSources, ProjectViewSpawnerService spawnerService, VRFNetworkManager networkManager)
        {
            this.list = list;
            this.inventoryItems = inventoryItems;
            this.netSources = netSources;
            this.spawnerService = spawnerService;
            this.networkManager = networkManager;
        }
        
        public void Initialize()
        {
            // При отключении Mirror автоматически очищает все кастомные ивенты для спауна объектов
            // Поэтому инициализация происходит при каждом подключении к серверу
            networkManager.OnClientStartEvent += RegisterPrefabs;
            networkManager.OnClientStopEvent += UnregisterPrefabs;
        }
        public void Dispose()
        {
            networkManager.OnClientStartEvent -= RegisterPrefabs;
            networkManager.OnClientStopEvent -= UnregisterPrefabs;
        }

        private void RegisterPrefabs()
        {
            if (list) foreach (var config in list.IdentitiesToEnumerable())
                RegisterIdentity(config);
            foreach (var config in inventoryItems.Items)
                RegisterInventoryItem(config);
        }
        private void UnregisterPrefabs()
        {
            netSources.Clear();
            registeredPrefabs.Clear();
        }

        private void RegisterInventoryItem(InventoryItemConfig config)
        {
            var itemView = config.ItemView;
            
            if (itemView.TryGetComponent<ItemObserverModule>(out var module))
                RegisterPrefab(module.NetViewSource);
        }
        private void RegisterIdentity(PlayerIdentityConfig config)
        {
            var appearance = config.Appearance;
            
            if (appearance.IsVR)
            {
                var viewVR = appearance.ConfigurationVR.NetPlayer;
                RegisterPrefab(viewVR);
            }
            if (appearance.IsWASD)
            {
                var viewWASD = appearance.ConfigurationWASD.NetPlayer;
                RegisterPrefab(viewWASD);
            }
        }

        private void RegisterPrefab(BaseView netView)
        {
            var prefab = netView.gameObject;

            if (registeredPrefabs.Add(prefab))
            {
                GameObject Spawn(SpawnMessage msg) => SpawnHandler(msg, netView);
                NetworkClient.RegisterPrefab(prefab, Spawn, UnSpawnHandler);
                netSources.Add(netView);
            }
        }
        private GameObject SpawnHandler(SpawnMessage msg, BaseView source)
        {
            Debug.Log($"Spawn {source.ViewType.Name} {JsonUtility.ToJson(msg)}");
            
            var view = spawnerService.SpawnView(source);
            view.name = $"{source.name} [netId={msg.netId}]";
            
            return view.gameObject;
        }
        private void UnSpawnHandler(GameObject spawned)
        {
            spawnerService.DestroyView(spawned);
            Object.Destroy(spawned);
        }
    }
}