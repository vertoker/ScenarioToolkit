using System;
using System.Collections.Generic;

namespace VRF.DataSources.Disposing
{
    public class DataSourceDisposeService : IDisposable
    {
        private readonly List<Action> actions = new();

        public void Add(Action action)
        {
            actions.Add(action);
        }
        public void Dispose()
        {
            foreach (var action in actions)
                action.Invoke();
            actions.Clear();
        }
    }
}