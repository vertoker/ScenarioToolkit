using System;
using Newtonsoft.Json;
using Scenario.Utilities;
using Scenario.Utilities.Extensions;

namespace Scenario.Core.Serialization.Converters.Base
{
    // Inspired by https://github.com/applejag/Newtonsoft.Json-for-Unity.Converters/blob/master/Packages/Newtonsoft.Json-for-Unity.Converters/UnityConverters/Math/ColorConverter.cs
    
    public abstract class JsonPropertyConverter<T> : BaseScenarioJsonConverter<T> where T : new()
    {
        public abstract void WriteJsonProperties(JsonWriter writer, T value, JsonSerializer serializer);
        public abstract void ReadValue(ref T value, string propertyName, JsonReader reader, JsonSerializer serializer);
        
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            WriteJsonProperties(writer, value, serializer);

            writer.WriteEndObject();
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.AssertNotNull(objectType)) return GetDefault();
            
            if (reader.TokenType != JsonToken.StartObject)
                throw new JsonReaderException($"Failed to read type '{typeof(T).Name}'. Expected object start, got '{reader.TokenType}' <{reader.Value}>");
            
            reader.Read(); // to property
            
            if (existingValue is not { } value)
                value = new T();
            
            string previousName = null;
            
            while (reader.TokenType == JsonToken.PropertyName)
            {
                if (reader.Value is string name)
                {
                    if (name == previousName)
                    {
                        throw new JsonReaderException($"Failed to read type '{typeof(T).Name}'. Possible loop when reading property '{name}'");
                    }
                    
                    previousName = name;
                    ReadValue(ref value, name, reader, serializer);
                }
                else
                {
                    reader.Skip();
                }

                reader.Read();
            }

            return value;
        }

        public virtual T GetDefault() => default;
    }
}