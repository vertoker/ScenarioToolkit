using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using VRF.Networking.Core.Interfaces;
using VRF.Networking.Core.Server;
using VRF.Networking.Messages;
using VRF.Players.Scriptables;
using VRF.Players.Services.Views;
using VRF.Scenes.Project;
using Zenject;

namespace VRF.Networking.Core
{
    /// <summary>
    /// Сетевой менеджер для всех систем VRF, которые хоть как-то связаны с сетью.
    /// В себе содержит систему игроков и смены сцены, а в остальном полностью универсален
    /// </summary>
    public class VRFNetworkManager : NetworkManager, 
        INetworkServerEvents, INetworkServerMessages,
        INetworkClientEvents, INetworkClientMessages
    {
        /// <summary>
        /// Интерфейс для работы с портом для удобства, по аналогии с networkAddress
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public ushort networkPort
        {
            get => transport is PortTransport portTransport ? portTransport.Port : default;
            set
            {
                if (transport is PortTransport portTransport)
                    portTransport.Port = value;
            }
        }

        // /////
        // Both
        // /////
        
        private VRFAuthenticator vrfAuthenticator;

        [Inject] private ScenesService scenesService;
        [Inject] private ProjectViewSpawnerService spawner;
        [Inject] private ViewsSpawnedContainer spawnedContainer;
        
        // /////
        // Server
        // /////
        
        public event Action OnServerStartEvent;
        public event Action OnServerStopEvent;
        
        private readonly Dictionary<ushort, Action> startServerMessages = new();
        private readonly Dictionary<ushort, Action> stopServerMessages = new();

        /// <summary> Вызывается когда клиент авторизовался и
        /// подключился (но ещё не идентифицировал свою роль) </summary>
        public event Action<NetworkConnectionToClient, PlayerIdentityConfig> OnServerClientAuthorized;
        /// <summary> Вызывается когда объект игрок запросил свою идентификацию и сервер исполнил запрос </summary>
        public event Action<NetworkConnectionToClient, PlayerIdentityConfig> OnServerPlayerAuthorized;
        /// <summary> Вызывается когда от сервера отключается клиент (и удаляется из списков игроков) </summary>
        public event Action<NetworkConnectionToClient, PlayerIdentityConfig, NetworkIdentity> OnServerDisconnectEvent;
        // TODO декомпозировать на несколько ивентов

        // TODO разобраться когда он вызывается и прокомментировать
        public event Action<string, SceneOperation> OnServerClientChangeSceneEvent;
        public event Action<string> OnServerSceneChangedEvent;
        public event Action<NetworkConnectionToClient> OnServerReadyEvent;

        private Dictionary<NetworkConnectionToClient, PlayerIdentityConfig> clients;
        private Dictionary<NetworkConnectionToClient, NetPlayerSpawnData> clientConfigs;
        private Dictionary<NetworkConnectionToClient, NetworkIdentity> spawnedPlayerIdentities;
        private Dictionary<NetworkIdentity, PlayerIdentityConfig> playerIdentities;

        /// <summary> Все подключённые клиенты и их роли </summary>
        public IReadOnlyDictionary<NetworkConnectionToClient, PlayerIdentityConfig> ServerClients => clients;
        /// <summary> Все подключённые клиенты и их роли </summary>
        public IReadOnlyDictionary<NetworkConnectionToClient, NetworkIdentity> ServerSpawnedIdentities => spawnedPlayerIdentities;

        /// <summary> Условие для спауна игроков, спаунит игроков
        /// у уже подключенных клиентов и у новых клиентов </summary>
        private bool spawnPlayers;

        // /////
        // Client
        // /////
        
        public event Action OnClientStartEvent;
        public event Action OnClientStopEvent;
        
        private readonly Dictionary<ushort, Action> startClientMessages = new();
        private readonly Dictionary<ushort, Action> stopClientMessages = new();
        
        public event Action OnClientConnectEvent;
        public event Action OnClientDisconnectEvent;
        
        /// <summary> Подключен ли клиент к серверу? </summary>
        public bool ClientConnected { get; private set; }
        /// <summary> Вызывается когда сервер отправляет ответ об успешном подключении к себе </summary>
        public event Action OnClientAuthorized;
        
        // TODO разобраться когда он вызывается и прокомментировать
        public event Action OnClientSceneChangedEvent;

        // TODO сетевая смена сцен СЛОМАНА, надо чинить её
        
        #region Scene Management
        
        // Данный блок перезаписанных методов рассчитан конкретно под NetScenesService
        // Большинство методов связанные со сменой сцен лучше не трогать

        public override void ServerChangeScene(string newSceneName)
        {
            Debug.LogError($"Use {nameof(ServerScenesService)} for loading");
        }

        public void UpdateActiveScene(string sceneName) => networkSceneName = sceneName;
        public new void FinishLoadScene() => base.FinishLoadScene();

        // Метод по особенному перезаписывается так, чтобы при вызове ивента
        // Уже происходила сама загрузка сцены
        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation,
            bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);

            if (NetworkServer.active) return;
            NetworkClient.isLoadingScene = true;

