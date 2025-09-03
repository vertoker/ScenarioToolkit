using System;
using Scenario.Editor.Content.Fields.Base;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Scenario.Editor.Content.Fields.Types.Unity
{
    public class UObjectFieldCreator : BaseFieldCreator<Object, ObjectField>
    {
        public override VisualElement CreateField(Type valueType, object initialValue, Action<object> valueChangedCallback)
        {
            var field = (ObjectField)base.CreateField(valueType, initialValue, valueChangedCallback);
            field.objectType = valueType;
            return field;
        }
    }
}