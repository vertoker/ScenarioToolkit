using Newtonsoft.Json;
using Scenario.Core.Serialization.Converters.Base;
using UnityEngine;

namespace Scenario.Core.Serialization.Converters.Unity
{
    public class BoundsConverter : JsonPropertyConverter<Bounds>
    {
        public override void WriteJsonProperties(JsonWriter writer, Bounds value, JsonSerializer serializer)
        {
            writer.WritePropertyName(nameof(value.center));
            serializer.Serialize(writer, value.center);
            writer.WritePropertyName(nameof(value.extents));
            serializer.Serialize(writer, value.extents);
        }

        public override void ReadValue(ref Bounds value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                //case "center.x": value.center.x = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.center): reader.Read(); 
                    value.center = serializer.Deserialize<Vector3>(reader); break;
                case nameof(value.extents): reader.Read();
                    value.extents = serializer.Deserialize<Vector3>(reader); break;
            }
        }
    }
}