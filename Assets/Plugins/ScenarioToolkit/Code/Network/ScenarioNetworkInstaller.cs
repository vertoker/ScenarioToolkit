using Zenject;

namespace ScenarioToolkit.Network
{
    /// <summary>
    /// Опциональное дополнение в существующему сетевому модулю VRF, полностью опционален
    /// </summary>
    public class ScenarioNetworkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // All
            // Container.BindInterfacesAndSelfTo<ScenarioStateContainer>().AsSingle();
            
            // Server/Host
            // Container.BindInterfacesAndSelfTo<ScenarioNetServer>().AsSingle();
            // Container.BindInterfacesAndSelfTo<ScenarioNetServerNodes>().AsSingle();
            // Container.BindInterfacesAndSelfTo<ScenarioNetServerMsgBuffer>().AsSingle();
            // Container.BindInterfacesAndSelfTo<ScenarioNetClientDummy>().AsSingle();
            
            // Client
            // Container.BindInterfacesAndSelfTo<ScenarioNetClient>().AsSingle();
        }
    }
}