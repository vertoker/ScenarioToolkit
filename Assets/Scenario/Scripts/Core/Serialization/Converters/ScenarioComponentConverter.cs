using System;
using System.Linq;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Serialization.Converters.Base;
using Scenario.Utilities;
using Scenario.Utilities.Extensions;

namespace Scenario.Core.Serialization.Converters
{
    public class ScenarioComponentConverter : BaseScenarioJsonConverter<IScenarioComponent>
    {
        public override void WriteJson(JsonWriter writer, IScenarioComponent value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();
            
            // Content from component
            var fields = value.GetComponentFields();
            var length = fields.Length;

            // serialize type
            writer.WritePropertyName(ScenarioTypeParser.TypePropertyName);
            serializer.Serialize(writer, value.GetType(), typeof(Type));
            //writer.WriteValue(typeParser.Serialize(type));
            
            // serialize fields
            for (var i = 0; i < length; i++)
            {
                writer.WritePropertyName(fields[i].Name);
                var fieldValue = fields[i].GetValue(value);
                serializer.Serialize(writer, fieldValue);
            }
            
            writer.WriteEndObject();
        }
        
        public override IScenarioComponent ReadJson(JsonReader reader, Type objectType, 
            IScenarioComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.AssertNotNull(objectType, Settings.LogWarnings)) return null;
            if (reader.AssertTokenIs(JsonToken.StartObject, Settings.LogWarnings)) return null;
            
            reader.Read(); // to type property
            reader.Read(); // to type value
            
            if (reader.AssertTokenIs(JsonToken.String)) return null;
            
            var type = serializer.Deserialize<Type>(reader);
            if (!typeof(IScenarioComponent).IsAssignableFrom(type)) return null;
            
            var componentObject = Activator.CreateInstance(type);
            if (componentObject is not IScenarioComponent component) return null;
            
            var fields = component.GetComponentFields();
            
            reader.Read(); // to component content (points to property)

            while (reader.TokenType == JsonToken.PropertyName)
            {
                if (reader.AssertTokenIs(JsonToken.PropertyName)) return null;
                var propertyName = (string)reader.Value;
                var field = fields.FirstOrDefault(m => m.Name == propertyName);
                
                reader.Read(); // to content value
                if (field != null)
                {
                    var value = serializer.Deserialize(reader, field.FieldType);
                    component.SetValueByField(field, value);
                }
                
                reader.Read(); // to content property or EndObject
            }
            
            return component;
        }
    }
}