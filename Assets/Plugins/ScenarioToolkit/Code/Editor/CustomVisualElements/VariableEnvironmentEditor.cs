using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Editor.Utilities.Providers;
using ScenarioToolkit.Shared;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.CustomVisualElements
{
    /// <summary>
    /// Редактор для среды переменных, работает с любым IVariableEnvironment.
    /// </summary>
    public class VariableEnvironmentEditor
    {
        public event Action DataUpdated;

        private readonly IVariableEnvironment data;

        private readonly ScrollView variablesView;
        public readonly Button AddVariableButton;
        public readonly Button ImportVariablesButton;
        private readonly VisualTreeAsset variableAsset;
        private readonly List<VariableElement> variables;

        public VariableEnvironmentEditor(TemplateContainer root, IVariableEnvironment data)
        {
            this.data = data;
            
            variableAsset = UxmlEditorProvider.instance.FieldsVariable;
            var variableGroup = UxmlEditorProvider.instance.ListsVariables.Instantiate();
            root.Add(variableGroup);
            
            variablesView = variableGroup.Q<ScrollView>("variables");
            AddVariableButton = variableGroup.Q<Button>("add-variable");
            ImportVariablesButton = variableGroup.Q<Button>("import-variables");
            variables = new List<VariableElement>();
            AddVariableButton.clicked += AddNewVariable;
        }
        public void Load()
        {
            foreach (var variableBind in data.Variables)
                AddVariable(variableBind.Key, variableBind.Value);
            DataUpdated?.Invoke();
        }
        public void AddNewVariable()
        {
            var newKey = CryptoUtility.GetRandomString();
            AddVariable(newKey, TypesReflection.FallbackTypedValue, true);
            UpdateAllVariables();
        }
        
        public void AddVariable(string initKey, ObjectTyped initTypedValue, bool isNew = false)
        {
            var variableElement = new VariableElement(initKey, initTypedValue, variableAsset, Remove, MoveUp, MoveDown);
            
            variables.Add(variableElement);
            if (isNew) data.Variables.TryAdd(initKey, initTypedValue);
            variablesView.contentContainer.Add(variableElement.Element);
            
            variableElement.KeyUpdated += KeyUpdated;
            variableElement.ValueUpdated += ValueUpdated;
            return;

            void KeyUpdated(string oldKey, string newKey)
            {
                UpdateAllVariables();
            }
            void ValueUpdated(string currentKey, ObjectTyped currentValue)
            {
                data.Variables[currentKey] = currentValue;
                DataUpdated?.Invoke();
            }
        }
        public void UpdateAllVariables()
        {
            data.Variables.Clear();
            foreach (var variable in variables)
            {
                if (string.IsNullOrWhiteSpace(variable.Key)) continue;
                /*var condition = !*/data.Variables.TryAdd(variable.Key, variable.TypedValue);
                
                //if (condition) Debug.LogError($"Can't add variable with key={variable.Key} and value={variable.Value}, ignore");
            }
            DataUpdated?.Invoke();
        }
        
        public void Remove(string key)
        {
            var variable = variables.FirstOrDefault(v => v.Key == key);
            if (variable != null) Remove(variable);
        }
        public void Remove(VariableElement element)
        {
            variables.Remove(element);
            data.Variables.Remove(element.Key);
            variablesView.contentContainer.Remove(element.Element);
            element.Clear();
            DataUpdated?.Invoke();
        }
        public void MoveUp(VariableElement element) => Move(element, -1);
        public void MoveDown(VariableElement element) => Move(element, +1);
        private void Move(VariableElement element, int delta)
        {
            var lastIndex = variables.IndexOf(element);
            var nextIndex = Mathf.Clamp(lastIndex + delta, 0, variables.Count - 1);
            
            //Debug.Log($"{lastIndex} - {nextIndex}");
            if (lastIndex == nextIndex) return;
            // lastIndex < nextIndex (must be always)
            if (lastIndex > nextIndex)
                (lastIndex, nextIndex) = (nextIndex, lastIndex);
            
            variablesView.contentContainer.RemoveAt(nextIndex);
            variablesView.contentContainer.RemoveAt(lastIndex);
            variablesView.contentContainer.Insert(lastIndex, variables[nextIndex].Element);
            variablesView.contentContainer.Insert(nextIndex, variables[lastIndex].Element);
            
            (variables[lastIndex], variables[nextIndex]) = (variables[nextIndex], variables[lastIndex]);
            
            DataUpdated?.Invoke();
        }

        public bool Contains(string key)
        {
            return data.Variables.ContainsKey(key);
        }
    }
}