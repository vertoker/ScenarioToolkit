using Mirror;
using UnityEngine;
using VRF.Networking.Services;
using Zenject;

namespace VRF.Networking.Core
{
    public class NetworkHUDInstaller : MonoInstaller
    {
        [Header("Network HUD")]
        [SerializeField] private KeyCode toggleHUDKeyCode = KeyCode.F2;

        [SerializeField] private bool showHUDInEditor = true;
        [SerializeField] private bool showHUDInRuntime = false;
        
        [Header("Fields")]
        [SerializeField] private NetworkManagerHUD networkManagerHUD;

        public override void InstallBindings()
        {
            var showHUD = Application.isEditor ? showHUDInEditor : showHUDInRuntime;
            var toggler = new NetworkHUDToggleService(networkManagerHUD, toggleHUDKeyCode, showHUD);
            
            Container.BindInstance(networkManagerHUD).AsSingle();
            Container.BindInterfacesAndSelfTo<NetworkHUDToggleService>()
                .FromInstance(toggler).AsSingle();
        }
    }
}