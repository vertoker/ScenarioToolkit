using System.Collections.Generic;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: EditorNodeV3
//  Current: EditorNodeV6
//     Next: 

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [JsonObject(IsReference = true)]
    public class EditorNodeV6 : IEditorNode
    {
        public IScenarioNode Node { get; set; }
        public Vector2 Position { get; set; }
        
        public int Hash => Node.Hash;

        public HashSet<int> IncomingLinks { get; set; } = new();
        public HashSet<int> OutcomingLinks { get; set; } = new();
        public HashSet<int> Groups { get; set; } = new();
        
        public void ClearAll()
        {
            IncomingLinks.Clear();
            OutcomingLinks.Clear();
            Groups.Clear();
        }
    }
}