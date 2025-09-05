using System;
using System.Collections.Generic;
using ModestTree;
using Scenario.Base.Components.Actions;
using Scenario.Core.Systems;
using Scenario.Core.Systems.States;
using Scenario.States;
using Scenario.Utilities;
using UnityEngine;
using Zenject;

namespace Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class CallMethodSystem : BaseScenarioStateSystem<CallMethodState>
    {
        private readonly Dictionary<string, Type> types; 
        
        public CallMethodSystem(SignalBus bus) : base(bus)
        {
            types = new Dictionary<string, Type>(Reflection.AllTypes.Length);
            foreach (var type in Reflection.AllTypes)
                types.TryAdd(type.Name, type);
            
            Bus.Subscribe<CallMonoMethod>(CallMonoMethod);
            Bus.Subscribe<CallStaticMethod>(CallStaticMethod);
        }
        
        public override void SetState(IState state) { ApplyState(State); }
        protected override void ApplyState(CallMethodState state)
        {
            foreach (var monoMethod in state.MonoMethods)
                CallMonoMethod(monoMethod);
            foreach (var staticMethod in state.StaticMethods)
                CallStaticMethod(staticMethod);
        }

        private void CallMonoMethod(CallMonoMethod component)
        {
            if (AssertLog.NotNull<CallMonoMethod>(component.MonoBehaviour, nameof(component.MonoBehaviour))) return;
            if (AssertLog.NotEmpty<CallMonoMethod>(component.MethodName, nameof(component.MethodName))) return;
            var monoType = component.MonoBehaviour.GetType();
            
            State.MonoMethods.Add(component);
            
            try
            {
                var method = monoType.GetMethod(component.MethodName);
                if (method == null) return;
                method.Invoke(component.MonoBehaviour, parameters: null);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while invoke method {component.MethodName} in " +
                               $"{monoType.PrettyName()}, exception={e}");
            }
        }
        private void CallStaticMethod(CallStaticMethod component)
        {
            if (AssertLog.NotEmpty<CallStaticMethod>(component.ClassName, nameof(component.ClassName))) return;
            if (AssertLog.NotEmpty<CallStaticMethod>(component.MethodName, nameof(component.MethodName))) return;
            if (!types.TryGetValue(component.ClassName, out var staticType)) return;
            
            State.StaticMethods.Add(component);
            
            try
            {
                var method = staticType.GetMethod(component.MethodName);
                if (method == null) return;
                method.Invoke(null, parameters: null);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while invoke static method {component.MethodName} " +
                               $"in {staticType.PrettyName()}, exception={e}");
            }
        }
    }
}