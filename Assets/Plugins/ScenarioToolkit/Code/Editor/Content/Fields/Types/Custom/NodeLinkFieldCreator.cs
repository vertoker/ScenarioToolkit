using System;
using Scenario.Core.Model;
using ScenarioToolkit.Editor.Content.Fields.Base;
using ScenarioToolkit.Editor.Windows;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Content.Fields.Types.Custom
{
    public class NodeLinkFieldCreator : ITypeFieldCreator
    {
        public bool CanCreate(Type type) => type == typeof(NodeRef);
        public object GetDefaultValue() => NodeRef.Empty;
        
        public VisualElement CreateField(Type valueType, object initialValue, Action<object> valueChangedCallback)
        {
            var currentLink = initialValue == null ? NodeRef.Empty : (NodeRef)initialValue;
            
            var parent = new VisualElement();
            
            var field = new IntegerField();
            if (initialValue != null) field.value = currentLink.Hash;
            
            var buttons = new VisualElement 
                { style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row) } };
            
            var selectBtn = new Button { text = "Select" };
            var centerBtn = new Button { text = "Center" };
            var statusText = new TextElement();
            
            field.RegisterCallback<ChangeEvent<int>>(evt => ValueChangedCallback(evt.newValue));
            selectBtn.clicked += SelectCallback;
            centerBtn.clicked += CenterCallback;

            UpdateCallback();
            
            buttons.Add(selectBtn);
            buttons.Add(centerBtn);
            buttons.Add(statusText);
            parent.Add(field);
            parent.Add(buttons);
            
            return parent;
            
            
            void ValueChangedCallback(int newValue)
            {
                currentLink = new NodeRef(newValue);
                UpdateCallback();
                valueChangedCallback?.Invoke(currentLink);
            }
            void UpdateCallback()
            {
                //if (SWEContext.Graph == null) SetNodeDisabled(statusText);
                if (currentLink.IsEmpty())
                    SetNodeEmpty(statusText);
                else if (SWEContext.Graph.ModelController.Model.Graph.ContainsNode(currentLink.Hash))
                    SetNodeCorrect(statusText);
                else
                    SetNodeFailed(statusText);
            }
            
            void SelectCallback()
            {
                if (SWEContext.Graph == null)
                {
                    Debug.LogWarning($"Empty context or GraphEditorWindow");
                    return;
                }
                
                var node = SWEContext.Graph.GraphController.Graph.GetNode(currentLink.Hash);
                if (node == null) return;
                var nodeView = SWEContext.Graph.GraphController.NodeViews[node.Hash];
                
                SWEContext.Graph.GraphView.ClearSelection();
                SWEContext.Graph.GraphView.AddToSelection(nodeView);
                SWEContext.Graph.GraphView.Show(nodeView.GetCenterPosition());
            }
            void CenterCallback()
            {
                if (SWEContext.Graph == null)
                {
                    Debug.LogWarning($"Empty context or GraphEditorWindow");
                    return;
                }
                
                var node = SWEContext.Graph.GraphController.Graph.GetNode(currentLink.Hash);
                if (node == null) return;
                var nodeView = SWEContext.Graph.GraphController.NodeViews[node.Hash];
                
                SWEContext.Graph.GraphView.Show(nodeView.GetCenterPosition());
            }
        }

        private static void SetNodeCorrect(TextElement el)
        { el.text = "Founded"; el.style.color = new Color(0.5f, 1f, 0.5f, 1f); }
        private static void SetNodeFailed(TextElement el)
        { el.text = "Not Founded"; el.style.color = new Color(1f, 0.5f, 0.5f, 1f); }
        private static void SetNodeEmpty(TextElement el)
        { el.text = "Empty"; el.style.color = new Color(1f, 1f, 1f, 1f); }
        private static void SetNodeDisabled(TextElement el)
        { el.text = string.Empty; el.style.color = new Color(1f, 1f, 1f, 1f); }
    }
}