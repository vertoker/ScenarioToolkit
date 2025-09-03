using System;
using System.Collections.Generic;

// Previous: SerializableGraphElements
//  Current: EditorGraphV1
//     Next: EditorGraphV3

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EditorGraphV1
    {
        public List<EditorGroupV1> Groups { get; set; } = new();
        public List<EditorLinkV1> Links { get; set; } = new();
        public List<EditorNodeV1> Nodes { get; set; } = new();
    }
}