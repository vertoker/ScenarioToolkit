using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModestTree;
using Scenario.Core.Model.Interfaces;
using Scenario.Editor.Content.Fields;
using Scenario.Editor.Content.UXML;
using Scenario.Utilities;
using Scenario.Utilities.Attributes;
using UnityEngine.UIElements;

namespace Scenario.Editor.CustomVisualElements
{
    /// <summary>
    /// Редактор для любого IScenarioComponent, также поддерживает override переменных и мета аттрибуты
    /// </summary>
    public class ComponentElementEditor : VisualElement
    {
        public event Action ValueChanged;
        public event Action Removed;
        public event Action MovedUp;
        public event Action MovedDown;
        
        private readonly VisualTreeAsset fieldAsset;
        private readonly Foldout foldout;
        private readonly Button info;
        
        public IScenarioComponent Component { get; private set; }
        public IComponentVariables Overrides;
        
        public ComponentElementEditor(VisualTreeAsset componentAsset, VisualTreeAsset fieldAsset)
        {
            this.fieldAsset = fieldAsset;
            var element = componentAsset.Instantiate();
            foldout = element.Q<Foldout>("props");
            element.Q<Button>("close").clicked += () => Removed?.Invoke();
            element.Q<Button>("up").clicked += () => MovedUp?.Invoke();
            element.Q<Button>("down").clicked += () => MovedDown?.Invoke();

            info = element.Q<Button>("info");
            
            Add(element);
        }

        public void Bind(IScenarioComponent component, IComponentVariables overrides)
        {
            Component = component;
            Overrides = overrides;
            
            if (Component != null) Redraw();
            else RedrawNull();
        }

        private void Redraw()
        {
            var componentType = Component.GetType();
            foldout.text = componentType.Name;
            foldout.Clear();
            
            var attribute = componentType.TryGetAttribute<ScenarioMetaAttribute>();
            UpdateInfoBtn(info, attribute);
            
            var members = Component.GetComponentFields();
            if (members.Length == 0) foldout.Add(new Label("No fields"));
            foreach (var element in CreateFields(members)) foldout.Add(element);
        }
        private void RedrawNull()
        {
            foldout.text = "null";
            foldout.Clear();
            //foldout.Add(new Label("No fields"));
        }

        // Та самая поддержка мета-аттрибутов сценария,
        // работает как для самого компонента, так и для каждого поля внутри него 
        private static void UpdateInfoBtn(Button infoBtn, ScenarioMetaAttribute attribute)
        {
            if (attribute == null)
            {
                infoBtn.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                return;
            }

            var tooltip = attribute.GetTooltip();
            if (string.IsNullOrWhiteSpace(tooltip))
            {
                infoBtn.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                return;
            }
            
            infoBtn.SetEnabled(false);
            infoBtn.tooltip = tooltip;
        }

        private IEnumerable<VisualElement> CreateFields(FieldInfo[] fields)
        {
            var length = fields.Length;
            for (var i = 0; i < length; i++)
            {
                var currentCounter = i;
                var localCacheText = string.Empty;
                
                var fieldValueType = fields[i].FieldType;
                var fieldValue = Component.GetValueByField(fields[i]);
                var fieldCreator = ScenarioFields.GetCreators().First(creator => creator.CanCreate(fieldValueType));
                var field = fieldCreator.CreateField(fieldValueType, fieldValue, CreateMemberSetCallback(Component, fields[i]));

                var data = new NodesFieldUxml(fieldAsset);
                
                var attribute = fields[i].TryGetAttribute<ScenarioMetaAttribute>();
                UpdateInfoBtn(data.InfoBtnComponent, attribute);
                
                data.ToggleField.RegisterCallback<ChangeEvent<bool>>(ToggleCallback);
                data.TextField.RegisterCallback<ChangeEvent<string>>(VariableFieldCallback);

                var variableName = Overrides.GetValueOrDefault(fields[i].Name).VariableName;
                if (!string.IsNullOrWhiteSpace(variableName))
                {
                    using var toggleChange = ChangeEvent<bool>.GetPooled(false, true);
                    using var textChange = ChangeEvent<string>.GetPooled(string.Empty, variableName);
                    
                    data.ToggleField.value = true;
                    ToggleCallback(toggleChange);
                    
                    data.TextField.value = variableName;
                    VariableFieldCallback(textChange);
                }
                
                data.NameField.text = fields[i].Name;
                data.FieldElement.Add(field);
                yield return data.RootContainer;
                continue;

                void ToggleCallback(ChangeEvent<bool> changeEvent)
                {
                    //Debug.Log($"Update to {changeEvent.newValue}");
                    if (changeEvent.newValue)
                    {
                        data.FieldElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                        data.VariableFieldElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                        
                        Overrides.Insert(fields[currentCounter].Name, localCacheText);
                        data.TextField.value = localCacheText;
                    }
                    else
                    {
                        data.VariableFieldElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                        data.FieldElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                        
                        localCacheText = data.TextField.value;
                        Overrides.Insert(fields[currentCounter].Name, string.Empty);
                    }
                    ValueChanged?.Invoke();
                }
                void VariableFieldCallback(ChangeEvent<string> changeEvent)
                {
                    //Debug.Log($"Update to {changeEvent.newValue}");
                    Overrides.Insert(fields[currentCounter].Name, changeEvent.newValue);
                    ValueChanged?.Invoke();
                }
            }
        }

        private Action<object> CreateMemberSetCallback(IScenarioComponent instance, FieldInfo fieldInfo)
        {
            return value =>
            {
                instance.SetValueByField(fieldInfo, value);
                ValueChanged?.Invoke();
            };
        }
    }
}