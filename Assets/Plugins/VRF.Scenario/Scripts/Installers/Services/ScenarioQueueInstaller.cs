using Scenario.Core.Installers.Systems;
using Scenario.Utilities.Extensions;
using UnityEngine;
using VRF.Scenario.Services;

namespace VRF.Scenario.Installers.Services
{
    public class ScenarioQueueInstaller : BaseSystemInstaller
    {
        [SerializeField] private ScenarioModuleLoader.ModeParameters parameters = new();
        
        public override void InstallBindings()
        {
            Container.BindScenarioSystem<ScenarioQueueService>(GetResolver()).WithArguments(parameters);
        }
    }
}