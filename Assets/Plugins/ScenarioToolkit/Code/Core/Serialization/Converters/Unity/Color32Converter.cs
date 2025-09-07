using Newtonsoft.Json;
using ScenarioToolkit.Core.Serialization.Converters.Base;
using ScenarioToolkit.Shared.Extensions;
using UnityEngine;

namespace ScenarioToolkit.Core.Serialization.Converters.Unity
{
    public class Color32Converter : JsonPropertyConverter<Color32>
    {
        public override void WriteJsonProperties(JsonWriter writer, Color32 value, JsonSerializer serializer)
        {
            writer.WritePropertyValue(nameof(value.r), value.r);
            writer.WritePropertyValue(nameof(value.g), value.g);
            writer.WritePropertyValue(nameof(value.b), value.b);
            writer.WritePropertyValue(nameof(value.a), value.a);
        }

        public override void ReadValue(ref Color32 value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                case nameof(value.r): value.r = reader.ReadAsByte() ?? 0; break;
                case nameof(value.g): value.g = reader.ReadAsByte() ?? 0; break;
                case nameof(value.b): value.b = reader.ReadAsByte() ?? 0; break;
                case nameof(value.a): value.a = reader.ReadAsByte() ?? 0; break;
            }
        }
    }
}