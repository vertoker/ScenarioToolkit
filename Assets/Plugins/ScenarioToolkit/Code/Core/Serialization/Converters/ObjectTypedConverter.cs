using System;
using Newtonsoft.Json;
using Scenario.Core.Model;
using Scenario.Core.Serialization.Converters.Base;
using Scenario.Utilities;
using Scenario.Utilities.Extensions;

namespace Scenario.Core.Serialization.Converters
{
    public class ObjectTypedConverter : BaseScenarioJsonConverter<ObjectTyped>
    {
        public override void WriteJson(JsonWriter writer, ObjectTyped value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName(ScenarioTypeParser.TypePropertyName);
            serializer.Serialize(writer, value.Type, typeof(Type));
            
            writer.WritePropertyName(nameof(value.Object));
            serializer.Serialize(writer, value.Object, value.Type);
            
            writer.WriteEndObject();
        }

        public override ObjectTyped ReadJson(JsonReader reader, Type objectType, 
            ObjectTyped existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.AssertNotNull(objectType, Settings.LogWarnings)) return ObjectTyped.Empty;
            if (reader.AssertTokenIs(JsonToken.StartObject, Settings.LogWarnings)) return ObjectTyped.Empty;
            
            reader.Read(); // to type property
            reader.Read(); // to type value
            
            if (reader.AssertTokenIs(JsonToken.String, Settings.LogWarnings)) return ObjectTyped.Empty;
            var type = serializer.Deserialize<Type>(reader);

            if (type == TypesReflection.SerializationNullType)
                return ObjectTyped.Empty; // Если тип null - значит и value = null
            
            reader.Read(); // to object property
            reader.Read(); // to object value
            
            var obj = serializer.Deserialize(reader, type);
            
            reader.Read(); // to EndObject
            
            // Тип всегда может быть null, ставка идёт на правильное сохранение NotNull типов
            return ObjectTyped.ConstructNull(obj, type);
        }
    }
}