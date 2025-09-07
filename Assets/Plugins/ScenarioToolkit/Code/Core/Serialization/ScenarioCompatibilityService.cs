using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.GraphEditor.Elements.Serialization;
using ScenarioToolkit.Core.Nodes;
using ScenarioToolkit.Shared;
using UnityEngine;

namespace ScenarioToolkit.Core.Serialization
{
    /// <summary>
    /// Сервис конвертации моделей сценариев до последней версии.
    /// Поддерживает только upcast версии, downcast не предусмотрен.
    /// Вся логика конвертации находится только в этом файле
    /// </summary>
    public static class ScenarioCompatibilityService
    {
        public static IScenarioModel UpdateModel(object model)
        {
            if (model is not IScenarioCompatibilityModel)
                throw new Exception("Not a IScenarioCompatibilityModel, use validate conversion");
            
            if (model is ScenarioData modelV0Runtime) model = Convert(modelV0Runtime);
            if (model is SerializableGraphElements modelV0Editor) model = Convert(modelV0Editor);
            
            // Versions here
            
            if (model is ScenarioModelV1 modelV1) model = Convert(modelV1);
            if (model is ScenarioModelV2 modelV2) model = Convert(modelV2);
            if (model is ScenarioModelV3 modelV3) model = Convert(modelV3);
            if (model is ScenarioModelV4 modelV4) model = Convert(modelV4);
            if (model is ScenarioModelV5 modelV5) model = Convert(modelV5);
            
            return (IScenarioModel)model;
        }

