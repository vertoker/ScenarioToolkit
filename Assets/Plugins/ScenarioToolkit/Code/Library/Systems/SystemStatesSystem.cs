using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SystemStatesSystem : BaseScenarioSystem
    {
        public SystemStatesSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<ClearSystemStates>(ResetSystemStates);
        }
        
        private void ResetSystemStates(ClearSystemStates component)
        {
            
        }
    }
}