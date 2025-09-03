using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using VRF.Components.Players;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Identities.Models;
using VRF.Networking.Core.Server;
using VRF.Networking.Messages;
using VRF.Players.Core;
using VRF.Players.Scriptables;
using VRF.Players.Services;
using Zenject;

namespace VRF.Networking.Core
{
    /// <summary>
    /// Кастомный идентификатор, поддерживает различные способы инициализации
    /// (это определяется внутри самого PlayerIdentity)
    /// </summary>
    public class VRFAuthenticator : NetworkAuthenticator
    {
        /// <summary> Задержка для обработки запросы от конкретного соединения </summary>
        [SerializeField] private float serverRejectDelay = 1f;
        
        /// <summary> Фильтр, чтобы клиенты не посылали множественные
        /// запросы на disconnect (сервер просто не ответит) </summary> 
        private readonly HashSet<NetworkConnection> connectionsPendingDisconnect = new();
        
        [InjectOptional] private IdentitiesConfig identities;
        [InjectOptional] private IdentityService identityService;
        [InjectOptional] private PlayersContainer playersContainer;
        [InjectOptional] private ServerCounterPlayers serverCounterPlayers;

        /// <summary> Вызывается, когда аутентификатор на сервере успешно нашёл идентичность игрока </summary>
        public event Action<NetworkConnectionToClient, PlayerIdentityConfig, NetPlayerSpawnData> OnServerAddIdentity;
        /// <summary> Вызывается, когда аутентификатор на клиенте получает любой ответ от сервера </summary>
        public event Action<bool> OnClientReceiveResponse;
        
        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<AuthNickname_RequestMessage>(OnAuthRequestMessage, false);
            NetworkServer.RegisterHandler<AuthNicknamePassword_RequestMessage>(OnAuthRequestMessage, false);
            NetworkServer.RegisterHandler<AuthIDRequest_Message>(OnAuthRequestMessage, false);
        }
        public override void OnStopServer()
        {
            NetworkServer.UnregisterHandler<AuthNickname_RequestMessage>();
            NetworkServer.UnregisterHandler<AuthNicknamePassword_RequestMessage>();
            NetworkServer.UnregisterHandler<AuthIDRequest_Message>();
        }
        
        /// <summary> Сервер получил запрос на подключение </summary>
        private void OnAuthRequestMessage<TAuthMessage>(NetworkConnectionToClient conn, TAuthMessage msg)
            where TAuthMessage : struct, NetworkMessage, IPlayerConfigMessage
        {
            Debug.Log($"<b>NetAuth</b> Request: {JsonUtility.ToJson(msg)}");
            
            // Фильтр против спама пакетами
            if (connectionsPendingDisconnect.Contains(conn)) return;
            
            // Если по данным о личности можно найти саму личность
            if (identities.TryFindIdentity(msg, out var identity) 
                // И не превышен лимит аккаунтов для личности
                && serverCounterPlayers.IsValidLimit(identity))
                // То сервер подключает игрока, иначе отключает его
                Accept(conn, identity, msg.SpawnData); else Reject(conn);
        }
        /// <summary> Сервер одобрил запрос на подключение </summary>
        private void Accept(NetworkConnectionToClient conn, PlayerIdentityConfig identity, NetPlayerSpawnData spawnData)
        {
            var authResponseMessage = new AuthResponse_Message
            {
                Code = 100,
                Message = "Success"
            };
            
            conn.Send(authResponseMessage);
            ServerAccept(conn);
            OnServerAddIdentity?.Invoke(conn, identity, spawnData);
        }
        /// <summary> Сервер отклонил запрос на подключение </summary>
        private async void Reject(NetworkConnectionToClient conn)
        {
            connectionsPendingDisconnect.Add(conn);
            
            var authResponseMessage = new AuthResponse_Message
            {
                Code = 200,
                Message = "Invalid Credentials"
            };

            conn.Send(authResponseMessage);
            conn.isAuthenticated = false;

            await UniTask.WaitForSeconds(serverRejectDelay);
            
            ServerReject(conn);
            connectionsPendingDisconnect.Remove(conn);
        }
        
        
        public override void OnStartClient()
        {
            NetworkClient.RegisterHandler<AuthResponse_Message>(OnAuthResponseMessage, false);
        }
        public override void OnStopClient()
        {
            NetworkClient.UnregisterHandler<AuthResponse_Message>();
        }
        
        /// <summary> Вызывается, когда приходит время авторизации </summary>
        public override void OnClientAuthenticate()
        {
            if (identityService == null) return;
            if (!identityService.SelfIdentity)
            {
                Debug.LogWarning("Can't log into server, doesn't have SelfIdentity", gameObject);
                return;
            }
            
            var authIdentity = identityService.SelfIdentity.AuthIdentityModel;
            var spawnData = new NetPlayerSpawnData(playersContainer.CurrentKey.Mode);
            
            switch (authIdentity.AuthMode)
            {
                case IdentityAuthMode.Nickname:
                    var nicknameMessage = new AuthNickname_RequestMessage
                    {
                        Nickname = authIdentity.IdentityName,
                        spawnData = spawnData
                    };
                    NetworkClient.Send(nicknameMessage);
                    break;
                case IdentityAuthMode.NicknamePassword:
                    var nicknamePasswordMessage = new AuthNicknamePassword_RequestMessage
                    {
                        Nickname = authIdentity.IdentityName, 
                        Password = authIdentity.Password,
                        spawnData = spawnData
                    };
                    NetworkClient.Send(nicknamePasswordMessage);
                    break;
                case IdentityAuthMode.ID:
                    var idMessage = new AuthIDRequest_Message
                    {
                        ID = authIdentity.IdentityID,
                        spawnData = spawnData
                    };
                    NetworkClient.Send(idMessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void OnAuthResponseMessage(AuthResponse_Message msg)
        {
            Debug.Log($"<b>NetAuth</b> Response: {msg.Message}");
            
            var accept = msg.Code == 100;
            if (accept) ClientAccept(); else ClientReject();
            OnClientReceiveResponse?.Invoke(accept);
        }
    }
}