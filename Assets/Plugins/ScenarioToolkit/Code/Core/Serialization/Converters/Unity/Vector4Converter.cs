using Newtonsoft.Json;
using ScenarioToolkit.Core.Serialization.Converters.Base;
using ScenarioToolkit.Shared.Extensions;
using UnityEngine;

namespace ScenarioToolkit.Core.Serialization.Converters.Unity
{
    public class Vector4Converter : JsonPropertyConverter<Vector4>
    {
        public override void WriteJsonProperties(JsonWriter writer, Vector4 value, JsonSerializer serializer)
        {
            writer.WritePropertyValue(nameof(value.x), value.x);
            writer.WritePropertyValue(nameof(value.y), value.y);
            writer.WritePropertyValue(nameof(value.z), value.z);
            writer.WritePropertyValue(nameof(value.w), value.w);
        }

        public override void ReadValue(ref Vector4 value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                case nameof(value.x): value.x = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.y): value.y = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.z): value.z = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.w): value.w = reader.ReadAsFloat() ?? 0f; break;
            }
        }
    }
}