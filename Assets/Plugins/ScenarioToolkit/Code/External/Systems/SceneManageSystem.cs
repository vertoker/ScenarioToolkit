using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using UnityEngine.SceneManagement;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace ScenarioToolkit.External.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SceneManageSystem : BaseScenarioSystem
    {
        public SceneManageSystem(SignalBus listener) : base(listener)
        {
            Bus.Subscribe<LoadScene>(LoadScene);
        }

        private void LoadScene(LoadScene component)
        {
            if (AssertLog.NotEmpty<LoadScene>(component.Scene, nameof(component.Scene))) return;
            
            SceneManager.LoadScene(component.Scene);
        }
    }
}