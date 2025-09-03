using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using VRF.Scenes.Project;
using Zenject;

namespace VRF.Networking.Core.Server
{
    // TODO закомментировать его полностью и проверить на работоспособность
    public class ServerScenesService : IInitializable, IDisposable
    {
        /// <summary> Вызывается если сцена на всех клиентах и самом сервере была изменена </summary>
        public event Action<string> OnNetSceneChanged;
        
        private readonly ScenesService scenesService;
        private readonly VRFNetworkManager networkManager;
        
        private readonly HashSet<NetworkConnectionToClient> notReadyClients;
        
        private SceneOperation clientSceneOperation;
        private bool inLoadingServer;
        private bool inLoadingClients;
        private string currentSceneName;

        public ServerScenesService(ScenesService scenesService, VRFNetworkManager networkManager)
        {
            this.scenesService = scenesService;
            this.networkManager = networkManager;
            notReadyClients = new HashSet<NetworkConnectionToClient>();
        }

        public void Initialize()
        {
            networkManager.OnServerClientChangeSceneEvent += OnClientChangeScene;
            networkManager.OnServerSceneChangedEvent += OnServerSceneChanged;
            networkManager.OnClientSceneChangedEvent += OnClientSceneChanged;
            networkManager.OnServerReadyEvent += OnServerReady;
        }
        public void Dispose()
        {
            networkManager.OnServerClientChangeSceneEvent -= OnClientChangeScene;
            networkManager.OnServerSceneChangedEvent -= OnServerSceneChanged;
            networkManager.OnClientSceneChangedEvent -= OnClientSceneChanged;
            networkManager.OnServerReadyEvent -= OnServerReady;
        }

        // TODO добавить возможность автоматического нового игрока, если он подключился
        // TODO добавить остальные методы из ScenesService
        // TODO вынести основные публичные методы из ScenesService в интерфейс и применить сюда
        
        [Server]
        public void LoadScene(string sceneName)
        {
            if (!NetworkServer.active)
            {
                Debug.LogError("ServerChangeScene can only be called on an active server");
                return;
            }
            
            if (inLoadingClients)
            {
                Debug.LogError("Server in loading process");
                return;
            }
            
            if (inLoadingServer)
            {
                Debug.LogError("Clients in loading process");
                return;
            }
            
            if (!scenesService.ClientScenes.Contains(sceneName))
            {
                Debug.LogError($"Can't load scenes if config doesn't have it");
                return;
            }

            inLoadingServer = true;
            foreach (var client in networkManager.ServerClients.Keys)
                notReadyClients.Add(client);
            inLoadingClients = true;
            currentSceneName = sceneName;
            
            NetworkServer.SetAllClientsNotReady();
            networkManager.UpdateActiveScene(sceneName);

            NetworkServer.isLoadingScene = true;

            StartLoadScene(sceneName);

            if (NetworkServer.active)
            {
                NetworkServer.SendToAll(new SceneMessage 
                    { sceneName = sceneName, customHandling = true});
            }
        }
        
        private void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation)
        {
            clientSceneOperation = sceneOperation;
            
            switch (sceneOperation)
            {
                case SceneOperation.Normal:
                    StartLoadScene(newSceneName);
                    networkManager.UpdateActiveScene(newSceneName);
                    break;
                case SceneOperation.LoadAdditive:
                    throw new NotImplementedException();
                case SceneOperation.UnloadAdditive:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneOperation), sceneOperation, null);
            }
        }
        
        // Работает как для сервера, так и для клиента
        private void StartLoadScene(string sceneName)
        {
            // loadingSceneAsync не заполняется, FinishLoadScene вызывается вручную
            scenesService.OnSceneLoaded += OnSceneLoaded;
            scenesService.LoadScene(sceneName);
        }
        private void OnSceneLoaded(string sceneName)
        {
            scenesService.OnSceneLoaded -= OnSceneLoaded;
            networkManager.FinishLoadScene();
        }
        
        private void OnClientSceneChanged()
        {
            if (NetworkClient.connection.isAuthenticated && !NetworkClient.ready)
                NetworkClient.Ready();
            
            if (NetworkClient.connection.isAuthenticated 
                && clientSceneOperation == SceneOperation.Normal 
                && networkManager.autoCreatePlayer
                && NetworkClient.localPlayer == null) 
            { NetworkClient.AddPlayer(); }
        }
        
        private void OnServerSceneChanged(string sceneName)
        {
            inLoadingServer = false;
            TryFinishClientsLoading();
            TrySendEvent();
        }
        
        private void OnServerReady(NetworkConnectionToClient conn)
        {
            if (inLoadingClients && notReadyClients.Remove(conn))
            {
                TryFinishClientsLoading();
                TrySendEvent();
            }
        }
        
        private void TryFinishClientsLoading()
        {
            if (notReadyClients.Count != 0) return;
            inLoadingClients = false;
        }
        private void TrySendEvent()
        {
            if (!inLoadingServer && !inLoadingClients)
                OnNetSceneChanged?.Invoke(currentSceneName);
        }
    }
}