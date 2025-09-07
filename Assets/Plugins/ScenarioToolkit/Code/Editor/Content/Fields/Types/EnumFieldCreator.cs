using System;
using Scenario.Editor.Content.Fields.Base;
using UnityEngine.UIElements;

namespace Scenario.Editor.Content.Fields.Types
{
    public class EnumFieldCreator : BaseFieldCreator<Enum, EnumField>
    {
        public override VisualElement CreateField(Type valueType, object initialValue, Action<object> valueChangedCallback)
        {
            var field = new EnumField((Enum)initialValue);
            field.RegisterCallback<ChangeEvent<Enum>>(evt => valueChangedCallback?.Invoke(evt.newValue));
            return field;
        }
    }
}