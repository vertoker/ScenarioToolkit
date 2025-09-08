using Cysharp.Threading.Tasks;
using Scenario.Base.Components.Actions;
using Scenario.Base.Components.Conditions;
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
    /// State-timer система для запуска ожидания (только на host работает)
    /// </summary>
    public class DelayedTriggerSystem : BaseScenarioSystem
    {
        public DelayedTriggerSystem(ScenarioComponentBus listener) : base(listener)
        {
            listener.Subscribe<StartDelayTrigger>(StartDelayTrigger);
        }

        private void StartDelayTrigger(StartDelayTrigger component)
        {
            if (AssertLog.NotEmpty<StartDelayTrigger>(component.Name, nameof(component.Name))) return;
            if (AssertLog.Above<StartDelayTrigger>(component.Seconds, 0, nameof(component.Seconds))) return;
            
            WaitDelayTrigger(component.Name, component.Seconds);
        }

        private async void WaitDelayTrigger(string name, float seconds)
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            await UniTask.Delay((int)(seconds * 1000));
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            
            StopDelayTrigger(name);
        }
        
        private void StopDelayTrigger(string name)
        {
            Bus.Fire(new DelayTriggerEnded { Name = name });
        }
    }
}