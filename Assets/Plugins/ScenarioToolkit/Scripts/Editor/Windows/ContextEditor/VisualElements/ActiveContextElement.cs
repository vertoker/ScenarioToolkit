using System.Linq;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Editor.CustomVisualElements;
using Scenario.Editor.Utilities.Providers;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Utilities;
using UnityEngine.UIElements;
using ZLinq;

namespace Scenario.Editor.Windows.ContextEditor.VisualElements
{
    /// <summary>
    /// Редактор для контекста, администрирует дочерние классы для редактирования разных данных внутри Context
    /// </summary>
    public class ActiveContextElement : BaseContextElement
    {
        private readonly IScenarioModel model;
        private readonly VariableEnvironmentEditor environmentEditor;
        
        public ActiveContextElement(IScenarioModel model, GraphEditorWindow graphEditor) 
            : base(UxmlEditorProvider.instance.ContextActiveEditor, UssEditorProvider.instance.ContextEditor)
        {
            this.model = model;
            //var rootData = Root.Q("root-data"); // TODO сюда добавить новые данные
            var statistics = Root.Q("statistics");

            var statisticsEditor = new StatisticsEditor(model, statistics);
            graphEditor.GraphController.GraphLoaded += statisticsEditor.LoadUpdate;
            graphEditor.GraphController.GraphChanged += statisticsEditor.Update;
            
            environmentEditor = new VariableEnvironmentEditor(Root, model.Context);
            environmentEditor.DataUpdated += OnDataUpdated;
            environmentEditor.ImportVariablesButton.clicked += ImportAllNodeVariables;
            environmentEditor.ImportVariablesButton.text = "Import All Node Variables";

            var importVariables = new Button(ImportSubgraphVariables) 
                { text = "Import All Subgraph Variables" };
            Root.Add(importVariables);

            //statisticsEditor.Update();
            environmentEditor.Load();
        }

        //private void UpdateStatistics() { }

        private void ImportAllNodeVariables()
        {
            if (model.Context.NodeOverrides == null) return;
            foreach (var nodePair in model.Context.NodeOverrides)
            {
                var node = model.Graph.NodesValuesAVE.FirstOrDefault(n => n.Hash == nodePair.Key);
                if (node is not IScenarioNodeComponents componentsNode) continue;
                
                using var componentsEnumerator = componentsNode.AsValueEnumerable().GetEnumerator();
                using var overridesEnumerator = nodePair.Value.AsValueEnumerable().GetEnumerator();

                while (componentsEnumerator.MoveNext())
                {
                    overridesEnumerator.MoveNext();

                    var component = componentsEnumerator.Current;
                    var componentVariables = overridesEnumerator.Current;

                    foreach (var field in component.GetComponentFields())
                    {
                        if (componentVariables == null) continue;
                        if (componentVariables.TryGet(field.Name, out var memberVariable))
                        {
                            if (!environmentEditor.Contains(memberVariable.VariableName))
                            {
                                var typedValue = ObjectTyped.ConstructNull(field.GetValue(component), field.FieldType);
                                environmentEditor.AddVariable(memberVariable.VariableName, typedValue, true);
                            }
                        }
                    }
                }
            }
            
            OnDataUpdated();
        }
        private void ImportSubgraphVariables()
        {
            foreach (var subgraphNode in model.Graph.NodesValuesAVE.OfType<ISubgraphNode>())
            {
                foreach (var variable in subgraphNode.Variables)
                {
                    if (!environmentEditor.Contains(variable.Key))
                        environmentEditor.AddVariable(variable.Key, variable.Value, true);
                }
            }
            
            OnDataUpdated();
        }
    }
}