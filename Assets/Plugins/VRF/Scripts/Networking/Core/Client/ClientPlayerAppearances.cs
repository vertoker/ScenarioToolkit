using System;
using System.Collections.Generic;
using Mirror;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Networking.Messages;
using VRF.Players.Scriptables;
using Zenject;

namespace VRF.Networking.Core.Client
{
    /// <summary>
    /// Контейнер отображений, синхронизирующий своё состояние с серверным
    /// </summary>
    public class ClientPlayerAppearances : IInitializable, IDisposable
    {
        private readonly VRFNetworkManager networkManager;
        private readonly IdentityService identityService;
        
        private readonly Dictionary<PlayerIdentityConfig, PlayerAppearanceConfig> appearances = new();

        /// <summary> Вызывается, когда у игрока поменялось отображение </summary>
        public event Action<PlayerIdentityConfig, PlayerAppearanceConfig> Updated;
        /// <summary> Вызывается, когда у игрока сбросилось отображение
        /// (второй параметр это старое отображение) </summary>
        public event Action<PlayerIdentityConfig, PlayerAppearanceConfig> Reset;

        public ClientPlayerAppearances(VRFNetworkManager networkManager, 
            [InjectOptional] IdentityService identityService)
        {
            this.networkManager = networkManager;
            this.identityService = identityService;
        }
        
        public void Initialize()
        {
            networkManager.RegisterClientMessage<NetAppearanceUpdate_Message>(OnUpdate);
            networkManager.RegisterClientMessage<NetAppearanceReset_Message>(OnReset);
        }
        public void Dispose()
        {
            networkManager.UnregisterClientMessage<NetAppearanceUpdate_Message>();
            networkManager.UnregisterClientMessage<NetAppearanceReset_Message>();
        }

        private void OnUpdate(NetAppearanceUpdate_Message msg)
        {
            if (identityService.SelfIdentity.AssetHashCode == msg.IdentityCode)
            {
                // Дропаем пакет если локальный игрок является автором сообщения
                return;
            }
            
            var appearance = identityService.Appearances[msg.AppearanceCode];
            var identity = identityService.Identities[msg.IdentityCode];

            if (!appearances.TryAdd(identity, appearance)) return;
            Updated?.Invoke(identity, appearance);
        }
        private void OnReset(NetAppearanceReset_Message msg)
        {
            if (identityService.SelfIdentity.AssetHashCode == msg.IdentityCode)
            {
                // Дропаем пакет если локальный игрок является автором сообщения
                return;
            }
            
            var identity = identityService.Identities[msg.IdentityCode];
            
            if (!appearances.Remove(identity, out var appearance)) return;
            Reset?.Invoke(identity, appearance);
        }
        
        /// <summary> У игрока поменяется сетевое отображение на выбранное </summary>
        /// <param name="identityConfig"> Идентификатор игрока, у которого нужно поменять отображение </param>
        /// <param name="appearanceConfig"> Новое отображение для игрока </param>
        public void SendUpdateAppearance(PlayerIdentityConfig identityConfig, PlayerAppearanceConfig appearanceConfig)
        {
            var message = new NetAppearanceUpdate_RequestMessage
            {
                IdentityCode = identityConfig.AssetHashCode,
                AppearanceCode = appearanceConfig.AssetHashCode
            };
            NetworkClient.Send(message);
        }
        /// <summary> У игрока сброситься сетевое отображение к его стандартному </summary>
        /// <param name="identityConfig"> Идентификатор игрока, у которого нужно поменять отображение </param>
        public void SendResetAppearance(PlayerIdentityConfig identityConfig)
        {
            var message = new NetNetAppearanceReset_RequestMessage
            {
                IdentityCode = identityConfig.AssetHashCode
            };
            NetworkClient.Send(message);
        }
    }
}