using System;
using System.Collections.Generic;
using ScenarioToolkit.Bus.Interfaces;

namespace ScenarioToolkit.Bus
{
    public class ScenarioSignalBus : IScenarioSignalBus
    {
        private readonly Dictionary<SignalSubscriptionId, ScenarioSignalDeclaration> declarationMap = new();
        
        public void Fire<TSignal>(TSignal signal)
        {
            InternalFire(typeof(TSignal), signal, true);
        }
        public void Fire<TSignal>()
        {
            InternalFire(typeof(TSignal), null, true);
        }
        public void Fire(object signal)
        {
            InternalFire(signal.GetType(), signal, true);
        }
        public void Fire(Type signalType)
        {
            InternalFire(signalType, null, true);
        }
        
        public bool IsSignalDeclared<TSignal>()
        {
            return IsSignalDeclared(typeof(TSignal));
        }
        public bool IsSignalDeclared(Type signalType)
        {
            return GetDeclaration(signalType) != null;
        }
        
        public bool TryFire<TSignal>(TSignal signal)
        {
            return InternalFire(typeof(TSignal), signal, true);
        }
        public bool TryFire<TSignal>()
        {
            return InternalFire(typeof(TSignal), null, true);
        }
        public bool TryFire(object signal)
        {
            return InternalFire(signal.GetType(), signal, true);
        }
        public bool TryFire(Type signalType)
        {
            return InternalFire(signalType, null, true);
        }

        private bool InternalFire(Type signalType, object signal, bool requireDeclaration)
        {
            var declaration = GetDeclaration(signalType);

            if (declaration == null)
            {
                if (requireDeclaration)
                {
                    throw Assert.CreateException("Fired undeclared signal '{0}'!", signalType);
                }
                return false;
            }

            signal ??= Activator.CreateInstance(signalType);
            declaration.Fire(signal);
            
            return true;
        }
        
        public void Subscribe<TSignal>(Action callback)
        {
            InternalSubscribe(new SignalSubscriptionId(typeof(TSignal), callback), WrapperCallback);
            return;
            void WrapperCallback(object args) => callback();
        }
        public void Subscribe<TSignal>(Action<TSignal> callback)
        {
            InternalSubscribe(new SignalSubscriptionId(typeof(TSignal), callback), WrapperCallback);
            return;
            void WrapperCallback(object args) => callback((TSignal)args);
        }
        public void Subscribe(Type signalType, Action callback)
        {
            InternalSubscribe(new SignalSubscriptionId(signalType, callback), WrapperCallback);
            return;
            void WrapperCallback(object args) => callback();
        }
        public void Subscribe(Type signalType, Action<object> callback)
        {
            InternalSubscribe(new SignalSubscriptionId(signalType, callback), callback);
        }

        private void InternalSubscribe(SignalSubscriptionId id, Action<object> callback)
        {
            Assert.That(!declarationMap.ContainsKey(id),
                "Tried subscribing to the same signal with the same callback on Zenject.SignalBus");
            
            var declaration = GetDeclaration(id.SignalType);
            
            if (declaration == null)
            {
                throw Assert.CreateException("Tried subscribing to undeclared signal '{0}'!", id.SignalType);
            }
            
            declaration.Add(callback);
            declarationMap.Add(id, declaration);
        }

        public void Unsubscribe<TSignal>(Action callback)
        {
            
        }
        public void Unsubscribe<TSignal>(Action<TSignal> callback)
        {
            
        }
        public void Unsubscribe(Type signalType, Action callback)
        {
            
        }
        public void Unsubscribe(Type signalType, Action<object> callback)
        {
            
        }
        
        // TODO закончить и сделать свой аналог из Zenject ScenarioException и Assert с возможностью LogError

        private void InternalUnsubscribe(SignalSubscriptionId id)
        {
            if (declarationMap.Remove(id, out var subscription))
            {
                subscription.Remove(id.Token);
            }
            else
            {
                throw Assert.CreateException(
                    "Called unsubscribe for signal '{0}' but could not find corresponding subscribe.  " +
                    "If this is intentional, call TryUnsubscribe instead.", id.SignalType);
            }
        }
        
        public void DeclareSignal<T>()
        {
            throw new NotImplementedException();
        }
        public void DeclareSignal(Type signalType)
        {
            throw new NotImplementedException();
        }

        private ScenarioSignalDeclaration GetDeclaration(Type signalType)
        {
            ScenarioSignalDeclaration handler;

            if (declarationMap.TryGetValue(signalType, out handler))
            {
                return handler;
            }

            return null;
        }
    }
}