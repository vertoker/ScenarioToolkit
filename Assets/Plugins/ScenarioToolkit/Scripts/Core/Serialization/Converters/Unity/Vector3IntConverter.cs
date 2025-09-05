using Newtonsoft.Json;
using Scenario.Core.Serialization.Converters.Base;
using Scenario.Utilities;
using Scenario.Utilities.Extensions;
using UnityEngine;

namespace Scenario.Core.Serialization.Converters.Unity
{
    public class Vector3IntConverter : JsonPropertyConverter<Vector3Int>
    {
        public override void WriteJsonProperties(JsonWriter writer, Vector3Int value, JsonSerializer serializer)
        {
            writer.WritePropertyValue(nameof(value.x), value.x);
            writer.WritePropertyValue(nameof(value.y), value.y);
            writer.WritePropertyValue(nameof(value.z), value.z);
        }

        public override void ReadValue(ref Vector3Int value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            switch (propertyName)
            {
                case nameof(value.x): value.x = reader.ReadAsInt32() ?? 0; break;
                case nameof(value.y): value.y = reader.ReadAsInt32() ?? 0; break;
                case nameof(value.z): value.z = reader.ReadAsInt32() ?? 0; break;
            }
        }
    }
}