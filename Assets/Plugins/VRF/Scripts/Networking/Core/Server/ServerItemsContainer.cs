using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using VRF.Networking.Messages;
using VRF.Networking.Services;
using VRF.Players.Scriptables;
using VRF.Players.Services.Views;
using VRF.Utilities.Extensions;
using Zenject;

namespace VRF.Networking.Core.Server
{
    /// <summary>
    /// Контейнер всех созданных сетевых предметов,
    /// создаёт и удаляет их для всех игроков
    /// </summary>
    public class ServerItemsContainer : IInitializable, IDisposable
    {
        // TODO операция ChangeAuthority пока что не поддерживается
        
        // Для идентификации используется RuntimeHashCode так как могут существовать одинаковые предметы
        private Dictionary<NetworkConnectionToClient, List<int>> playerItems;
        private Dictionary<int, NetworkIdentity> spawnedItems;

        private readonly VRFNetworkManager networkManager;
        private readonly ProjectViewSpawnerService spawner;
        private readonly ViewsNetSourcesContainer sourcesContainer;

        public ServerItemsContainer(VRFNetworkManager networkManager, ProjectViewSpawnerService spawner, 
            ViewsNetSourcesContainer sourcesContainer)
        {
            this.networkManager = networkManager;
            this.spawner = spawner;
            this.sourcesContainer = sourcesContainer;
        }

        public void Initialize()
        {
            networkManager.OnServerStartEvent += OnServerStart;
            networkManager.OnServerStopEvent += OnServerStop;
            networkManager.OnServerDisconnectEvent += OnServerDisconnect;
            
            networkManager.RegisterServerMessage<NetItemSpawn_RequestMessage>(SpawnItem);
            networkManager.RegisterServerMessage<NetItemDestroy_RequestMessage>(DestroyItem);
            
            networkManager.RegisterServerMessage<NetItemEnable_RequestMessage>(ResendToAll);
            networkManager.RegisterServerMessage<NetItemDisable_RequestMessage>(ResendToAll);
            networkManager.RegisterServerMessage<NetItemToggleBehaviour_Message>(ResendToAll);
            networkManager.RegisterServerMessage<NetItemToggleBehaviours_Message>(ResendToAll);
        }
        public void Dispose()
        {
            networkManager.OnServerStartEvent -= OnServerStart;
            networkManager.OnServerStopEvent -= OnServerStop;
            networkManager.OnServerDisconnectEvent -= OnServerDisconnect;
            
            networkManager.UnregisterServerMessage<NetItemSpawn_RequestMessage>();
            networkManager.UnregisterServerMessage<NetItemDestroy_RequestMessage>();
            
            networkManager.UnregisterServerMessage<NetItemEnable_RequestMessage>();
            networkManager.UnregisterServerMessage<NetItemDisable_RequestMessage>();
            networkManager.UnregisterServerMessage<NetItemToggleBehaviour_Message>();
            networkManager.UnregisterServerMessage<NetItemToggleBehaviours_Message>();
        }
        
        private void OnServerStart()
        {
            playerItems ??= new Dictionary<NetworkConnectionToClient, List<int>>();
            spawnedItems ??= new Dictionary<int, NetworkIdentity>();
        }
        private void OnServerStop()
        {
            playerItems?.Clear();
            spawnedItems?.Clear();
        }
        /// <summary> Очищает сетевые предметы для конкретного игрока если они есть </summary>
        private void OnServerDisconnect(NetworkConnectionToClient conn, PlayerIdentityConfig identityConfig, NetworkIdentity identity)
        {
            if (playerItems == null) return;
            if (playerItems.Remove(conn, out var list))
            {
                foreach (var assetCode in list)
                    spawnedItems.Remove(assetCode);
            }
        }

        private void ResendToAll<T>(NetworkConnectionToClient conn, T msg) where T : struct, NetworkMessage
        {
            NetworkServer.SendToAll(msg);
        }
        private void SpawnItem(NetworkConnectionToClient conn, NetItemSpawn_RequestMessage msg)
        {
            var viewSource = sourcesContainer.GetAssetView(msg.AssetHashCode);
            var view = spawner.SpawnView(viewSource);
            var netIdentity = view.GetComponent<NetworkIdentity>();
            
            //var netId = $"netId={netIdentity.netId}";
            var connId = $"connId={conn.connectionId}";
            netIdentity.name = $"NetItem [{connId}]";

            if (!playerItems.TryAddValue(conn, msg.RuntimeHashCode))
            {
                Debug.LogError($"Can't add item AssetHashCode=<b>{msg.AssetHashCode}</b> " +
                               $"RuntimeHashCode=<b>{msg.RuntimeHashCode}</b> for connId=<b>{conn.connectionId}</b>");
                return;
            }
             
            if (!spawnedItems.TryAdd(msg.RuntimeHashCode, netIdentity))
                return; //TODO: Изучить проблему. Я не знаю, когда это случается

            NetworkServer.Spawn(netIdentity.gameObject, conn);
        }
        private void DestroyItem(NetworkConnectionToClient conn, NetItemDestroy_RequestMessage msg)
        {
            if (!playerItems.TryRemoveValue(conn, msg.RuntimeHashCode))
            {
                //Debug.LogError($"Can't find items list for connId={conn.connectionId}");
                return;
            }
            if (spawnedItems.Remove(msg.RuntimeHashCode, out var item))
                NetworkServer.Destroy(item.gameObject);
        }
    }
}