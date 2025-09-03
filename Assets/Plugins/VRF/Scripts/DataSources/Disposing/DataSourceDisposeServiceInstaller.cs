using Zenject;

namespace VRF.DataSources.Disposing
{
    public class DataSourceDisposeServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DataSourceDisposeService>().AsSingle();
        }
    }
}