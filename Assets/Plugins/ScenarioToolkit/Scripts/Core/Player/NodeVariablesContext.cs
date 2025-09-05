using System;
using System.Collections.Generic;
using System.Reflection;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;
using Scenario.Utilities.Extensions;
using ZLinq;
using ZLinq.Linq;

namespace Scenario.Core.Player
{
    /// <summary> Варианты переменных сред для операции над ними </summary>
    public enum VariableEnvironmentType
    {
        /// <summary> Local - только для чтения, локальные переменные среды </summary>
        LVE = 0,
        /// <summary> Nested - результат смешения при начале сценария, тут хранятся финальные переменные </summary>
        NVE = 1,
    }
    
    public readonly struct NodeVariablesContext
    {
        // Все данные должны быть private, потому что все данные используются вместе
        // Все необходимые функции должны быть тут, а сам объект должен передаваться как единица данных
        
        /// <summary> LVE - только для чтения, локальные переменные среды </summary>
        private Dictionary<string, ObjectTyped> LocalVariableEnvironment { get; }
        /// <summary> NVE - результат смешения при начале сценария, тут хранятся финальные переменные </summary>
        private Dictionary<string, ObjectTyped> NestedVariableEnvironment { get; } 
        
        // Все места, где нужно перезаписать значения на переменные
        private readonly Dictionary<int, List<IComponentVariables>> nodeOverrides;
        
        public NodeVariablesContext(IScenarioContext context)
        {
            nodeOverrides = context.NodeOverrides;
            LocalVariableEnvironment = context.Variables; // copy LVE
            NestedVariableEnvironment = new Dictionary<string, ObjectTyped>(3); // create new NVE
        }
        
        public void MixVariables(IVariableEnvironment variableEnvironment)
        { NestedVariableEnvironment.MixVariables(variableEnvironment.Variables); }
        public void MixVariables(NodeVariablesContext context)
        { NestedVariableEnvironment.MixVariables(context.NestedVariableEnvironment); }

        public bool TryGetValue(string variableName, out ObjectTyped objectTyped)
        {
            // Сначала проверяется NVE, а потом LVE
            return NestedVariableEnvironment.TryGetValue(variableName, out objectTyped) 
                   || LocalVariableEnvironment.TryGetValue(variableName, out objectTyped);
        }
        public bool TryGetValue(string variableName, VariableEnvironmentType environmentType, out ObjectTyped objectTyped)
        {
            return environmentType switch
            {
                VariableEnvironmentType.LVE => NestedVariableEnvironment.TryGetValue(variableName, out objectTyped),
                VariableEnvironmentType.NVE => LocalVariableEnvironment.TryGetValue(variableName, out objectTyped),
                _ => throw new ArgumentOutOfRangeException(nameof(environmentType), environmentType, null)
            };
        }
        public ObjectTyped GetValue(string variableName)
        {
            if (!TryGetValue(variableName, out var objectTyped))
                throw new KeyNotFoundException($"The given key '{variableName}' was not present in the dictionary.");
            return objectTyped;
        }
        public ObjectTyped GetValue(string variableName, VariableEnvironmentType environmentType)
        {
            if (!TryGetValue(variableName, environmentType, out var objectTyped))
                throw new KeyNotFoundException($"The given key '{variableName}' was not present in the dictionary.");
            return objectTyped;
        }
        
        public Dictionary<string, ObjectTyped> GetAll()
        {
            var length = LocalVariableEnvironment.Count + NestedVariableEnvironment.Count;
            var mergedVariables = new Dictionary<string, ObjectTyped>(length);
            
            foreach (var lveBind in LocalVariableEnvironment)
                mergedVariables.Add(lveBind.Key, lveBind.Value);
            foreach (var lveBind in NestedVariableEnvironment)
                mergedVariables.TryAdd(lveBind.Key, lveBind.Value);
            return mergedVariables;
        }

        public void Insert(string variableName, ObjectTyped value, VariableEnvironmentType environmentType)
        {
            switch (environmentType)
            {
                case VariableEnvironmentType.LVE: InsertLVE(variableName, value); break;
                case VariableEnvironmentType.NVE: InsertNVE(variableName, value); break;
                default: throw new ArgumentOutOfRangeException(nameof(environmentType), environmentType, null);
            }
        }
        public bool Remove(string variableName, VariableEnvironmentType environmentType)
        {
            return environmentType switch
            {
                VariableEnvironmentType.LVE => RemoveLVE(variableName),
                VariableEnvironmentType.NVE => RemoveNVE(variableName),
                _ => throw new ArgumentOutOfRangeException(nameof(environmentType), environmentType, null)
            };
        }
        
        public void InsertLVE(string variableName, ObjectTyped value)
            => LocalVariableEnvironment[variableName] = value;
        public void InsertNVE(string variableName, ObjectTyped value)
            => NestedVariableEnvironment[variableName] = value;
        public bool RemoveLVE(string variableName) => LocalVariableEnvironment.Remove(variableName);
        public bool RemoveNVE(string variableName) => NestedVariableEnvironment.Remove(variableName);

        public bool IsValidContext()
        {
            return nodeOverrides != null && LocalVariableEnvironment != null && NestedVariableEnvironment != null;
        }

        public IEnumerable<TComponent> Process<TComponent>(IScenarioNodeComponents<TComponent> node)
            where TComponent : IScenarioComponent
        {
            if (nodeOverrides == null) return node.Components;
            if (nodeOverrides.Count == 0 || NestedVariableEnvironment.Count == 0 && LocalVariableEnvironment.Count == 0)
                return node.Components;
            return ProcessInternal(node);
        }
        public ValueEnumerable<FromEnumerable<TComponent>, TComponent> ProcessAVE<TComponent>(IScenarioNodeComponents<TComponent> node)
            where TComponent : IScenarioComponent
        {
            if (nodeOverrides == null) return node.ComponentsEnumerableAVE;
            if (nodeOverrides.Count == 0 || NestedVariableEnvironment.Count == 0 && LocalVariableEnvironment.Count == 0)
                return node.ComponentsEnumerableAVE;
            return ProcessInternal(node).AsValueEnumerable();
        }

        // O^3 * log(O^2)
        private IEnumerable<TComponent> ProcessInternal<TComponent>(IScenarioNodeComponents<TComponent> node)
            where TComponent : IScenarioComponent
        {
            var length = node.Components.Count;
            for (var index = 0; index < length; index++)
            {
                var component = node.Components[index];
                var fields = component.GetComponentFields();
                
                foreach (var field in fields)
                {
                    // Если нет записей о ноде
                    if (!nodeOverrides.TryGetValue(node.Hash, out var nodeOverride)) continue;
                    
                    // Если вместо переменных, стоит null
                    // (всегда гарантированно, что nodeOverride.Length = node.Components.Length)
                    var componentOverride = nodeOverride[index];
                    if (componentOverride == null) continue;
                    
                    // Если есть записи о переменной в поле
                    var memberOverride = componentOverride.GetValueOrDefault(field.Name);
                    if (memberOverride == null) continue;
                    
                    // Сначала поиск в NVE, потом в LVE
                    if (NestedVariableEnvironment.TryGetValue(memberOverride.VariableName, out var variableTyped) 
                        || LocalVariableEnvironment.TryGetValue(memberOverride.VariableName, out variableTyped))
                    {
                        // Обработка в отдельном методе для удобства
                        ProcessFieldInternal(field, variableTyped, ref component);
                    }
                }

                yield return component;
            }
        }

        private static void ProcessFieldInternal<TComponent>(FieldInfo field, ObjectTyped variableTyped, 
            ref TComponent component) where TComponent : IScenarioComponent
        {
            // Переменные
            
            var fieldValue = field.GetValue(component);
            Type fieldType; var isObjectTyped = false;
            // Специальная проверка на ObjectTyped как переменную
            if (fieldValue is ObjectTyped fieldTyped)
            {
                fieldType = fieldTyped.Type;
                isObjectTyped = true;
            }
            else fieldType = field.FieldType;
            
            var fieldIsValueType = fieldType.IsValueType;
            var variableIsValueType = variableTyped.Type.IsValueType;
            
            // Логика
            
            if (fieldIsValueType == variableIsValueType) // оба struct или оба class
            {
                if (fieldIsValueType) // оба struct
                {
                    // Значит наследования нет и можно просто сравнить типы
                    if (field.FieldType == variableTyped.Type)
                        field.SetValue(component, isObjectTyped ? variableTyped : variableTyped.Object);
                }
                else // оба class
                {
                    var variableValue = isObjectTyped ? variableTyped : variableTyped.Object;
                    
                    // Переменная может иметь тип Derived от типа field'а, наоборот - невозможно
                    if (field.FieldType.IsAssignableFrom(variableTyped.Type))
                        field.SetValue(component, variableValue);
                    
                    // На случай несовпадения неявных типов, но при этом явной попытки обнуления
                    else if (variableValue == null)
                        field.SetValue(component, null);
                    
                    // Эта проверка на null нарушает проверку совпадения типов, но справедливости ради
                    // На момент написания есть только 2 класса: string и UObject и они между собой назначаемы
                    // TODO когда появится больше классов - можно этот баг и исправить
                }
            }
        }
    }
}