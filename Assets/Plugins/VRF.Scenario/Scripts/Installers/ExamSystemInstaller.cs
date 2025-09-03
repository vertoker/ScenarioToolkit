using Scenario.Core.Installers.Systems;
using Scenario.Utilities.Extensions;
using UnityEngine;
using VRF.Scenario.Systems;

namespace VRF.Scenario.Installers
{
    public class ExamSystemInstaller : BaseSystemInstaller, IDebugParam
    {
        [SerializeField] private bool debug;
        
        public override void InstallBindings()
        {
            Container.BindScenarioSystem<ExamSystem>(GetResolver()).WithArguments((IDebugParam)this);
        }

        public bool Debug => debug;
    }
}