using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;

// Previous: EditorGraphV1
//  Current: EditorGraphV3
//     Next: EditorGraphV5

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EditorGraphV3
    {
        public List<EditorGroupV3> Groups { get; set; } = new();
        public List<EditorLinkV3> Links { get; set; } = new();
        public List<EditorNodeV3> Nodes { get; set; } = new();
    }
}