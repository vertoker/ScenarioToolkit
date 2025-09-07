using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Core.Systems.States;
using ScenarioToolkit.Library.States;
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
    public class GameObjectActivitySystem : BaseScenarioStateSystem<GameObjectActivityState>
    {
        public GameObjectActivitySystem(SignalBus bus) : base(bus)
        {
            Bus.Subscribe<SetGameObjectActivity>(SetGameObjectActivity);
        }

        protected override void ApplyState(GameObjectActivityState state)
        {
            foreach (var (key, value) in state.GameObjects)
            {
                key.SetActive(value);
            }
        }

        private void SetGameObjectActivity(SetGameObjectActivity component)
        {
            if (AssertLog.NotNull<SetGameObjectActivity>(component.GameObject, nameof(component.GameObject))) return;
            
            State.GameObjects.SetStateActivity(component.GameObject, component.GameObject.activeSelf, component.IsActive);
            
            component.GameObject.SetActive(component.IsActive);
        }
    }
}