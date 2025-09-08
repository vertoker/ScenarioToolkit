using System;

namespace ScenarioToolkit.Bus
{
    internal class SignalSubscription : IDisposable
    {
        public Action<object> Callback;
        public SignalDeclaration Declaration;

        public void Dispose()
        {
            Declaration.Remove(Callback);
        }
    }
}