using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews
{
    public class ScenarioPortView : Port, IScenarioGraphElement
    {
        public ScenarioPortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) 
            : base(portOrientation, portDirection, portCapacity, type)
        {
            
        }

        public Vector2 GetCenterPosition() => GetPosition().center;
        public GraphElement GetGraphElement() => this;
        public void SetCenterPosition(Vector2 newPos)
        {
            style.left = newPos.x;// - style.width.value.value / 2;
            style.top = newPos.y;// + style.height.value.value / 2;
        }
        
        public new static ScenarioPortView Create<TEdge>(Orientation orientation, 
            Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
        {
            var listener = new ScenarioLinkConnectorListener();
            var ele = new ScenarioPortView(orientation, direction, capacity, type) 
                { m_EdgeConnector = new EdgeConnector<TEdge>(listener) };
            ele.AddManipulator(ele.m_EdgeConnector);
            return ele;
        }

        public override void OnStartEdgeDragging()
        {
            base.OnStartEdgeDragging();
        }
        public override void OnStopEdgeDragging()
        {
            base.OnStopEdgeDragging();
            // TODO добавить сюда вызов контекстного меню если путь этого edge пуст, в меню предлагать создать что-то
        }

        // Copy from Port.DefaultEdgeConnectorListener with love
        private class ScenarioLinkConnectorListener : IEdgeConnectorListener 
        {
            private readonly GraphViewChange mGraphViewChange;
            private readonly List<Edge> edgesToCreate;
            private readonly List<GraphElement> edgesToDelete;
            
            public ScenarioLinkConnectorListener()
            {
                edgesToCreate = new List<Edge>();
                edgesToDelete = new List<GraphElement>();
                mGraphViewChange.edgesToCreate = edgesToCreate;
            }
            
            public void OnDropOutsidePort(Edge edge, Vector2 position)
            {
                
            }
            
            public void OnDrop(GraphView graphView, Edge edge)
            {
                edgesToCreate.Clear();
                edgesToCreate.Add(edge);
                edgesToDelete.Clear();
                
                if (edge.input.capacity == Capacity.Single)
                {
                    foreach (var connection in edge.input.connections)
                    {
                        if (connection != edge)
                            edgesToDelete.Add(connection);
                    }
                }
                if (edge.output.capacity == Capacity.Single)
                {
                    foreach (var connection in edge.output.connections)
                    {
                        if (connection != edge)
                            edgesToDelete.Add(connection);
                    }
                }
                
                if (edgesToDelete.Count > 0) 
                    graphView.DeleteElements(edgesToDelete); 
                var localEdgesToCreate = edgesToCreate; 
                if (graphView.graphViewChanged != null) 
                    localEdgesToCreate = graphView.graphViewChanged(mGraphViewChange).edgesToCreate; 
                
                foreach (var edgeToCreate in localEdgesToCreate) 
                {
                    graphView.AddElement(edgeToCreate);
                    edge.input.Connect(edgeToCreate);
                    edge.output.Connect(edgeToCreate);
                }
            }
        }
    }
}