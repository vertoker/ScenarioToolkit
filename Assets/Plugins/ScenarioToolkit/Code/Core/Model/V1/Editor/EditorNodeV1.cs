using System;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;

// Previous: SerializableNodeElement
//  Current: EditorNodeV1
//     Next: EditorNodeV3

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [JsonObject(IsReference = true)]
    public class EditorNodeV1
    {
        public ScenarioNodeV1 Node { get; set; }
        public Type ContentType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}