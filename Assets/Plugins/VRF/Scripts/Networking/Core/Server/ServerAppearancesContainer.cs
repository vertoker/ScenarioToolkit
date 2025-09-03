using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Networking.Messages;
using VRF.Players.Scriptables;
using Zenject;

namespace VRF.Networking.Core.Server
{
    /// <summary>
    /// Контейнер всех изменённых внешностей, синхронизирует внешний вид
    /// всех изменённых игроков и всех клиентов
    /// </summary>
    public class ServerAppearancesContainer : IInitializable, IDisposable
    {
        private Dictionary<PlayerIdentityConfig, PlayerAppearanceConfig> playerAppearances;

        private readonly VRFNetworkManager networkManager;
        private readonly IdentityService identityService;

        public ServerAppearancesContainer(VRFNetworkManager networkManager, [InjectOptional] IdentityService identityService)
        {
            this.networkManager = networkManager;
            this.identityService = identityService;
        }

        public void Initialize()
        {
            networkManager.OnServerStartEvent += OnServerStart;
            networkManager.OnServerStopEvent += OnServerStop;
            networkManager.OnServerDisconnectEvent += OnServerDisconnect;
            networkManager.OnServerPlayerAuthorized += SendSyncAppearances;
            
            networkManager.RegisterServerMessage<NetAppearanceUpdate_RequestMessage>(UpdateAppearance);
            networkManager.RegisterServerMessage<NetNetAppearanceReset_RequestMessage>(ResetAppearance);
        }
        public void Dispose()
        {
            networkManager.OnServerStartEvent -= OnServerStart;
            networkManager.OnServerStopEvent -= OnServerStop;
            networkManager.OnServerDisconnectEvent -= OnServerDisconnect;
            networkManager.OnServerPlayerAuthorized -= SendSyncAppearances;
            
            networkManager.UnregisterServerMessage<NetAppearanceUpdate_RequestMessage>();
            networkManager.UnregisterServerMessage<NetNetAppearanceReset_RequestMessage>();
        }
        
        private void OnServerStart()
        {
            playerAppearances ??= new Dictionary<PlayerIdentityConfig, PlayerAppearanceConfig>();
        }
        private void OnServerStop()
        {
            playerAppearances?.Clear();
        }
        private void OnServerDisconnect(NetworkConnectionToClient conn, PlayerIdentityConfig identityConfig, NetworkIdentity identity)
        {
            if (identityConfig)
                playerAppearances?.Remove(identityConfig);
        }
        
        /// <summary> Отправляет все изменённые облики новому игроку </summary>
        private void SendSyncAppearances(NetworkConnectionToClient conn, PlayerIdentityConfig identityConfig)
        {
            foreach (var appearance in playerAppearances)
            {
                var updateMsg = new NetAppearanceUpdate_Message
                {
                    IdentityCode = appearance.Key.AssetHashCode,
                    AppearanceCode = appearance.Value.AssetHashCode
                };
                
                conn.Send(updateMsg);
            }
        }
        
        /// <summary> Обновляет облик у конкретного игрока </summary>
        private void UpdateAppearance(NetworkConnectionToClient conn, NetAppearanceUpdate_RequestMessage msg)
        {
            if (!identityService.Appearances.TryGetValue(msg.AppearanceCode, out var appearance))
            {
                Debug.LogError($"Can't resolve appearance code <b>{msg.AppearanceCode}</b>"); 
                return;
            }
            if (!identityService.Identities.TryGetValue(msg.IdentityCode, out var identity))
            {
                Debug.LogError($"Can't resolve identity code <b>{msg.IdentityCode}</b>"); 
                return;
            }
            if (!playerAppearances.TryAdd(identity, appearance)) return;
            
            var updateMsg = new NetAppearanceUpdate_Message
            {
                IdentityCode = msg.IdentityCode,
                AppearanceCode = msg.AppearanceCode
            };
            
            NetworkServer.SendToAll(updateMsg);
        }
        /// <summary> Сбрасывает облик у конкретного игрока до стандартного вида </summary>
        private void ResetAppearance(NetworkConnectionToClient conn, NetNetAppearanceReset_RequestMessage msg)
        {
            if (!identityService.Identities.TryGetValue(msg.IdentityCode, out var identity))
            {
                Debug.LogError($"Can't resolve identity code <b>{msg.IdentityCode}</b>"); 
                return;
            }
            if (!playerAppearances.Remove(identity)) return;
            
            var resetMsg = new NetAppearanceReset_Message
            {
                IdentityCode = msg.IdentityCode
            };
            
            NetworkServer.SendToAll(resetMsg);
        }
    }
}