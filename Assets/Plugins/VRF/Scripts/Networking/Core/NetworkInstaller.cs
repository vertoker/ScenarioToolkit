using UnityEngine;
using VRF.Networking.Core.Client;
using VRF.Networking.Core.Server;
using VRF.Networking.Init;
using VRF.Networking.Players;
using VRF.Networking.Services;
using Zenject;

namespace VRF.Networking.Core
{
    /// <summary>
    /// Главный, основной и единственный инсталлер всех основных сервисов для работы мультиплеера.
    /// Не является инициализацией сети, для этого есть другой инсталлер
    /// </summary>
    public class NetworkInstaller : MonoInstaller
    {
        [SerializeField] private VRFNetworkManager networkManager;
        [SerializeField] private VRFAuthenticator authenticator;

        public override void InstallBindings()
        {
            // Core
            Container.BindInstance(networkManager).AsSingle();
            networkManager.authenticator = authenticator;
            Container.BindInstance(authenticator).AsSingle();
            
            // Scenes
            Container.BindInterfacesAndSelfTo<ServerScenesService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientNetScenesService>().AsSingle();
            
            // Views
            Container.BindInterfacesAndSelfTo<RegisterNetPrefabsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ViewsNetSourcesContainer>().AsSingle();
            
            // Players
            Container.BindInterfacesAndSelfTo<NetPlayersContainerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientPlayerAppearances>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientMessagesRepeater>().AsSingle();
            
            // Items
            Container.BindInterfacesAndSelfTo<ServerItemsContainer>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientItemsService>().AsSingle();
            
            // Server
            Container.BindInterfacesAndSelfTo<ServerAppearancesContainer>().AsSingle();
            Container.BindInterfacesAndSelfTo<ServerCounterPlayers>().AsSingle();
        }
    }
}