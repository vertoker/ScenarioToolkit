using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace ScenarioToolkit.External.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SetScenarioModuleSystem : BaseScenarioSystem
    {
        // private readonly ScenarioModuleLoader moduleLoader;

        public SetScenarioModuleSystem(SignalBus listener) : base(listener)
        {
            listener.Subscribe<LoadModule>(LoadModule);
        }

        private void LoadModule(LoadModule component)
        {
            if (AssertLog.NotNull<LoadModule>(component.Module, nameof(component.Module))) return;
            
            // var parameters = new ScenarioModuleLoader.ModeParameters
            // {
            //     ForceReloadScene = component.ForceReloadScene,
            //     StopCurrentScenario = component.StopCurrentScenario,
            // };
            // moduleLoader.Load(component.Module, parameters);
        }
    }
}