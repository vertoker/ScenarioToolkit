using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Services;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SetScenarioModuleSystem : BaseScenarioSystem
    {
        private readonly ScenarioModuleLoader moduleLoader;

        public SetScenarioModuleSystem(SignalBus listener, ScenarioModuleLoader moduleLoader) : base(listener)
        {
            this.moduleLoader = moduleLoader;
            listener.Subscribe<LoadModule>(LoadModule);
        }

        private void LoadModule(LoadModule component)
        {
            if (AssertLog.NotNull<LoadModule>(component.Module, nameof(component.Module))) return;
            
            var parameters = new ScenarioModuleLoader.ModeParameters
            {
                ForceReloadScene = component.ForceReloadScene,
                StopCurrentScenario = component.StopCurrentScenario,
            };
            moduleLoader.Load(component.Module, parameters);
        }
    }
}