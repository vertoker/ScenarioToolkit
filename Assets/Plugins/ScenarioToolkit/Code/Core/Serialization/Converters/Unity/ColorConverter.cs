using Newtonsoft.Json;
using Scenario.Core.Serialization.Converters.Base;
using Scenario.Utilities;
using Scenario.Utilities.Extensions;
using UnityEngine;

namespace Scenario.Core.Serialization.Converters.Unity
{
    public class ColorConverter : JsonPropertyConverter<Color>
    {
        public override void WriteJsonProperties(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WritePropertyValue(nameof(value.r), value.r);
            writer.WritePropertyValue(nameof(value.g), value.g);
            writer.WritePropertyValue(nameof(value.b), value.b);
            writer.WritePropertyValue(nameof(value.a), value.a);
        }

        public override void ReadValue(ref Color value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                case nameof(value.r): value.r = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.g): value.g = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.b): value.b = reader.ReadAsFloat() ?? 0f; break;
                case nameof(value.a): value.a = reader.ReadAsFloat() ?? 0f; break;
            }
        }
    }
}