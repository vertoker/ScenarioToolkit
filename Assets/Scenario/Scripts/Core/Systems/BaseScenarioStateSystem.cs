using Scenario.Core.Systems.States;
using Zenject;

namespace Scenario.Core.Systems
{
    /// <summary>
    /// Расширение базовой системы, которое предоставляет функционал для его синхронизации по сети
    /// </summary>
    /// <typeparam name="TState">Состояние, которое синхронизируется по сети</typeparam>
    public abstract class BaseScenarioStateSystem<TState> : BaseScenarioSystem, IScenarioStateProvider
        where TState : IState, new()
    {
        protected TState State;
        
        protected BaseScenarioStateSystem(SignalBus bus) : base(bus)
        {
            State = new TState();
        }

        public IState GetState() => State;
        
        public virtual void SetState(IState state)
        {
            State = (TState)state;
            
            ApplyState(State);
        }
        protected abstract void ApplyState(TState state);
    }
}