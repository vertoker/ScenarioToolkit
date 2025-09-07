using Newtonsoft.Json;
using Scenario.Core.Serialization.Converters.Base;
using Scenario.Utilities;
using Scenario.Utilities.Extensions;
using UnityEngine;

namespace Scenario.Core.Serialization.Converters.Unity
{
    public class Vector2IntConverter : JsonPropertyConverter<Vector2Int>
    {
        public override void WriteJsonProperties(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
        {
            writer.WritePropertyValue(nameof(value.x), value.x);
            writer.WritePropertyValue(nameof(value.y), value.y);
        }

        public override void ReadValue(ref Vector2Int value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                case nameof(value.x): value.x = reader.ReadAsInt32() ?? 0; break;
                case nameof(value.y): value.y = reader.ReadAsInt32() ?? 0; break;
            }
        }
    }
}