        private static ScenarioModelV6 Convert(ScenarioModelV5 model)
        {
            // Nodes Convert
            
            Dictionary<ScenarioNodeV1, ScenarioNodeV6> nodes = new(model.Graph.Nodes.Count);
            foreach (var node in model.Graph.Nodes) nodes.Add(node, null);
            
            nodes = ReplaceNodes3<ActionNodeV1, ActionNodeV6>(nodes, ConvertActionNode);
            nodes = ReplaceNodes3<ConditionNodeV1, ConditionNodeV6>(nodes, ConvertConditionNode);
            nodes = ReplaceNodes3<SubgraphNodeV5, SubgraphNodeV6>(nodes, ConvertSubgraphNode);
            nodes = ReplaceNodes3<StartNodeV1, StartNodeV6>(nodes, ConvertStartNode);
            nodes = ReplaceNodes3<EndNodeV5, EndNodeV6>(nodes, ConvertEndNode);
            nodes = ReplaceNodes3<NoteNodeV2, NoteNodeV6>(nodes, ConvertNoteNode);
            nodes = ReplaceNodes3<PortInNodeV4, PortInNodeV6>(nodes, ConvertPortInNode);
            nodes = ReplaceNodes3<PortOutNodeV4, PortOutNodeV6>(nodes, ConvertPortOutNode);
            LinkPortNodesV6();
            
            // Editor Nodes Convert

            Dictionary<EditorNodeV3, EditorNodeV6> editorNodes = new(model.EditorGraph.Nodes.Count);
            
            foreach (var editorNodeV3 in model.EditorGraph.Nodes)
            {
                EditorNodeV6 editorNodeV6 = new()
                {
                    Node = nodes[editorNodeV3.Node],
                    Position = editorNodeV3.Position
                };
                editorNodes.Add(editorNodeV3, editorNodeV6);
            }
            
            
            // Context

            var newOverrides = new Dictionary<int, List<IComponentVariables>>();
            foreach (var bindNode in model.Context.NodeOverrides)
            {
                var newNode = new List<IComponentVariables>();
                
                foreach (var bindComponent in bindNode.Value)
                {
                    if (bindComponent != null)
                    {
                        var newComponent = new ComponentVariablesV6();
                        
                        foreach (var member in bindComponent.MemberVariables)
                        {
                            var newMember = new MemberVariableV6
                            {
                                MemberName = member.MemberName,
                                VariableName = member.VariableName,
                            };

                            newComponent.MemberVariables.Add(newMember);
                        }
                        newNode.Add(newComponent);
                    }
                    else
                    {
                        newNode.Add(null);
                    }
                }

                newOverrides.Add(bindNode.Key, newNode);
            }
            
            var contextV6 = new ScenarioContextV6
            {
                Variables = model.Context.Variables,
                NodeOverrides = newOverrides,
            };
            
            // Graph
            
            var graphV6 = new ScenarioGraphV6();

            foreach (var nodeV1 in model.Graph.Nodes)
                graphV6.AddNode(nodes.GetValueOrDefault(nodeV1));
            foreach (var linkV1 in model.Graph.Links)
            {
                var fromV6 = nodes.GetValueOrDefault(linkV1.From) as IScenarioNodeFlow;
                var toV6 = nodes.GetValueOrDefault(linkV1.To) as IScenarioNodeFlow;
                
                var link = new ScenarioLinkFlowV6
                {
                    From = fromV6,
                    To = toV6
                };
                graphV6.AddLink(link);
            }
            
            // Editor Graph

            var editorV6 = new EditorGraphV6();
            
            foreach (var editorNodeV3 in model.EditorGraph.Nodes)
                editorV6.AddNode(editorNodes.GetValueOrDefault(editorNodeV3));
            foreach (var editorLinkV3 in model.EditorGraph.Links)
            {
                var fromV6 = editorNodes.GetValueOrDefault(editorLinkV3.From);
                var toV6 = editorNodes.GetValueOrDefault(editorLinkV3.To);

                var link = new EditorLinkV6
                {
                    From = fromV6,
                    To = toV6
                };
                editorV6.AddLink(link);
            }
            foreach (var editorGroupV3 in model.EditorGraph.Groups)
            {
                var nodesV6 = new HashSet<IEditorNode>(editorGroupV3.Nodes.Count);
                foreach (var editorNode in editorGroupV3.Nodes)
                {
                    var newEditorNode = editorNodes.GetValueOrDefault(editorNode);
                    if (newEditorNode != null) nodesV6.Add(newEditorNode);
                }

                var group = new EditorGroupV6
                {
                    Name = editorGroupV3.Name,
                    Nodes = nodesV6,
                    Position = editorGroupV3.Position,
                };
                if (group.Hash == 0) group.InitializeHash();
                
                editorV6.AddGroup(group);
            }
            
            return new ScenarioModelV6
            {
                Context = contextV6,
                Graph = graphV6,
                EditorGraph = editorV6,
            };
            
            
            ActionNodeV6 ConvertActionNode(ActionNodeV1 oldNode)
            {
                ActionNodeV6 newNode = new();
                CopyBaseFlowNode(newNode, oldNode);

                foreach (var component in oldNode.Components)
                    newNode.Components.Add(component);
                
                return newNode;
            }
            ConditionNodeV6 ConvertConditionNode(ConditionNodeV1 oldNode)
            {
                ConditionNodeV6 newNode = new();
                CopyBaseFlowNode(newNode, oldNode);

                foreach (var component in oldNode.Components)
                    newNode.Components.Add(component);
                
                return newNode;
            }
            SubgraphNodeV6 ConvertSubgraphNode(SubgraphNodeV5 oldNode)
            {
                SubgraphNodeV6 newNode = new();
                CopyBaseFlowNode(newNode, oldNode);

                newNode.LoadType = oldNode.LoadType;
                newNode.Json = oldNode.Json;
                newNode.StreamingPath = oldNode.StreamingPath;
                newNode.AbsolutePath = oldNode.AbsolutePath;
                newNode.Variables = oldNode.Variables;
                
                return newNode;
            }
            StartNodeV6 ConvertStartNode(StartNodeV1 oldNode)
            {
                StartNodeV6 newNode = new();
                CopyBaseFlowNode(newNode, oldNode);
                
                return newNode;
            }
            EndNodeV6 ConvertEndNode(EndNodeV5 oldNode)
            {
                EndNodeV6 newNode = new();
                CopyBaseFlowNode(newNode, oldNode);

                newNode.InstantEnd = oldNode.InstantEnd;
                
                return newNode;
            }
            NoteNodeV6 ConvertNoteNode(NoteNodeV2 oldNode)
            {
                NoteNodeV6 newNode = new();
                CopyBaseFlowNode(newNode, oldNode);

                newNode.Text = oldNode.Text;
                
                return newNode;
            }
            
            PortInNodeV6 ConvertPortInNode(PortInNodeV4 oldNode)
            {
                PortInNodeV6 newNode = new();
                CopyBaseFlowNode(newNode, oldNode);

                return newNode;
            }
            PortOutNodeV6 ConvertPortOutNode(PortOutNodeV4 oldNode)
            {
                PortOutNodeV6 newNode = new();
                CopyBaseFlowNode(newNode, oldNode);
                
                return newNode;
            }
            void LinkPortNodesV6()
            {
                foreach (var bind in nodes)
                {
                    if (bind.Key is PortInNodeV4 { OutputNode: not null } portInV4)
                    {
                        var portInV6 = nodes[portInV4] as PortInNodeV6;
                        var portOutV6 = nodes[portInV4.OutputNode] as PortOutNodeV6;
                        if (portInV6 != null)  portInV6.OutputNode = portOutV6;
                        if (portOutV6 != null) portOutV6.InputNode = portInV6;
                    }
                    else if (bind.Key is PortOutNodeV4 { InputNode: not null } portOutV4)
                    {
                        var portOutV6 = nodes[portOutV4] as PortOutNodeV6;
                        var portInV6 = nodes[portOutV4.InputNode] as PortInNodeV6;
                        if (portOutV6 != null) portOutV6.InputNode = portInV6;
                        if (portInV6 != null)  portInV6.OutputNode = portOutV6;
                    }
                }
            }
        }
        private static ScenarioModelV5 Convert(ScenarioModelV4 model)
        {
            // Graph
            
            var nodes = model.Graph.Nodes.ToDictionary(node => node);
            nodes = ReplaceNodes2<ScenarioNodeV1, SubgraphNodeV2, SubgraphNodeV5>(nodes, ConvertSubgraphNode);
            nodes = ReplaceNodes2<ScenarioNodeV1, EndNodeV1, EndNodeV5>(nodes, ConvertEndNode);
            
            var graphV5 = new ScenarioGraphV5
            {
                Nodes = nodes.Values.ToHashSet(),
                Links = model.Graph.Links,
            };
            foreach (var link in graphV5.Links)
            {
                link.From = nodes[link.From];
                link.To = nodes[link.To];
            }
            
            // Editor Graph
            
            var editorV5 = new EditorGraphV5
            {
                Nodes = model.EditorGraph.Nodes,
                Links = model.EditorGraph.Links,
                Groups = model.EditorGraph.Groups
            };
            foreach (var editorNode in editorV5.Nodes)
            {
                editorNode.Node = nodes[editorNode.Node];
            }

            // Result
            
            return new ScenarioModelV5
            {
                Context = model.Context,
                Graph = graphV5,
                EditorGraph = editorV5,
            };
            
            SubgraphNodeV5 ConvertSubgraphNode(SubgraphNodeV2 oldNode)
            {
                SubgraphNodeV5 newNode = new();
                CopyBaseNode(newNode, oldNode);

                newNode.LoadType = SubgraphLoadType.TextAsset;
                newNode.Json = oldNode.Json;
                newNode.StreamingPath = string.Empty;
                newNode.AbsolutePath = string.Empty;
                newNode.Variables = oldNode.Variables;

                return newNode;
            }
            EndNodeV5 ConvertEndNode(EndNodeV1 oldNode)
            {
                EndNodeV5 newNode = new();
                CopyBaseNode(newNode, oldNode);

                newNode.InstantEnd = false;

                return newNode;
            }
        }
        
