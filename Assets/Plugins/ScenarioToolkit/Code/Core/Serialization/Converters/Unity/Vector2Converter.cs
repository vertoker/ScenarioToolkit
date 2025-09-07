using Newtonsoft.Json;
using ScenarioToolkit.Core.Serialization.Converters.Base;
using ScenarioToolkit.Shared.Extensions;
using UnityEngine;

namespace ScenarioToolkit.Core.Serialization.Converters.Unity
{
    public class Vector2Converter : JsonPropertyConverter<Vector2>
    {
        public override void WriteJsonProperties(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WritePropertyValue(nameof(value.x), value.x);
            writer.WritePropertyValue(nameof(value.y), value.y);
        }

        public override void ReadValue(ref Vector2 value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                case nameof(value.x): value.x = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.y): value.y = reader.ReadAsFloat() ?? 0f; break;
            }
        }
    }
}