using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: SerializableGroupElement
//     Next: EditorGroupV1

// ReSharper disable once CheckNamespace
namespace Scenario.GraphEditor.Elements.Serialization
{
    public class SerializableGroupElement
    {
        public string Name;
        public List<SerializableNodeElement> Nodes;
        public float X, Y, XSize, YSize;
        
        public EditorGroupV1 Convert(Dictionary<SerializableNodeElement, EditorNodeV1> nodeConvert)
        {
            return new EditorGroupV1
            {
                Name = Name,
                Nodes = Nodes.Select(n => nodeConvert[n])
                    //.Cast<IEditorNode>()
                    .ToList(),
                X = X, Y = Y, XSize = XSize, YSize = YSize,
            };
        }
    }
}