        private static ScenarioModelV4 Convert(ScenarioModelV3 model)
        {
            return new ScenarioModelV4
            {
                Context = model.Context,
                Graph = model.Graph,
                EditorGraph = model.EditorGraph,
            };
        }
        private static ScenarioModelV3 Convert(ScenarioModelV2 model)
        {
            var editorV1 = model.EditorGraph;
            var newEditorGraph = new EditorGraphV3();
            
            var nodesBind = new Dictionary<EditorNodeV1, EditorNodeV3>();
            foreach (var editorNodeV1 in editorV1.Nodes)
            {
                var newNode = new EditorNodeV3
                {
                    Node = editorNodeV1.Node,
                    Position = new Vector2(editorNodeV1.X, editorNodeV1.Y),
                };
                nodesBind.Add(editorNodeV1, newNode);
            }
            
            var newNodes = nodesBind.Values;
            var newLinks = editorV1.Links.Select(linkV1 => new EditorLinkV3
            {
                From = nodesBind[linkV1.From],
                To = nodesBind[linkV1.To],
            });
            var newGroups = editorV1.Groups.Select(groupV1 => new EditorGroupV3
            {
                Name = groupV1.Name,
                Nodes = groupV1.Nodes.Select(n => nodesBind[n])
                    //.Cast<IEditorNode>() // comment cast if next update
                    .ToList(),
                Position = new Vector2(groupV1.X, groupV1.Y),
            });
            
            newEditorGraph.Nodes = newNodes
                //.Cast<IEditorNode>() // comment cast if next update
                .ToList();
            newEditorGraph.Links = newLinks
                //.Cast<IEditorLink>() // comment cast if next update
                .ToList();
            newEditorGraph.Groups = newGroups
                //.Cast<IEditorGroup>() // comment cast if next update
                .ToList();
            
            return new ScenarioModelV3
            {
                Context = model.Context,
                Graph = model.Graph,
                EditorGraph = newEditorGraph,
            };
        }
        private static ScenarioModelV2 Convert(ScenarioModelV1 model)
        {
            model.Graph.Nodes = ReplaceNodes1<ScenarioNodeV1, SubgraphNodeV1, SubgraphNodeV2>(model.Graph.Nodes, ConvertNode);
            return new ScenarioModelV2
            {
                Context = new ScenarioContextV2(),
                Graph = model.Graph,
                EditorGraph = model.EditorGraph,
            };
            
            SubgraphNodeV2 ConvertNode(SubgraphNodeV1 oldNode)
            {
                SubgraphNodeV2 newNode = new();
                CopyBaseNode(newNode, oldNode);

                newNode.Json = oldNode.Json;
                newNode.Variables = new Dictionary<string, ObjectTyped>();

                return newNode;
            }
        }

