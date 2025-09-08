using System;
using System.Collections.Generic;

namespace ScenarioToolkit.Bus
{
    internal class SignalDeclaration
    {
        private readonly List<Action<object>> callbacks;

        public SignalDeclaration(int capacity = 0)
        {
            callbacks = new List<Action<object>>(capacity);
        }

        public void Add(Action<object> callback)
        {
            callbacks.Add(callback);
        }
        public void Remove(Action<object> callback)
        {
            callbacks.Remove(callback);
        }

        public void Fire(object signal)
        {
            foreach (var callback in callbacks)
                callback.Invoke(signal);
        }
    }
}