using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: EditorGroupV1
//  Current: EditorGroupV3
//     Next: EditorGroupV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EditorGroupV3
    {
        public string Name { get; set; }
        public List<EditorNodeV3> Nodes { get; set; }
        public Vector2 Position { get; set; }
    }
}