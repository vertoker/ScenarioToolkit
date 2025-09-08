using System;
using System.Collections.Generic;
using ScenarioToolkit.Bus.Interfaces;
using ScenarioToolkit.Shared.Exceptions;
using UnityEngine.Pool;

namespace ScenarioToolkit.Bus
{
    public class ScenarioComponentBus : IScenarioComponentBus
    {
        private readonly Dictionary<Type, SignalDeclaration> declarationMap = new();
        private readonly Dictionary<SignalSubscriptionId, SignalSubscription> subscriptionMap = new();
        
        private readonly ObjectPool<SignalDeclaration> declarationPool;
        private readonly ObjectPool<SignalSubscription> subscriptionPool;

        public ScenarioComponentBus(int declarationCapacity = 100, int subscriptionCapacity = 200, bool poolCheck = true)
        {
            declarationPool = SignalFactory.CreateDeclarationPool(poolCheck, declarationCapacity);
            subscriptionPool = SignalFactory.CreateSubscriptionPool(poolCheck, subscriptionCapacity);
        }

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

        public void Unsubscribe<TSignal>(Action callback)
        {
            InternalUnsubscribe(new SignalSubscriptionId(typeof(TSignal), callback), true);
        }
        public void Unsubscribe<TSignal>(Action<TSignal> callback)
        {
            InternalUnsubscribe(new SignalSubscriptionId(typeof(TSignal), callback), true);
        }
        public void Unsubscribe(Type signalType, Action callback)
        {
            InternalUnsubscribe(new SignalSubscriptionId(signalType, callback), true);
        }
        public void Unsubscribe(Type signalType, Action<object> callback)
        {
            InternalUnsubscribe(new SignalSubscriptionId(signalType, callback), true);
        }
        
        public bool TryUnsubscribe<TSignal>(Action callback)
        {
            return InternalUnsubscribe(new SignalSubscriptionId(typeof(TSignal), callback), false);
        }
        public bool TryUnsubscribe<TSignal>(Action<TSignal> callback)
        {
            return InternalUnsubscribe(new SignalSubscriptionId(typeof(TSignal), callback), false);
        }
        public bool TryUnsubscribe(Type signalType, Action callback)
        {
            return InternalUnsubscribe(new SignalSubscriptionId(signalType, callback), false);
        }
        public bool TryUnsubscribe(Type signalType, Action<object> callback)
        {
            return InternalUnsubscribe(new SignalSubscriptionId(signalType, callback), false);
        }
        public void DeclareSignal<T>()
        {
            DeclareSignal(typeof(T));
        }
        public void DeclareSignal(Type signalType)
        {
            var declaration = declarationPool.Get();
            
            declarationMap.Add(signalType, declaration);
        }
        
        private bool InternalFire(Type signalType, object signal, bool requireDeclaration)
        {
            var declaration = GetDeclaration(signalType);

            if (declaration == null)
            {
                if (requireDeclaration)
                {
                    throw Assert.CreateException($"Fired undeclared signal '{signalType}'!");
                }
                return false;
            }

            signal ??= Activator.CreateInstance(signalType);
            declaration.Fire(signal);
            
            return true;
        }
        
        private void InternalSubscribe(SignalSubscriptionId id, Action<object> callback)
        {
            Assert.That(!declarationMap.ContainsKey(id.SignalType),
                "Tried subscribing to the same signal with the same callback on Zenject.SignalBus");
            
            var declaration = GetDeclaration(id.SignalType);
            
            if (declaration == null)
            {
                throw Assert.CreateException($"Tried subscribing to undeclared signal '{id.SignalType}'!");
            }
            
            var subscription = subscriptionPool.Get();
            subscription.Callback = callback;
            subscription.Declaration = declaration;
            
            declaration.Add(callback);
            subscriptionMap.Add(id, subscription);
        }
        
        private bool InternalUnsubscribe(SignalSubscriptionId id, bool throwIfMissing)
        {
            if (subscriptionMap.Remove(id, out var subscription))
            {
                subscription.Dispose();
                subscriptionPool.Release(subscription);
                return true;
            }
            
            if (throwIfMissing)
            {
                throw Assert.CreateException($"Called unsubscribe for signal '{id.SignalType}' " +
                                             $"but could not find corresponding subscribe.  " +
                                             "If this is intentional, call TryUnsubscribe instead.");
            }
            return false;
        }

        private SignalDeclaration GetDeclaration(Type signalType)
        {
            return declarationMap.GetValueOrDefault(signalType);
        }
    }
}