        #region V0

        private static ScenarioModelV1 Convert(SerializableGraphElements model)
        {
            // Create Graphs
            var editorGraph = model.ConvertV1();
            var scenarioGraph = new ScenarioGraphV1();

            // Fill Runtime Graph
            foreach (var bind in editorGraph.Nodes)
                scenarioGraph.AddNode(bind.Node);
            foreach (var editorLink in editorGraph.Links)
                scenarioGraph.AddLink(new ScenarioLinkV1
                {
                    From = editorLink.From.Node,
                    To = editorLink.To.Node,
                    //From = editorGraph.Nodes[editorLink.From],
                    //To = editorGraph.Nodes[editorLink.To],
                });
            
            // Finalize
            var newModel = new ScenarioModelV1
            {
                Graph = scenarioGraph,
                EditorGraph = editorGraph,
            };
            return newModel;
        }
        
        private static IEditorNodeContent[] Contents;
        
        private static ScenarioModelV1 Convert(ScenarioData model)
        {
            // Create Runtime Graph
            var scenarioGraph = model.Graph.Convert();
            
            // Create temp cache dictionary
            var nodeToEditorDict = scenarioGraph.Nodes
                .ToDictionary(node => node, 
                    node => new EditorNodeV1 { Node = node, ContentType = node.GetCompatibilityContent().GetType() }
                );
            
            // Create Editor Graph using two dictionaries
            var editorGraph = new EditorGraphV1
            {
                Nodes = nodeToEditorDict.Values.ToList(),
                Links = scenarioGraph.Links.Select(link => new EditorLinkV1
                {
                    From = nodeToEditorDict[link.From],
                    To = nodeToEditorDict[link.To],
                }).ToList(),
            };
            
            Contents ??= Reflection.GetImplementations<IEditorNodeContent>()
                .Select(Activator.CreateInstance).Cast<IEditorNodeContent>().ToArray();

            const float xOffset = 256f * 1f;
            const float yOffset = 256f * 2f;
            
            var counter = 0;
            foreach (var editorNode in nodeToEditorDict.Values)
            {
                editorNode.X = counter * xOffset;
                editorNode.Y = 0;
                counter++;
            }
            
            // Place all nodes by their topology position
            var startNode = (StartNodeV1)scenarioGraph.Nodes.FirstOrDefault(node => node is StartNodeV1);
            foreach (var nodeTopology in scenarioGraph.BuildTopologyTree(startNode))
            {
                var editorNode = nodeToEditorDict[nodeTopology.Node];
                editorNode.X = nodeTopology.Depth * xOffset;
                editorNode.Y = nodeTopology.Index * yOffset;
            }
            
            // Finalize
            var newModel = new ScenarioModelV1
            {
                Graph = scenarioGraph,
                EditorGraph = editorGraph,
            };
            return newModel;
        }
        #endregion

