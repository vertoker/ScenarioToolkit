using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace VRF.Networking.Init.NetPlayers
{
    public class NetPlayersInstaller : MonoInstaller
    {
        [SerializeField] private bool spawn = true;
        [SerializeField] private bool destroy = true;

        public override void InstallBindings()
        {
            if (spawn) Container.BindInterfacesAndSelfTo<NetPlayersSpawner>().AsSingle();
            else Container.Bind<NetPlayersSpawner>().AsSingle();
            
            if (destroy) Container.BindInterfacesAndSelfTo<NetPlayersDestroyer>().AsSingle();
            else Container.Bind<NetPlayersDestroyer>().AsSingle();
        }

#if UNITY_EDITOR
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void SpawnPlayers()
        {
            Container.Resolve<NetPlayersSpawner>().SpawnPlayers();
        }
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void DespawnPlayers()
        {
            Container.Resolve<NetPlayersDestroyer>().DespawnPlayers();
        }
#endif
    }
}