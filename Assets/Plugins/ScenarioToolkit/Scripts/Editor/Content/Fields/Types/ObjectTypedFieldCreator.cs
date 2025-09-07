using System;
using JetBrains.Annotations;
using Scenario.Core.Model;
using Scenario.Editor.Content.Fields.Base;
using Scenario.Editor.Utilities.Providers;
using UnityEngine.UIElements;

namespace Scenario.Editor.Content.Fields.Types
{
    public class ObjectTypedFieldCreator : ITypeFieldCreator
    {
        private readonly VisualTreeAsset variableAsset
            = UxmlEditorProvider.instance.FieldsObjectTyped; // VariableField

        public bool CanCreate(Type type) => typeof(ObjectTyped) == type;
        public object GetDefaultValue() => ObjectTyped.Empty;

        public VisualElement CreateField(Type valueType, object initialValue, Action<object> valueChangedCallback)
        {
            var element = variableAsset.Instantiate();
            var row = element.Q<VisualElement>("row");
            
            var variableTypeField = row.Q<EnumField>("value-type");
            var variableValueParent = row.Q<VisualElement>("variable-value");

            var initTypedValue = initialValue != null ? (ObjectTyped)initialValue : ObjectTyped.Empty;
            var initSelectedType = ScenarioFields.GetFieldEnum(valueType);
            
            var currentType = valueType;
            var typeDerived = false;
            
            variableTypeField.Init(initSelectedType);
            variableTypeField.RegisterCallback<ChangeEvent<Enum>>(evt => UpdateFieldType((ScenarioFieldType)evt.newValue));
            InitFieldType();

            return element;
            
            // TODO перенести логику из VariableElement и отсюда в единое место (разрешаю сделать класс для данных)
            
            void InitFieldType()
            {
                variableValueParent.Clear();

                var fieldType = ScenarioFields.GetType(initSelectedType);
                var fieldCreator = ScenarioFields.GetCreator(fieldType);
            
                var elementValue = fieldCreator.CreateField(valueType, initTypedValue.Object, ValueChangedCallback);
                ChangeValue(initTypedValue.Object, fieldType);
            
                variableValueParent.Add(elementValue);
                return;

                void ValueChangedCallback(object newValue) => ChangeValue(newValue, fieldType);
            }
            
            void UpdateFieldType(ScenarioFieldType newSelectedType)
            {
                variableValueParent.Clear();

                var fieldType = ScenarioFields.GetType(newSelectedType);
                var fieldCreator = ScenarioFields.GetCreator(fieldType);
                var defaultValue = fieldCreator.GetDefaultValue();
                typeDerived = false;
                
                var elementValue = fieldCreator.CreateField(valueType, defaultValue, ValueChangedCallback);
                ValueChangedCallback(defaultValue);
            
                variableValueParent.Add(elementValue);
                return;

                void ValueChangedCallback(object newValue) => ChangeValue(newValue, fieldType);
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
                valueChangedCallback?.Invoke(ObjectTyped.ConstructNull(newValue, currentType));
            }
        }
    }
}