using Cysharp.Threading.Tasks;
using Mirror;
using ScenarioToolkit.Core.Systems.States;
using Zenject;

namespace ScenarioToolkit.Core.Systems
{
    /// <summary>
    /// Расширение базовой системы, которое предоставляет функционал для его синхронизации по сети
    /// и заточено специально под синхронизацию временных значений
    /// </summary>
    /// <typeparam name="TState">Состояние, которое синхронизируется по сети</typeparam>
    public abstract class BaseScenarioAsyncStateSystem<TState> : BaseScenarioStateSystem<TState> 
        where TState : IState, new()
    {
        protected BaseScenarioAsyncStateSystem(SignalBus bus) : base(bus) { }

        public override async void SetState(IState state)
        {
            State = (TState)state;
            
            await UniTask.WaitWhile(() => NetworkTime.time == 0);
            
            ApplyState(State);
        }
    }
}