using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: EditorNodeV1
//  Current: EditorNodeV3
//     Next: EditorNodeV6

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [JsonObject(IsReference = true)]
    public class EditorNodeV3
    {
        public ScenarioNodeV1 Node { get; set; }
        public Vector2 Position { get; set; }
    }
}