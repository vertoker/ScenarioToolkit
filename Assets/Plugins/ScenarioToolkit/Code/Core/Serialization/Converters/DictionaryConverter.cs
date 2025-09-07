using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using ScenarioToolkit.Core.Serialization.Converters.Base;

namespace ScenarioToolkit.Core.Serialization.Converters
{
    public class DictionaryConverter : BaseScenarioJsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            var dictionary = (IDictionary)value;

            writer.WriteStartArray();

            foreach (var key in dictionary.Keys)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Key");
                serializer.Serialize(writer, key);

                writer.WritePropertyName("Value");
                serializer.Serialize(writer, dictionary[key]);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //if (!CanConvert(objectType)) throw new Exception($"This converter is not for {objectType}.");

            if (reader.TokenType == JsonToken.Null) return null;

            var keyType = objectType.GetGenericArguments()[0];
            var valueType = objectType.GetGenericArguments()[1];
            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var result = (IDictionary)Activator.CreateInstance(dictionaryType);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                    return result;
                if (reader.TokenType == JsonToken.StartObject)
                    AddObjectToDictionary(reader, result, serializer, keyType, valueType);
            }

            return result;
        }
        private static void AddObjectToDictionary(JsonReader reader, IDictionary result, 
            JsonSerializer serializer, Type keyType, Type valueType)
        {
            object key = null;
            object value = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject && key != null)
                {
                    result.Add(key, value);
                    return;
                }

                var propertyName = reader.Value.ToString();
                if (propertyName == "Key")
                {
                    reader.Read();
                    key = serializer.Deserialize(reader, keyType);
                }
                else if (propertyName == "Value")
                {
                    reader.Read();
                    value = serializer.Deserialize(reader, valueType);
                }
            }
        }
        
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType 
                   && (objectType.GetGenericTypeDefinition() == typeof(IDictionary<,>) 
                       || objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>) 
                       && objectType.GetGenericArguments()[0] != typeof(string));
        }
    }
}