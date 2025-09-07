using System;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Content.Fields.Base
{
    /// <summary>
    /// Стандартный базовый конструктор, который администрирует поле и тип значения в нём
    /// </summary>
    /// <typeparam name="TValue">Тип значения поля</typeparam>
    /// <typeparam name="TField">Тип самого поля</typeparam>
    public abstract class BaseFieldCreator<TValue, TField> : ITypeFieldCreator
        where TField : BaseField<TValue>, new()
    {
        public virtual bool CanCreate(Type type) => typeof(TValue).IsAssignableFrom(type);
        public virtual object GetDefaultValue() => default(TValue);
        
        public virtual VisualElement CreateField(Type valueType, object initialValue, Action<object> valueChangedCallback)
        {
            var field = new TField();
            if (initialValue != null)
                field.value = (TValue)initialValue;
            
            field.RegisterCallback<ChangeEvent<TValue>>(evt => valueChangedCallback?.Invoke(evt.newValue));
            return field;
        }
    }
}