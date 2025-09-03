using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using VRF.DataSources;
using VRF.Identities;
using VRF.Identities.Models;
using VRF.Networking.Core;
using VRF.Networking.Models;
using Zenject;

namespace VRF.Networking.Init
{
    /// <summary>
    /// Инициализирует и запускает сеть, инициализирует идентификацию
    /// </summary>
    public class NetworkLauncher : IInitializable, IDisposable
    {
        private readonly ContainerDataSource dataSource;

        [CanBeNull] private readonly VRFNetworkManager networkManager;
        private readonly DataSourceType[] networkSources;
        private readonly bool initialize, dispose;

        public static bool ActiveNetwork => NetworkServer.active || NetworkClient.active;

        public NetworkLauncher(ContainerDataSource dataSource, [InjectOptional] VRFNetworkManager networkManager,
            DataSourceType[] networkSources, bool initialize, bool dispose)
        {
            this.networkManager = networkManager;
            this.dataSource = dataSource;
            this.networkSources = networkSources;
            this.initialize = initialize;
            this.dispose = dispose;
        }
        public void Initialize()
        {
            if (initialize)
                StartNetwork(networkSources);
        }
        public void Dispose()
        {
            if (dispose)
                ShutdownNetwork();
        }
        
        public void StartNetwork(IEnumerable<DataSourceType> localNetworkSources)
        {
            if (!networkManager) return;
            if (ActiveNetwork) return;
            
            var data = dataSource.Load<NetModel>(localNetworkSources);
            
            if (data == null)
            {
                Debug.LogWarning($"Empty <b>{nameof(NetModel)}</b>, abort network initialization");
                return;
            }

            networkManager.networkAddress = data.Address;
            networkManager.networkPort = data.Port;

            Debug.Log($"<b>Start</b> Net " +
                      $"mode=<b>{data.NetMode.ToString()}</b>, " +
                      $"address=<b>{data.Address}</b>, " +
                      $"port=<b>{data.Port}</b>");
            
            switch (data.NetMode)
            {
                case NetworkManagerMode.Offline:
                    break;
                case NetworkManagerMode.ServerOnly:
                    networkManager.StartServer();
                    break;
                case NetworkManagerMode.ClientOnly:
                    networkManager.StartClient();
                    break;
                case NetworkManagerMode.Host:
                    networkManager.StartHost();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void ShutdownNetwork()
        {
            if (!networkManager) return;
            if (!ActiveNetwork) return;
            
            Debug.Log($"<b>Shutdown</b> Net " +
                      $"mode=<b>{networkManager.mode}</b>");
            
            switch (networkManager.mode)
            {
                case NetworkManagerMode.Offline:
                    break;
                case NetworkManagerMode.ServerOnly:
                    networkManager.StopServer();
                    break;
                case NetworkManagerMode.ClientOnly:
                    networkManager.StopClient();
                    break;
                case NetworkManagerMode.Host:
                    networkManager.StopHost();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}