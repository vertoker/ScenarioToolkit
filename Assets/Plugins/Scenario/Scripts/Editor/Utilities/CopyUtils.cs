using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;
using Scenario.Editor.Model;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using UnityEditor.Experimental.GraphView;
using ZLinq;

namespace Scenario.Editor.Utilities
{
    public static class CopyUtils
    {
        // Модель сценария ещё используется как модель копирования
        // Изначально копирование работало с обычным IEditorGraph
        // Но с появлением невидимых элементов, потребовались данные из обычного IScenarioGraph
        // Но при этом сериализация IEditorGraph всё равно используется самостоятельно
        // Просто IScenarioGraph является дополнительными данными для работы механизма копирования
        
        public static CopyModel CreateCopyModel(IScenarioModel scenarioModel, ScenarioGraphController controller)
        {
            //var validatedLinks = scenarioModel.EditorGraph.Links.Select(ConvertLink);
            var invisibleGraph = GetInvisibleGraph(scenarioModel.EditorGraph, controller);

            var model = new CopyModel
            {
                EditorGraph = scenarioModel.EditorGraph,
                InvisibleGraph = invisibleGraph,
            };

            return model;
        }

        public static CopyModel CreateCopyModel(this ScenarioGraphController controller, 
            params GraphElement[] elementsToCopy) => CreateCopyModel(elementsToCopy, controller);
        public static CopyModel CreateCopyModel(this ScenarioGraphController controller, 
            List<GraphElement> elementsToCopy) => CreateCopyModel(elementsToCopy, controller);
        public static CopyModel CreateCopyModel(IEnumerable<GraphElement> elementsToCopy, ScenarioGraphController controller)
        {
            var copyEditorGraph = CreateEditorGraph(elementsToCopy);
            var invisibleGraph = GetInvisibleGraph(copyEditorGraph, controller);

            var model = new CopyModel
            {
                EditorGraph = copyEditorGraph,
                InvisibleGraph = invisibleGraph,
            };

            return model;
        }
        
        // копирование неявных связей не подразумевалось и ломает всю работу с редактором
        // TODO переписать редактор так, чтобы он нормально смог воспринимать невидимые link откуда угодно
        
        private static IScenarioGraph GetInvisibleGraph(IEditorGraph copyEditorGraph, ScenarioGraphController controller)
        {
            var invisibleGraph = IScenarioGraph.CreateNew();
            
            foreach (var editorNode in copyEditorGraph.NodesValuesAVE)
            {
                if (editorNode.Node is IPortInNode inNode)
                {
                    if (inNode.OutcomingLinks.Count == 0) continue;
                    var linkHash = inNode.OutcomingLinks.First();

                    var link = controller.Graph.GetLink(linkHash);
                    if (link == null) continue;

                    if (link.To is IPortOutNode)
                        invisibleGraph.AddLink(link);
                }
            }
            
            return invisibleGraph;
        }
        
        public static IEditorGraph CreateEditorGraph(IEnumerable<GraphElement> elements)
        {
            var elementArray = elements.ToArray();
            
            // Graph View Collection
            
            var editorNodes = elementArray.OfType<ScenarioNodeView>().ToArray();
            
            var editorLinks = elementArray.OfType<ScenarioLinkView>()
                .Where(edge => editorNodes.Contains(edge.input.node) 
                               && editorNodes.Contains(edge.output.node));
            
            var editorGroups = elementArray.OfType<ScenarioGroupView>()
                .Where(group => group.containedElements.OfType<ScenarioNodeView>()
                    .All(node => editorNodes.Contains(node))).ToList();
            
            // Serialized Collections
            
            var editorToSerializedNodes = editorNodes.ToDictionary(n => n, n => n.CreateSerializedNode());

            var serializedNodes = editorToSerializedNodes.Values.ToList();
            var serializedLinks = editorLinks
                .Select(edge => edge.CreateSerializedLink(editorToSerializedNodes)).ToList();
            var serializedGroups = editorGroups
                .Select(group => group.CreateSerializedGroup(editorToSerializedNodes)).ToHashSet();
            
            var editorGraph = IEditorGraph.CreateNew();
            foreach (var editorNode in serializedNodes)
                editorGraph.AddNode(editorNode);
            foreach (var editorLink in serializedLinks)
                editorGraph.AddLink(editorLink);
            foreach (var editorGroup in serializedGroups)
                editorGraph.AddGroup(editorGroup);
            
            return editorGraph;
        }

        // Это дополнительные методы для сериализации активных editor классов
        // Если будете менять editor модели, то в первую очередь смотрите сюда
        
        private static IEditorNode CreateSerializedNode(this ScenarioNodeView nodeView)
        {
            var serializedNode = IEditorNode.CreateNew();
            var rect = nodeView.GetPosition();
            
            serializedNode.Node = nodeView.ScenarioNode;
            serializedNode.Position = rect.position;
            
            return serializedNode;
        }
        private static IEditorLink CreateSerializedLink(this ScenarioLinkView linkView, 
            Dictionary<ScenarioNodeView, IEditorNode> viewToEditorNodes)
        {
            var serializedLink = IEditorLink.CreateNew();
            serializedLink.From = viewToEditorNodes[(ScenarioNodeView)linkView.output.node];
            serializedLink.To = viewToEditorNodes[(ScenarioNodeView)linkView.input.node];
            return serializedLink;
        }
        private static IEditorGroup CreateSerializedGroup(this ScenarioGroupView groupView, 
            Dictionary<ScenarioNodeView, IEditorNode> viewToEditorNodes)
        {
            var serializedGroup = IEditorGroup.CreateNew();
            var rect = groupView.GetPosition();

            serializedGroup.Hash = groupView.Hash;
            serializedGroup.Name = groupView.Name;
            serializedGroup.Position = rect.position;
            serializedGroup.Nodes = groupView.containedElements.OfType<ScenarioNodeView>()
                .Select(nodeView => viewToEditorNodes[nodeView]).ToHashSet();
            
            return serializedGroup;
        }
        
        private static IScenarioLinkFlow ConvertLink(IEditorLink editorLink)
        {
            var link = IScenarioLinkFlow.CreateNew();
            link.From = editorLink.From.Node as IScenarioNodeFlow;
            link.To = editorLink.To.Node as IScenarioNodeFlow;
            return link;
        }
    }
}