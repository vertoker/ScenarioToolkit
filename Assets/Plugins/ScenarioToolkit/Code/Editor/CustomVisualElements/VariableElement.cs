using System;
using JetBrains.Annotations;
using Scenario.Core.Model;
using ScenarioToolkit.Editor.Content.Fields;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.CustomVisualElements
{
    /// <summary>
    /// Редактор для одной конкретной переменной
    /// </summary>
    public class VariableElement
    {
        public VisualElement Element { get; private set; }
        private VisualElement ElementValue { get; set; }
        
        public string Key
        {
            get => currentKey;
            private set
            {
                var oldKey = currentKey;
                currentKey = value;
                KeyUpdated?.Invoke(oldKey, currentKey);
            }
        }
        public ObjectTyped TypedValue
        {
            get => ObjectTyped.ConstructNull(currentValue, currentType);
            private set
            {
                currentValue = value.Object;
                currentType = value.Type;
                ValueUpdated?.Invoke(currentKey, value);
            }
        }

        private string currentKey;
        private object currentValue;
        private Type currentType;
        
        private bool typeDerived;
        
        /// <summary> oldKey, currentKey </summary>
        public event Action<string, string> KeyUpdated;
        /// <summary> currentKey, currentValue </summary>
        public event Action<string, ObjectTyped> ValueUpdated;
        
        private readonly VisualElement variableValueParent;
        public VariableElement(string initKey, ObjectTyped initTypedValue, VisualTreeAsset variableAsset,
            [CanBeNull] Action<VariableElement> removeAction = null,
            [CanBeNull] Action<VariableElement> upAction = null,
            [CanBeNull] Action<VariableElement> downAction = null)
        {
            Element = variableAsset.Instantiate();
            var row1 = Element.Q<VisualElement>("row-1");
            var row2 = Element.Q<VisualElement>("row-2");
            
            var variableKeyField = row1.Q<TextField>("variable-key");
            var variableTypeField = row2.Q<EnumField>("value-type");
            variableValueParent = row2.Q<VisualElement>("variable-value");
            
            var variableFunctions = row1.Q<VisualElement>("variable-functions");

            if (removeAction != null)
            {
                var remove = variableFunctions.Q<Button>("remove");
                remove.clicked += () => removeAction.Invoke(this);
            }
            if (upAction != null)
            {
                var up = variableFunctions.Q<Button>("up");
                up.clicked += () => upAction.Invoke(this);
            }
            if (downAction != null)
            {
                var down = variableFunctions.Q<Button>("down");
                down.clicked += () => downAction.Invoke(this);
            }
            
            // string by default
            var initSelectedType = ScenarioFields.GetFieldEnum(initTypedValue.Type);
            Key = initKey;
            TypedValue = initTypedValue;
            
            variableKeyField.value = initKey;
            variableTypeField.Init(initSelectedType);
            
            variableKeyField.RegisterCallback<ChangeEvent<string>>(evt => KeyChangedCallback(evt.newValue));
            variableTypeField.RegisterCallback<ChangeEvent<Enum>>(evt => UpdateFieldType((ScenarioFieldType)evt.newValue));
            
            KeyChangedCallback(initKey);
            InitFieldType(initSelectedType, initTypedValue);
            //Debug.Log($"Added variable {key} - {value}");
        }
        public void Clear()
        {
            variableValueParent.Clear();
        }

        private void InitFieldType(ScenarioFieldType initSelectedType, ObjectTyped initTypedValue)
        {
            variableValueParent.Clear();
            
            var fieldType = ScenarioFields.GetType(initSelectedType);
            var fieldCreator = ScenarioFields.GetCreator(fieldType);
            
            ElementValue = fieldCreator.CreateField(initTypedValue.Type, initTypedValue.Object, ValueChangedCallback);
            ValueChangedCallback(initTypedValue.Object);
            
            variableValueParent.Add(ElementValue);
            return;

            void ValueChangedCallback(object newValue) => ChangeValue(newValue, fieldType);
        }
        private void UpdateFieldType(ScenarioFieldType newSelectedType)
        {
            variableValueParent.Clear();

            var fieldValueType = ScenarioFields.GetType(newSelectedType);
            var fieldCreator = ScenarioFields.GetCreator(fieldValueType);
            var defaultValue = fieldCreator.GetDefaultValue();
            typeDerived = false;
            
            ElementValue = fieldCreator.CreateField(fieldValueType, defaultValue, ValueChangedCallback);
            ValueChangedCallback(defaultValue);
            
            variableValueParent.Add(ElementValue);
            return;

            void ValueChangedCallback(object newValue) => ChangeValue(newValue, fieldValueType);
        }

        void ChangeValue(object newValue, Type fieldType)
        {
            // Механизм по сохранению дочернего типа для базового поля
            if (typeDerived) // Тип уже дочерний
            {
                if (newValue != null) // Если назначено новое не null значение
                    currentType = newValue.GetType(); // То тип надо сменить
                // В ином случае это null и тип сохраняется
            }
            else
            {
                if (newValue != null) // Если назначено первое не null значение
                {
                    typeDerived = true; // Тип обновился
                    currentType = newValue.GetType(); // И его надо обновить
                }
                else currentType = fieldType; // Иначе он останется по умолчанию
            }
                
            //Debug.Log(newValue);
            //Debug.Log(ScenarioTypeParser.Serialize(currentType));
            TypedValueChangedCallback(ObjectTyped.ConstructNull(newValue, currentType));
        }
        
        private void KeyChangedCallback(string value) => Key = value;
        private void TypedValueChangedCallback(ObjectTyped value) => TypedValue = value;
    }
}