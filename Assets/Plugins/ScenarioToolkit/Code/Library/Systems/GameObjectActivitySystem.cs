using Scenario.Base.Components.Actions;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    /// <summary>
    /// State система для контроля активности GameObject скриптов
    /// </summary>
    public class GameObjectActivitySystem : BaseScenarioSystem
    {
        public GameObjectActivitySystem(ScenarioComponentBus bus) : base(bus)
        {
            Bus.Subscribe<SetGameObjectActivity>(SetGameObjectActivity);
        }

        private void SetGameObjectActivity(SetGameObjectActivity component)
        {
            if (AssertLog.NotNull<SetGameObjectActivity>(component.GameObject, nameof(component.GameObject))) return;
            
            component.GameObject.SetActive(component.IsActive);
        }
    }
}