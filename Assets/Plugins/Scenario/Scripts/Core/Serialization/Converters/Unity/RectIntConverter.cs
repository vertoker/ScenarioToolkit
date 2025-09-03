using Newtonsoft.Json;
using Scenario.Core.Serialization.Converters.Base;
using Scenario.Utilities.Extensions;
using UnityEngine;

namespace Scenario.Core.Serialization.Converters.Unity
{
    public class RectIntConverter : JsonPropertyConverter<RectInt>
    {
        public override void WriteJsonProperties(JsonWriter writer, RectInt value, JsonSerializer serializer)
        {
            writer.WritePropertyValue(nameof(value.x), value.x);
            writer.WritePropertyValue(nameof(value.y), value.y);
            writer.WritePropertyValue(nameof(value.width), value.width);
            writer.WritePropertyValue(nameof(value.height), value.height);
        }

        public override void ReadValue(ref RectInt value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                case nameof(value.x): value.x = reader.ReadAsInt32() ?? 0; break;
                case nameof(value.y): value.y = reader.ReadAsInt32() ?? 0; break;
                case nameof(value.width): value.width = reader.ReadAsInt32() ?? 0; break;
                case nameof(value.height): value.height = reader.ReadAsInt32() ?? 0; break;
            }
        }
    }
}