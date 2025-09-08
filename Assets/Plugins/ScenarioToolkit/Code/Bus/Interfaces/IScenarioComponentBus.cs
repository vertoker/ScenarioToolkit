using System;

namespace ScenarioToolkit.Bus.Interfaces
{
    public interface IScenarioComponentBus
    {
        public void Fire<TSignal>(TSignal signal);
        public void Fire<TSignal>();
        public void Fire(object signal);
        public void Fire(Type signalType);

        public bool IsSignalDeclared<TSignal>();
        public bool IsSignalDeclared(Type signalType);
        
        public bool TryFire<TSignal>(TSignal signal);
        public bool TryFire<TSignal>();
        public bool TryFire(object signal);
        public bool TryFire(Type signalType);

        public void Subscribe<TSignal>(Action callback);
        public void Subscribe<TSignal>(Action<TSignal> callback);
        public void Subscribe(Type signalType, Action callback);
        public void Subscribe(Type signalType, Action<object> callback);
        
        public void Unsubscribe<TSignal>(Action callback);
        public void Unsubscribe<TSignal>(Action<TSignal> callback);
        public void Unsubscribe(Type signalType, Action callback);
        public void Unsubscribe(Type signalType, Action<object> callback);
        
        public bool TryUnsubscribe<TSignal>(Action callback);
        public bool TryUnsubscribe<TSignal>(Action<TSignal> callback);
        public bool TryUnsubscribe(Type signalType, Action callback);
        public bool TryUnsubscribe(Type signalType, Action<object> callback);

        public void DeclareSignal<T>();
        public void DeclareSignal(Type signalType);
    }
}