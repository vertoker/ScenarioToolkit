using JetBrains.Annotations;
using KBCore.Refs;
using Mirror;
using UnityEngine;
using VRF.Components.Players;
using VRF.Components.Players.Views.NetPlayer;
using VRF.Components.Players.Views.Player;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Networking.Core;
using VRF.Networking.Core.Client;
using VRF.Networking.Messages;
using VRF.Players.Core;
using VRF.Players.Models;
using VRF.Players.Scriptables;
using Zenject;

namespace VRF.Networking.Players
{
    /// <summary>
    /// Конфигуратор для начала работы сетевого игрока.
    /// Используется только для инициализации своей роли
    /// и делает инициализацию NetPlayerView
    /// </summary>
    [RequireComponent(typeof(BaseNetPlayerView))]
    public class NetPlayerViewInitializer : NetworkBehaviour
    {
        [SerializeField, Self] private BaseNetPlayerView netView;
        [SerializeField, ReadOnly] private PlayerIdentityConfig selfIdentity;

        [InjectOptional] private IdentityService identityService;
        [InjectOptional] private ClientMessagesRepeater repeater;
        [InjectOptional] private NetPlayersContainerService netPlayersContainer;
        [InjectOptional] private PlayersContainer playersContainer;
        [InjectOptional] private VRFNetworkManager networkManager;

        public override void OnStartClient()
        {
            repeater.OnInitNetPlayer += ReceiveOnClient;
            playersContainer.PlayerChanged += PlayerChanged;
            SelfInitialize();
        }
        public override void OnStopClient()
        {
            repeater.OnInitNetPlayer -= ReceiveOnClient;
            playersContainer.PlayerChanged -= PlayerChanged;
        }
        
        private void SelfInitialize()
        {
            if (isOwned) // Если был создан локально
                InitFromLocalPlayer(); // То через локального игрока
            else // Если по запросу сервера
                SendInitRequestToServer(); // Идёт запрос на инициализацию на ВСЕХ клиентах сервера
        }
        private void PlayerChanged()
        {
            if (isOwned) // Если был создан локально
                UpdateFromLocalPlayer(); // То через локального игрока
            else // Если по запросу сервера
                SendUpdateRequestToServer(); // Идёт запрос на обновление на ВСЕХ клиентах сервера
        }
        
        private void SendInitRequestToServer()
        {
            // Запрос на самоидентификацию через сервер
            var initRequest = new InitNetPlayer_RequestMessage
                { NetId = netId };
            NetworkClient.Send(initRequest);
        }
        private void SendUpdateRequestToServer()
        {
            // Запрос на самоидентификацию через сервер
            var updateRequest = new UpdateNetPlayer_RequestMessage
                { NetId = netId, NewMode = playersContainer.CurrentKey.Mode };
            NetworkClient.Send(updateRequest);
        }
        private void ReceiveOnClient(InitNetPlayer_Message msg)
        {
            // Если полученный пакет не для этого объекта
            if (msg.NetId != netId) return;
            // Если уже объект инициализирован
            if (selfIdentity) return;
            
            InitFromServer(msg);
        }
        private void ReceiveOnClient(UpdateNetPlayer_Message msg)
        {
            // Если полученный пакет не для этого объекта
            if (msg.NetId != netId) return;
            // Если уже объект инициализирован
            if (selfIdentity) return;
            
            UpdateFromServer(msg);
        }

        private void InitFromLocalPlayer()
        {
            // То это сетевой объект локального игрока
            selfIdentity = identityService.SelfIdentity;
                
            // И инициализируем через локального игрока
            if (playersContainer.CurrentValue.View)
                netView.Initialize(playersContainer.CurrentValue.View);
                
            // И инициализируем суб системы
            Debug.Log($"<b>Init</b> NetPlayer <b>Local</b> - " +
                      $"NetId={netId}, Code={selfIdentity.AssetHashCode}", gameObject);
            OnInitialize();
        }
        private void UpdateFromLocalPlayer()
        {
            Debug.Log($"<b>Update</b> NetPlayer <b>Local</b> - " +
                      $"NetId={netId}, Code={selfIdentity.AssetHashCode}", gameObject);
            // TODO
        }
        private void InitFromServer(InitNetPlayer_Message msg)
        {
            // Получаем роль из сервиса идентификации
            selfIdentity = identityService.Identities[msg.IdentityCode];
            
            // И инициализируем как сетевого игрока другого клиента
            netView.Initialize(selfIdentity, msg.SpawnData);
            
            Debug.Log($"<b>Init</b> NetPlayer <b>Server</b> - " +
                      $"NetId={netId}, Code={selfIdentity.AssetHashCode}", gameObject);
            // И инициализируем суб системы
            OnInitialize();
        }
        private void UpdateFromServer(UpdateNetPlayer_Message msg)
        {
            Debug.Log($"<b>Update</b> NetPlayer <b>Server</b> - " +
                      $"NetId={netId}, Code={selfIdentity.AssetHashCode}", gameObject);
            // TODO
        }

        // Инициализация и деинициализация суб систем
        private void OnInitialize()
        {
            netPlayersContainer?.Add(netView);
        }
        private void OnDispose()
        {
            netPlayersContainer?.Remove(netView);
        }

        // При удалении объекта он удаляется из контейнера существующих
        private void OnDestroy()
        {
            OnDispose();
        }
    }
}