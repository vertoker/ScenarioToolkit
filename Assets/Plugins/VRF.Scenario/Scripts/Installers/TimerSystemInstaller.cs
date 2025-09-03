using Scenario.Core.Installers.Systems;
using Scenario.Utilities.Extensions;
using UnityEngine;
using VRF.Scenario.Systems;

namespace VRF.Scenario.Installers
{
    public class TimerSystemInstaller : BaseSystemInstaller, IDebugParam
    {
        [SerializeField] private bool debug;
        
        public override void InstallBindings()
        {
            Container.BindScenarioSystem<TimerSystem>(GetResolver()).WithArguments((IDebugParam)this);
        }
        
        public bool Debug => debug;
    }
}