using Scenario.Core.Installers.Systems;
using Scenario.Systems;
using Scenario.Utilities.Extensions;
using UnityEngine;

namespace Scenario.Installers
{
    public class AutoAudioSystemInstaller : BaseSystemInstaller
    {
        [SerializeField] private int reservedSources = 5;
        [SerializeField] private Transform self;
        
        public override void InstallBindings()
        {
            Container.BindScenarioSystem<AutoAudioSystem>(GetResolver()).WithArguments(reservedSources, self);
        }
    }
}