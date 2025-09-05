using Scenario.Base.Components.Conditions;
using Scenario.Core.Systems;
using Scenario.Utilities;
using Zenject;
using System;
using Scenario.Base.Components.Actions;
using Scenario.Core.Model;
using UnityEngine;

namespace Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class VariablesReflectionSystem : BaseScenarioSystem
    {
        private const string NullContext = "Variable context is null";
        
        private static string TypeMismatchMessage(string variableName, Type type1, Type type2) 
            => $"Variable {variableName} has type mismatch: {type1.Name} != {type2.Name}";
        private static string VariableNotFound(string variableName) 
            => $"Variable {variableName} is not founded in variables";
        
        public VariablesReflectionSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<RequestVariableEquals>(RequestVariableEquals);
            bus.Subscribe<VariableInsertContext>(VariableInsertContext);
            bus.Subscribe<VariableIncrementContext>(VariableIncrementContext);
            
            bus.Subscribe<VariableRemoveContext>(VariableRemoveContext);
            bus.Subscribe<VariableNotifyContext>(VariableNotifyContext);
            bus.Subscribe<VariableNotifyAllContext>(VariableNotifyAllContext);
        }

        private void RequestVariableEquals(RequestVariableEquals component)
        {
            if (AssertLog.IsTrue(component.Variables.IsValidContext(), NullContext)) return;
            if (AssertLog.NotWhiteSpace<VariablesReflectionSystem>(component.Source.VariableName,
                    nameof(component.Source.VariableName))) return;
            
            if (!component.Variables.TryGetValue(component.Source.VariableName, out var variableObject))
            {
                //if (component.Source.UseWarnings)
                //    Debug.LogWarning(VariableNotFound(component.Source.VariableName));
                return;
            }
            if (variableObject.Type != component.Source.Value.Type)
            {
                //if (component.Source.UseWarnings)
                //    Debug.LogWarning(TypeMismatchMessage(component.Source.VariableName, 
                //        variableObject.Type, component.Source.Value.Type));
                return;
            }
            
            if (variableObject.Object == component.Source.Value.Object)
                Bus.Fire(component.Source);
        }
        private void VariableInsertContext(VariableInsertContext component)
        {
            if (AssertLog.IsTrue(component.Variables.IsValidContext(), NullContext)) return;
            if (AssertLog.NotWhiteSpace<VariablesReflectionSystem>(component.Source.VariableName,
                    nameof(component.Source.VariableName))) return;
            
            component.Variables.Insert(component.Source.VariableName, component.Source.Value, component.Source.EnvironmentType);
            
            if (component.Source.Notify)
            {
                Bus.Fire(new VariableEquals
                {
                    VariableName = component.Source.VariableName, 
                    Value = component.Variables.GetValue(component.Source.VariableName), // не может не выполниться
                });
            }
        }
        private void VariableIncrementContext(VariableIncrementContext component)
        {
            if (AssertLog.IsTrue(
                    component.Variables.IsValidContext(), NullContext
                )) return;
            
            if (AssertLog.NotWhiteSpace<VariablesReflectionSystem>(
                    component.Source.VariableName, nameof(component.Source.VariableName)
                )) return;

            if (AssertLog.IsTrue(
                    component.Variables.TryGetValue(
                        component.Source.VariableName, component.Source.EnvironmentType, out var objectTyped
                    ), VariableNotFound(component.Source.VariableName)
                )) return;
            
            if (AssertLog.IsTrue(
                    component.Source.Value.Type == objectTyped.Type, 
                    TypeMismatchMessage(component.Source.VariableName, component.Source.Value.Type,objectTyped.Type)
                )) return;

            
            if (objectTyped.Is<string>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<string>() + component.Source.Value.GetAs<string>());
            else if (objectTyped.Is<int>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<int>() + component.Source.Value.GetAs<int>());
            else if (objectTyped.Is<float>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<float>() + component.Source.Value.GetAs<float>());
            
            else if (objectTyped.Is<long>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<long>() + component.Source.Value.GetAs<long>());
            else if (objectTyped.Is<double>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<double>() + component.Source.Value.GetAs<double>());
            else if (objectTyped.Is<uint>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<uint>() + component.Source.Value.GetAs<uint>());
            else if (objectTyped.Is<ulong>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<ulong>() + component.Source.Value.GetAs<ulong>());
            
            else if (objectTyped.Is<Vector2>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<Vector2>() + component.Source.Value.GetAs<Vector2>());
            else if (objectTyped.Is<Vector3>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<Vector3>() + component.Source.Value.GetAs<Vector3>());
            else if (objectTyped.Is<Vector4>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<Vector4>() + component.Source.Value.GetAs<Vector4>());
            else if (objectTyped.Is<Vector2Int>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<Vector2Int>() + component.Source.Value.GetAs<Vector2Int>());
            else if (objectTyped.Is<Vector3Int>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<Vector3Int>() + component.Source.Value.GetAs<Vector3Int>());
            else if (objectTyped.Is<Color>()) objectTyped = ObjectTyped.ConstructNotNull(
                objectTyped.GetAs<Color>() + component.Source.Value.GetAs<Color>());
            
            if (component.Source.Notify)
            {
                Bus.Fire(new VariableEquals
                {
                    VariableName = component.Source.VariableName, 
                    Value = objectTyped, // не может не выполниться
                });
            }
        }
        
        private void VariableRemoveContext(VariableRemoveContext component)
        {
            if (AssertLog.IsTrue(component.Variables.IsValidContext(), NullContext)) return;
            if (AssertLog.NotWhiteSpace<VariablesReflectionSystem>(component.Source.VariableName,
                    nameof(component.Source.VariableName))) return;
            
            component.Variables.Remove(component.Source.VariableName, component.Source.EnvironmentType);
        }
        private void VariableNotifyContext(VariableNotifyContext component)
        {
            if (AssertLog.IsTrue(component.Variables.IsValidContext(), NullContext)) return;
            if (AssertLog.IsTrue(component.Variables.TryGetValue(component.Source.VariableName, 
                    out var variable), "Variable is not founded")) return;
            
            Bus.Fire(new VariableEquals
            {
                VariableName = component.Source.VariableName, 
                Value = variable,
            });
        }
        private void VariableNotifyAllContext(VariableNotifyAllContext component)
        {
            if (AssertLog.IsTrue(component.Variables.IsValidContext(), NullContext)) return;

            var mergedVariables = component.Variables.GetAll();
            foreach (var variable in mergedVariables)
            {
                Bus.Fire(new VariableEquals
                {
                    VariableName = variable.Key, 
                    Value = variable.Value,
                });
            }
        }
    }
}