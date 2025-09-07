using System;
using Scenario.Core.Model.Interfaces;
using Scenario.Editor.CustomVisualElements;
using Scenario.Editor.Utilities.Providers;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using Scenario.Editor.Windows.SearchLegacy;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Scenario.Editor.Windows.ElementEditor.NodeElements
{
    public class ComponentsNodeElement<TComponent> : BaseNodeElement
        where TComponent : IScenarioComponent
    {
        private readonly Button addComponent;
        private readonly IScenarioNodeComponents<TComponent> componentsNode;
        private readonly NodeOverridesController nodeOverridesController;

        private readonly VisualTreeAsset componentAsset;
        private readonly VisualTreeAsset fieldAsset;

        private readonly ScrollView componentsView;
        
        public ComponentsNodeElement(ScenarioNodeView nodeView, GraphEditorWindow graphEditor, 
            NodeOverridesController nodeOverridesController) : base(nodeView, graphEditor)
        {
            componentsNode = (IScenarioNodeComponents<TComponent>)nodeView.ScenarioNode;
            this.nodeOverridesController = nodeOverridesController;
            var enumField = CreateActivationTypeEnum(componentsNode);
            Root.Add(enumField);

            componentAsset = UxmlEditorProvider.instance.NodesComponent;
            fieldAsset = UxmlEditorProvider.instance.NodesField;
            Root.Add(UxmlEditorProvider.instance.ListsComponents.Instantiate());
            
            componentsView = Root.Q<ScrollView>("components");
            addComponent = Root.Q<Button>("add-component");
            addComponent.clicked += AddComponent;
            
            var componentsTitle = Root.Q<Label>("title");
            componentsTitle.AddManipulator(new ContextualMenuManipulator(ConstructFire));
            
            nodeOverridesController.Load(componentsNode);
        }

        private void AddComponent()
        {
            ComponentTypesSearchWindow.Open<TComponent>(CreateComponent, type 
                => !typeof(INotSerializableComponent).IsAssignableFrom(type));
        }
        private void CreateComponent(Type type)
        {
            var component = (TComponent)Activator.CreateInstance(type);
            if (component is IComponentDefaultValues defaultValues)
                defaultValues.SetDefault();
            
            nodeOverridesController.Add(component);
            componentsNode.Components.Add(component);
            
            OnDataUpdated();
        }
        
        public override void RedrawElements()
        {
            base.RedrawElements();
            
            componentsView.Clear();
            var length = componentsNode.Components.Count;
            for (var i = 0; i < length; i++)
            {
                var currentCounter = i;
                
                var element = new ComponentElementEditor(componentAsset, fieldAsset);
                element.AddManipulator(new ContextualMenuManipulator(ConstructComponent));
                element.Bind(componentsNode.Components[i], nodeOverridesController.CurrentOverrides[i]);
                
                element.Removed += () =>
                {
                    nodeOverridesController.Remove(currentCounter);
                    componentsNode.Components.Remove(componentsNode.Components[currentCounter]);
                    OnDataUpdated();
                };
                element.ValueChanged += () =>
                {
                    OnDataUpdated(false);
                };
                element.MovedUp += () => SwapComponents(componentsNode.Components[currentCounter], -1);
                element.MovedDown += () => SwapComponents(componentsNode.Components[currentCounter], +1);
                componentsView.Add(element);
            }
        }

        protected override void OnDataUpdated(bool redrawElements)
        {
            //base.OnDataUpdated(redrawElements);
            if (redrawElements) RedrawElements();
            
            // кринж, ну да ладно, аналог flush
            nodeOverridesController.Save();
            nodeOverridesController.Load(componentsNode);
            
            NodeView.UpdateVisuals();
            InvokeDataUpdated();
        }

        private void SwapComponents(TComponent component, int delta)
        {
            var indexA = componentsNode.Components.IndexOf(component);
            var indexB = Mathf.Clamp(indexA + delta, 0, componentsNode.Components.Count - 1);
            if (indexA == indexB) return;
            
            nodeOverridesController.Swap(indexA, indexB);
            (componentsNode.Components[indexA], componentsNode.Components[indexB]) 
                = (componentsNode.Components[indexB], componentsNode.Components[indexA]);
            
            OnDataUpdated();
        }

        private void ConstructComponent(ContextualMenuPopulateEvent evt)
        {
            var componentElement = (ComponentElementEditor)evt.target;
            evt.menu.AppendAction("Copy", _ => Copy(componentElement), DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendAction("Paste", _ => Paste(componentElement), DropdownMenuAction.AlwaysEnabled);
            //evt.menu.AppendAction("Open source code", _ => OpenSourceCode(componentElement), DropdownMenuAction.AlwaysEnabled);
            ConstructFire(evt);
        }
        private void ConstructFire(ContextualMenuPopulateEvent evt)
        {
            if (Application.isPlaying)
                evt.menu.AppendAction("Fire (local)", _ => Fire(componentsNode), DropdownMenuAction.AlwaysEnabled);
        }

        private void Copy(ComponentElementEditor componentElementEditor)
        {
            GUIUtility.systemCopyBuffer = GraphEditor.Serialization.Serialize(componentElementEditor.Component);
        }
        private void Paste(ComponentElementEditor componentElementEditor)
        {
            var obj = GraphEditor.Serialization.Deserialize<IScenarioComponent>(GUIUtility.systemCopyBuffer);
            var component = (TComponent)obj;
            var index = componentsView.IndexOf(componentElementEditor);
            componentsNode.Components.Insert(index, component);
            nodeOverridesController.Insert(index, component);
            OnDataUpdated();
        }
        private void OpenSourceCode(ComponentElementEditor componentElementEditor)
        {
            var type = componentElementEditor.Component.GetType();
            // TODO найти решение для получения пути файла, где находится компонент
            //Debug.Log(MonoCecilHelper.TryGetCecilFileName(type));
            
            //var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            //AssetDatabase.OpenAsset(asset, line);
        }
        
        private void Fire(IScenarioNodeComponents<TComponent> componentsNode)
        {
            foreach (var component in componentsNode.Components)
                Fire(component);
        }
        private void Fire(object component)
        {
            GraphEditor.Bus.Fire(component);
        }
    }
}