using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.Scenario.Components.Actions;
using VRF.Scenes.Project;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SceneManageSystem : BaseScenarioSystem
    {
        private readonly ScenesService scenesService;

        public SceneManageSystem(SignalBus listener, ScenesService scenesService) : base(listener)
        {
            this.scenesService = scenesService;
            Bus.Subscribe<LoadScene>(LoadScene);
        }

        private void LoadScene(LoadScene component)
        {
            if (AssertLog.NotEmpty<LoadScene>(component.Scene, nameof(component.Scene))) return;
            
            scenesService.LoadScene(component.Scene);
        }
    }
}