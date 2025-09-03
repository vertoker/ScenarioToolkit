using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: EditorGroupV3
//  Current: EditorGroupV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EditorGroupV6 : IEditorGroup
    {
        public string Name { get; set; }
        public int Hash { get; set; }
        
        public int GetBaseHashCode() => base.GetHashCode();
        
        public HashSet<IEditorNode> Nodes { get; set; }
        public Vector2 Position { get; set; }
    }
}