            OnServerClientChangeSceneEvent?.Invoke(newSceneName, sceneOperation);
        }

        public override void OnClientSceneChanged()
        {
            OnClientSceneChangedEvent?.Invoke();
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            OnServerSceneChangedEvent?.Invoke(sceneName);
        }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);
            OnServerReadyEvent?.Invoke(conn);
        }

        #endregion

        #region Players
        /// <summary> Заспаунить всех игроков (в том числе и для тех, кто подключится позже) </summary>
        public void SpawnNetPlayers()
        {
            if (spawnPlayers) return;
            spawnPlayers = true;
            Debug.Log(nameof(SpawnNetPlayers));

            foreach (var client in clients)
                SpawnNetPlayer(client.Key, client.Value);
        }
        /// <summary> Задеспаунить всех игроков </summary>
        public void DespawnNetPlayers()
        {
            if (!spawnPlayers) return;
            spawnPlayers = false;
            Debug.Log(nameof(DespawnNetPlayers));

            foreach (var netPlayer in spawnedPlayerIdentities.Values)
            {
                playerIdentities.Remove(netPlayer);
                spawnedContainer.Remove(netPlayer);
                NetworkServer.Destroy(netPlayer.gameObject);
            }

            spawnedPlayerIdentities.Clear();
        }
        private void SpawnNetPlayer(NetworkConnectionToClient conn, PlayerIdentityConfig identity)
        {
            var spawnData = clientConfigs[conn];
            var viewSource = identity.Appearance.GetNetPlayerPrefab(spawnData.CurrentMode);
            var view = spawner.SpawnView(viewSource);
            var netPlayer = view.GetComponent<NetworkIdentity>();

            var netId = $"netId={netPlayer.netId}";
            var connId = $"connId={conn.connectionId}";
            netPlayer.name = $"NetPlayer [{netId}, {connId}]";

            spawnedPlayerIdentities.Add(conn, netPlayer);
            playerIdentities.Add(netPlayer, identity);
            NetworkServer.Spawn(netPlayer.gameObject, conn);
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            var player = spawner.Spawn<VRFNetworkManager>(playerPrefab);
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

            NetworkServer.AddPlayerForConnection(conn, player);
        }

        #endregion

        #region Server
        /*public override void OnServerConnect(NetworkConnectionToClient conn) { base.OnServerConnect(conn); }*/
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            NetworkServer.DestroyPlayerForConnection(conn);
            clients.Remove(conn, out var identity);
            clientConfigs.Remove(conn);
            
            if (spawnedPlayerIdentities.Remove(conn, out var netPlayer))
                playerIdentities.Remove(netPlayer);

            OnServerDisconnectEvent?.Invoke(conn, identity, netPlayer);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            clients ??= new Dictionary<NetworkConnectionToClient, PlayerIdentityConfig>();
            clientConfigs ??= new Dictionary<NetworkConnectionToClient, NetPlayerSpawnData>();
            spawnedPlayerIdentities ??= new Dictionary<NetworkConnectionToClient, NetworkIdentity>();
            playerIdentities ??= new Dictionary<NetworkIdentity, PlayerIdentityConfig>();

            NetworkServer.RegisterHandler<InitNetPlayer_RequestMessage>(OnCreatePlayer);
            NetworkServer.RegisterHandler<UpdateNetPlayer_RequestMessage>(OnUpdatePlayer);
            NetworkServer.RegisterHandler<SceneUpdate_Message>(OnSceneUpdate);

            vrfAuthenticator = (VRFAuthenticator)authenticator;
            vrfAuthenticator.OnServerAddIdentity += OnServerAuthorize;
            
            foreach (var serverMessage in startServerMessages.Values)
                serverMessage?.Invoke();
            OnServerStartEvent?.Invoke();
        }
        public override void OnStopServer()
        {
            base.OnStopServer();
            
            foreach (var serverMessage in stopServerMessages.Values)
                serverMessage?.Invoke();
            OnServerStopEvent?.Invoke();

            vrfAuthenticator.OnServerAddIdentity -= OnServerAuthorize;

            NetworkServer.UnregisterHandler<InitNetPlayer_RequestMessage>();
            NetworkServer.UnregisterHandler<UpdateNetPlayer_RequestMessage>();
            NetworkServer.UnregisterHandler<SceneUpdate_Message>();
            
            clients.Clear();
            clientConfigs.Clear();
            spawnedPlayerIdentities.Clear();
            playerIdentities.Clear();
        }

        public void RegisterServerMessage<T>(Action<NetworkConnectionToClient, T> handler, bool requireAuthentication = true)
            where T : struct, NetworkMessage
        {
            var id = NetworkMessageId<T>.Id;
            startServerMessages.Add(id, StartServerMessage);
            stopServerMessages.Add(id, StopServerMessage);

            if (NetworkServer.active)
                StartServerMessage();
            return;
            
            void StartServerMessage() => NetworkServer.RegisterHandler(handler, requireAuthentication);
            void StopServerMessage() => NetworkServer.UnregisterHandler<T>();
        }
        public void UnregisterServerMessage<T>()
            where T : struct, NetworkMessage
        {
            var id = NetworkMessageId<T>.Id;
            
            startServerMessages.Remove(id);
            if (stopServerMessages.Remove(id, out var stopAction))
                stopAction.Invoke();
        }
        #endregion

        #region Client
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            vrfAuthenticator = (VRFAuthenticator)authenticator;
            vrfAuthenticator.OnClientReceiveResponse += OnClientAuthorize;
            
            foreach (var serverMessage in startClientMessages.Values)
                serverMessage?.Invoke();
            OnClientStartEvent?.Invoke();
        }
        public override void OnStopClient()
        {
            base.OnStopClient();
            
            foreach (var serverMessage in stopClientMessages.Values)
                serverMessage?.Invoke();
            OnClientStopEvent?.Invoke();
            
            vrfAuthenticator.OnClientReceiveResponse -= OnClientAuthorize;
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            OnClientConnectEvent?.Invoke();
        }
        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            OnClientDisconnectEvent?.Invoke();
        }

        public void RegisterClientMessage<T>(Action<T> handler, bool requireAuthentication = true)
            where T : struct, NetworkMessage
        {
            var id = NetworkMessageId<T>.Id;
            startClientMessages.Add(id, StartClientMessage);
            stopClientMessages.Add(id, StopClientMessage);

            if (NetworkClient.active)
                StartClientMessage();
            return;
            
            void StartClientMessage() => NetworkClient.RegisterHandler(handler, requireAuthentication);
            void StopClientMessage() => NetworkClient.UnregisterHandler<T>();
        }
        public void UnregisterClientMessage<T>()
            where T : struct, NetworkMessage
        {
            var id = NetworkMessageId<T>.Id;
            
            startClientMessages.Remove(id);
            if (stopClientMessages.Remove(id, out var stopAction))
                stopAction.Invoke();
        }
        #endregion

        #region Messages
        /// <summary> Вызывается аутентификатором на сервере если клиент прошёл идентификацию </summary>
        private void OnServerAuthorize(NetworkConnectionToClient conn, 
            PlayerIdentityConfig identity, NetPlayerSpawnData spawnData)
        {
            clients.Add(conn, identity);
            clientConfigs.Add(conn, spawnData);
            
            OnServerClientAuthorized?.Invoke(conn, identity);
        }
        /// <summary> Вызывается аутентификотором на клиенте после успешного ответа сервера </summary>
        private void OnClientAuthorize(bool authorized)
        {
            ClientConnected = authorized;
            if (authorized) OnClientAuthorized?.Invoke();
        }
        
        /// <summary> Вызывается игроком для отправки идентификации своей идентификации </summary>
        private void OnCreatePlayer(NetworkConnectionToClient conn, InitNetPlayer_RequestMessage msg)
        {
            var netPlayer = NetworkServer.spawned[msg.NetId];
            var identity = playerIdentities[netPlayer];

            var toClientsIdentity = new InitNetPlayer_Message
            {
                IdentityCode = identity.AssetHashCode,
                NetId = msg.NetId
            };

            NetworkServer.SendToAll(toClientsIdentity); // все должны знать что это за объект
            OnServerPlayerAuthorized?.Invoke(conn, identity);
        }
        /// <summary> Вызывается игроком для отправки идентификации своей идентификации </summary>
        private void OnUpdatePlayer(NetworkConnectionToClient conn, UpdateNetPlayer_RequestMessage msg)
        {
            // TODO 
        }
        
        /// <summary> Вызывается когда игрок загрузит новую сцену </summary>
        private void OnSceneUpdate(NetworkConnectionToClient conn, SceneUpdate_Message msg)
        {
            // Пробуем заспаунить игрока для клиента
            if (spawnPlayers)
            {
                // Если клиент не авторизовался
                if (!clients.TryGetValue(conn, out var identity))
                {
                    Debug.LogWarning($"Client is not authorized, conn={conn.connectionId}");
                    return;
                }
                // Если игрок уже создан
                if (spawnedPlayerIdentities.TryGetValue(conn, out var netIdentity))
                {
                    Debug.LogWarning($"Player object is already spawned, netId={netIdentity.netId}");
                    return;
                }
                // Если сцена игрока не та же, что и у сервера
                if (msg.SceneIndex != scenesService.SceneIndex)
                {
                    Debug.LogWarning($"Server and client scene mismatch, server={scenesService.SceneIndex}, client={msg.SceneIndex}");
                    return;
                }

                SpawnNetPlayer(conn, identity);
            }
        }

        #endregion

        public override void OnValidate()
        {
            base.OnValidate();

            // Запуск всегда происходит извне отдельными скриптами
            headlessStartMode = HeadlessStartOptions.DoNothing;
            // Данную функцию уже предоставляет ProjectContext или SceneContext
            dontDestroyOnLoad = false;

            // Администрированием сцен занимается отдельный сервис
            offlineScene = string.Empty;
            onlineScene = string.Empty;

            // Стартовые позиции задаются отдельными точками
            startPositionIndex = 0;
            startPositions.Clear();

            // Аутентификатор задаётся через соответствующий инсталлятор
            // Если он не сработает, то аутентификация не требуется
            authenticator = null;
        }
    }
}