        #region Utility
        private static HashSet<TNode> ReplaceNodes1<TNode, TNodeOld, TNodeNew>(HashSet<TNode> nodes, 
            Func<TNodeOld, TNodeNew> converter) where TNodeOld : TNode where TNodeNew : TNode
        {
            var newNodes = new HashSet<TNode>(nodes.Count);

            foreach (var node in nodes)
            {
                if (node is TNodeOld oldNode)
                {
                    var newNode = converter(oldNode);
                    newNodes.Add(newNode);
                    continue;
                }
                newNodes.Add(node);
            }

            return newNodes;
        }
        private static Dictionary<TNode, TNode> ReplaceNodes2<TNode, TNodeOld, TNodeNew>
            (Dictionary<TNode, TNode> nodes, Func<TNodeOld, TNodeNew> converter)
            where TNodeOld : TNode where TNodeNew : TNode
        {
            var newNodes = new Dictionary<TNode, TNode>(nodes.Count);

            foreach (var bind in nodes)
            {
                if (bind.Value is TNodeOld oldNode)
                {
                    var newNode = converter(oldNode);
                    newNodes.Add(bind.Key, newNode);
                    continue;
                }
                newNodes.Add(bind.Key, bind.Value);
            }

            return newNodes;
        }
        private static Dictionary<TBaseOld, TBaseNew> ReplaceNodes3<TBaseOld, TBaseNew, TNodeOld, TNodeNew>
            (Dictionary<TBaseOld, TBaseNew> nodes, Func<TNodeOld, TNodeNew> converter)
            where TNodeOld : TBaseOld where TNodeNew : TBaseNew
        {
            var newNodes = new Dictionary<TBaseOld, TBaseNew>(nodes.Count);

            foreach (var bind in nodes)
            {
                if (bind.Key is TNodeOld oldNode)
                {
                    var newNode = converter(oldNode);
                    newNodes.Add(oldNode, newNode);
                    continue;
                }
                newNodes.Add(bind.Key, bind.Value);
            }

            return newNodes;
        }
        private static Dictionary<ScenarioNodeV1, ScenarioNodeV6> ReplaceNodes3<TNodeOld, TNodeNew>
            (Dictionary<ScenarioNodeV1, ScenarioNodeV6> nodes, Func<TNodeOld, TNodeNew> converter)
            where TNodeOld : ScenarioNodeV1 where TNodeNew : ScenarioNodeV6 
            => ReplaceNodes3<ScenarioNodeV1, ScenarioNodeV6, TNodeOld, TNodeNew>(nodes, converter);
        
        private static void CopyBaseNode(ScenarioNodeV1 to, ScenarioNodeV1 from)
        {
            to.Name = from.Name;
            to.Hash = from.Hash;
            if (to.Hash == 0)
                to.InitializeHash();
        }
        private static void CopyBaseFlowNode(ScenarioNodeV6 to, ScenarioNodeV1 from)
        {
            to.Name = from.Name;
            to.Hash = from.Hash;
            if (to.Hash == 0)
                to.InitializeHash();
            
            if (to is ScenarioNodeFlowV6 flowTo)
            {
                flowTo.ActivationType = ActivationType.AND;
                flowTo.IncomingLinks = new HashSet<int>();
                flowTo.OutcomingLinks = new HashSet<int>();
            }
        }
        #endregion
    }
}