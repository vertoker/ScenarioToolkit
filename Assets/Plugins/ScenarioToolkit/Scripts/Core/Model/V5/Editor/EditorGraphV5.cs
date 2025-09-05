using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;

// Previous: EditorGraphV3
//  Current: EditorGraphV5
//     Next: EditorGraphV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EditorGraphV5
    {
        public List<EditorNodeV3> Nodes { get; set; } = new();
        public List<EditorLinkV3> Links { get; set; } = new();
        public List<EditorGroupV3> Groups { get; set; } = new();
    }
}