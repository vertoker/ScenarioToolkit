using Newtonsoft.Json;
using Scenario.Core.Serialization.Converters.Base;
using UnityEngine;

namespace Scenario.Core.Serialization.Converters.Unity
{
    public class BoundsIntConverter : JsonPropertyConverter<BoundsInt>
    {
        public override void WriteJsonProperties(JsonWriter writer, BoundsInt value, JsonSerializer serializer)
        {
            writer.WritePropertyName(nameof(value.position));
            serializer.Serialize(writer, value.position);
            writer.WritePropertyName(nameof(value.size));
            serializer.Serialize(writer, value.size);
        }

        public override void ReadValue(ref BoundsInt value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                case nameof(value.position): reader.Read(); 
                    value.position = serializer.Deserialize<Vector3Int>(reader); break;
                case nameof(value.size): reader.Read(); 
                    value.size = serializer.Deserialize<Vector3Int>(reader); break;
            }
        }
    }
}