using Zenject;

namespace VRF.Players.Core
{
    public class PlayersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayersContainer>().AsSingle();
        }
    }
}