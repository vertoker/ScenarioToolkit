using ScenarioToolkit.Core.Installers.Systems;
using ScenarioToolkit.Library.Systems;
using ScenarioToolkit.Shared.Extensions;
using UnityEngine;

namespace ScenarioToolkit.Library.Installers
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