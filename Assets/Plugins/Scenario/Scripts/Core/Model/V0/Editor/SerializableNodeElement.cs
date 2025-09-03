using System;
using Newtonsoft.Json;
using Scenario.Core.Model;

// Previous: 
//  Current: SerializableNodeElement
//     Next: EditorNodeV1

// ReSharper disable once CheckNamespace
namespace Scenario.GraphEditor.Elements.Serialization
{
    [JsonObject(IsReference = true)]
    public class SerializableNodeElement
    {
        public Type ContentType;
        public ScenarioNode Node;
        public float X, Y;

        public EditorNodeV1 ConvertV1()
        {
            return new EditorNodeV1
            {
                Node = Node.ConvertV1(),
                ContentType = ContentType,
                X = X, Y = Y,
            };
        }
    }
}