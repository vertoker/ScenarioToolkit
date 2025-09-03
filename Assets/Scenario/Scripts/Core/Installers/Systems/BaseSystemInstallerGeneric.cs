using Scenario.Utilities.Extensions;

namespace Scenario.Core.Installers.Systems
{
    public abstract class BaseSystemInstallerGeneric<TSystem> : BaseSystemInstaller
        where TSystem : class
    {
        public override void InstallBindings()
        {
            Container.BindScenarioSystem<TSystem>(GetResolver());
        }
    }
}