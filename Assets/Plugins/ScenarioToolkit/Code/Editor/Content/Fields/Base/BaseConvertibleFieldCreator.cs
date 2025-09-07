using System;
using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace Scenario.Editor.Content.Fields.Base
{
    /// <summary>
    /// Ответвление от обычного базового конструктора, которое работает с двумя типами значений
    /// и конвертирует данные для сериализации и обратно
    /// </summary>
    /// <typeparam name="TValue">Тип значения, которое вводиться в поле</typeparam>
    /// <typeparam name="TFieldValue">Тип значения, которое будет в поле</typeparam>
    /// <typeparam name="TField">Тип поля для ввода (использует TFieldValue)</typeparam>
    public abstract class BaseConvertibleFieldCreator<TValue, TFieldValue, TField> : ITypeFieldCreator
        where TField : BaseField<TFieldValue>, new()
    {
        public bool CanCreate(Type type) => typeof(TValue).IsAssignableFrom(type);
        public object GetDefaultValue() => default(TValue);
        
        public virtual VisualElement CreateField(Type valueType, object initialValue, Action<object> valueChangedCallback)
        {
            initialValue ??= GetDefaultValue();
            
            // На входе value = TValue
            var serializedValue = Serialize((TValue)initialValue);
            // А поле работает на TSerializedValue
            var field = new TField { value = serializedValue };
            
            field.RegisterCallback<ChangeEvent<TFieldValue>>(evt =>
            {
                var newValue = Deserialize(evt.newValue);
                valueChangedCallback?.Invoke(newValue);
            });
            
            return field;
        }

        protected abstract TFieldValue Serialize([CanBeNull] TValue value);
        protected abstract TValue Deserialize([CanBeNull] TFieldValue value);
    }
}