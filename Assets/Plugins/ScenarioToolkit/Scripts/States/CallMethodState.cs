using System.Collections.Generic;
using Scenario.Base.Components.Actions;
using Scenario.Core.Systems.States;

namespace Scenario.States
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