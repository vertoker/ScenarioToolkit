using System;
using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace Scenario.Editor.Content.Fields.Base
{
    /// <summary>
    /// Интерфейс для создания экземпляров полей в редакторе
    /// </summary>
    public interface ITypeFieldCreator
    {
        public bool CanCreate(Type type);
        public object GetDefaultValue();
        public VisualElement CreateField(Type valueType,
            [CanBeNull] object initialValue = default, 
            [CanBeNull] Action<object> valueChangedCallback = null);
    }
}