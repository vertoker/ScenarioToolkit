using System;
using ModestTree;
using ScenarioToolkit.Editor.Content.Fields.Base;
using ScenarioToolkit.Editor.Windows.SearchLegacy;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Content.Fields.Types.Custom
{
    /// <summary>
    /// Особенная реализация FieldCreator, которая работает с неизвестным enum.
    /// Она просто отрисовывает ещё одно поле, в котором нужно выбрать тип enum
    /// </summary>
    public class EnumUndefinedFieldCreator : BaseFieldCreator<Enum, EnumField>
    {
        // скажу честно, код - полное говно, сделано максимально быстро, поэтому
        // TODO исправить этот говнокод
        
        public override VisualElement CreateField(Type valueType, object initialValue, Action<object> valueChangedCallback)
        {
            var parent = new VisualElement();
            var selectType = new VisualElement 
                { style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row) } };
            parent.Add(selectType);
            
            var btn = new Button { text = "Select Type" };
            
            var btnText = new Button { text = GetTypeName(initialValue) };
            btnText.SetEnabled(false);
            
            var field = new EnumField((Enum)initialValue);
            
            btn.clicked += SelectType;
            
            selectType.Add(btn);
            selectType.Add(btnText);
            parent.Add(field);
            
            return parent;

            void TypeSelected(Type enumType)
            {
                parent.Remove(field);
                var values = Enum.GetValues(enumType);
                var newValue = values.Length == 0 ? null : values.GetValue(0);
                
                field = new EnumField((Enum)newValue);
                btnText.text = GetTypeName(newValue);
                //Debug.Log(newValue.GetType().FullName);
                
                field.RegisterCallback<ChangeEvent<Enum>>(evt => valueChangedCallback?.Invoke(evt.newValue));
                valueChangedCallback?.Invoke(newValue);
                parent.Add(field);
            }

            void SelectType()
            {
                var searchWindowProvider = ScriptableObject.CreateInstance<EnumTypesSearchWindow>();
                searchWindowProvider.Initialize();
                searchWindowProvider.TypeSelected += TypeSelected;
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(btn.transform.position)),
                    searchWindowProvider);
            }
        }

        private static string GetTypeName(object value)
        {
            if (value == null) return "None";
            var type = value.GetType();
            return $"{type.PrettyName()}";
        }
    }
}