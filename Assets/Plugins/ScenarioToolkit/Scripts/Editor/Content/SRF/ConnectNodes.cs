using System.Collections.Generic;
using Scenario.Core.Model;
using Scenario.Editor.SRF;
using Scenario.Editor.Windows;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using UnityEditor.Experimental.GraphView;

namespace Scenario.Editor.Content.SRF
{
    public class ConnectNodes : IScenarioReflexFunction
    {
        public SRFMetadata GetMetadata()
        {
            return new SRFMetadata
            {
                FunctionName = "Connect Nodes",
                FunctionTooltip = "Соединяет ноды в редакторе"
            };
        }

        public void BuildUI(SRFContext context)
        {
            context.CreateList<NodeRef>("FromNodes", tooltip:"Входящие ноды для всех связей");
            context.CreateList<NodeRef>("ToNodes", tooltip:"Выходящие ноды для всех связей");
        }
        public void Execute(SRFContext context)
        {
            var fromNodes = context.GetList<NodeRef>("FromNodes");
            var toNodes = context.GetList<NodeRef>("ToNodes");
            var graphController = SWEContext.Graph.GraphController;
            
            var fromNodeViews = new List<ScenarioNodeView>();
            var toNodeViews = new List<ScenarioNodeView>();
            foreach (var fromRef in fromNodes)
            {
                if (graphController.NodeViews.TryGetValue(fromRef.Hash, out var fromNodeView))
                    fromNodeViews.Add(fromNodeView);
            }
            foreach (var toRef in toNodes)
            {
                if (graphController.NodeViews.TryGetValue(toRef.Hash, out var toNodeView))
                    toNodeViews.Add(toNodeView);
            }

            foreach (var fromView in fromNodeViews)
            {
                foreach (var toView in toNodeViews)
                {
                    graphController.CreateEditorLink(fromView, toView);
                }
            }
        }
    }
}