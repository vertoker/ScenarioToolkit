using UnityEngine;
using Zenject;

namespace VRF.Players.Parents
{
    public class PlayerParentsInstaller : MonoInstaller
    {
        [SerializeField] private PlayerParentsContainer parents;
        
        public override void InstallBindings()
        {
            Container.BindInstance(parents).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerParentsService>().AsSingle();
        }
    }
}