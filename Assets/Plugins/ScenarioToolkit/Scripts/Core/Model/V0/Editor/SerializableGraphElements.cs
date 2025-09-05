using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: SerializableGraphElements
//     Next: EditorGraphV1

// ReSharper disable once CheckNamespace
namespace Scenario.GraphEditor.Elements.Serialization
{
    public class SerializableGraphElements : IScenarioCompatibilityModel
    {
        public List<SerializableGroupElement> Groups;
        public List<SerializableLinkElement> Links;
        public List<SerializableNodeElement> Nodes;

        public EditorGraphV1 ConvertV1()
        {
            var newGraph = new EditorGraphV1();

            var nodeConvert = Nodes.ToDictionary(n => n, n => n.ConvertV1());

            foreach (var node in nodeConvert.Values)
                newGraph.Nodes.Add(node);
            
            foreach (var link in Links)
                newGraph.Links.Add(link.Convert(nodeConvert));
            
            foreach (var group in Groups)
                newGraph.Groups.Add(group.Convert(nodeConvert));
            
            return newGraph;
        }
    }
}