using Newtonsoft.Json;
using Scenario.Core.Serialization.Converters.Base;
using Scenario.Utilities.Extensions;
using UnityEngine;

namespace Scenario.Core.Serialization.Converters.Unity
{
    public class Hash128Converter : JsonPropertyConverter<Hash128>
    {
        public override void WriteJsonProperties(JsonWriter writer, Hash128 value, JsonSerializer serializer)
        {
            writer.WritePropertyValue("hash", value.ToString());
        }
        public override void ReadValue(ref Hash128 value, string propertyName, JsonReader reader, JsonSerializer serializer)
        {
            if (propertyName == "hash")
                value = Hash128.Parse(reader.ReadAsString());
        }
    }
}