using Scenario.Core.Installers.Systems;
using Scenario.Utilities.Extensions;
using SimpleUI.Scriptables.Core;
using UnityEngine;
using VRF.Scenario.Services;

namespace VRF.Scenario.Installers.Services
{
    public class PlayerScenarioScreensServiceInstaller : BaseSystemInstaller, IDebugParam
    {
        [SerializeField] private ScreenStorage infoStorage;
        [SerializeField] private ScreenStorage timerStorage;
        [SerializeField] private bool debug;

        public override void InstallBindings()
        {
            var entry = new PlayerScenarioScreensService.ConstructEntry
            {
                Info = infoStorage,
                Timer = timerStorage
            };
            Container.BindScenarioSystem<PlayerScenarioScreensService>(GetResolver())
                .WithArguments(entry, (IDebugParam)this);
        }

        public bool Debug => debug;
    }
}