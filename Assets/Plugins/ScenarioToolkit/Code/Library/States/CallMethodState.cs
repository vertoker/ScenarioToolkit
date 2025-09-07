using System.Collections.Generic;
using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems.States;

namespace ScenarioToolkit.Library.States
{
    public class CallMethodState : IState
    {
        public List<CallMonoMethod> MonoMethods = new();
        public List<CallStaticMethod> StaticMethods = new();
        
        public void Clear()
        {
            MonoMethods.Clear();
            MonoMethods.Clear();
        }
    }
}