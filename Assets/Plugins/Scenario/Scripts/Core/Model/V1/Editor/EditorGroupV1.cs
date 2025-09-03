using System;
using System.Collections.Generic;

// Previous: SerializableGroupElement
//  Current: EditorGroupV1
//     Next: EditorGroupV3

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EditorGroupV1
    {
        public string Name { get; set; }
        public List<EditorNodeV1> Nodes { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float XSize { get; set; }
        public float YSize { get; set; }
    }
}