using Newtonsoft.Json;
using ScenarioToolkit.Core.Serialization.Converters.Base;
using ScenarioToolkit.Shared.Extensions;
using UnityEngine;

namespace ScenarioToolkit.Core.Serialization.Converters.Unity
{
    public class RectConverter : JsonPropertyConverter<Rect>
    {
        public override void WriteJsonProperties(JsonWriter writer, Rect value, JsonSerializer serializer)
        {
            writer.WritePropertyValue(nameof(value.x), value.x);
            writer.WritePropertyValue(nameof(value.y), value.y);
            writer.WritePropertyValue(nameof(value.width), value.width);
            writer.WritePropertyValue(nameof(value.height), value.height);
        }

        public override void ReadValue(ref Rect value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                case nameof(value.x): value.x = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.y): value.y = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.width): value.width = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.height): value.height = reader.ReadAsFloat() ?? 0f; break;
            }
        }
    }
}