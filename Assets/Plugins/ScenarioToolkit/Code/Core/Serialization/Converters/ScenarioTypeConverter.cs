using System;
using Newtonsoft.Json;
using ScenarioToolkit.Core.Serialization.Converters.Base;
using ScenarioToolkit.Shared;
using UnityEngine;

namespace ScenarioToolkit.Core.Serialization.Converters
{
    /* Workaround for strange Newtonsoft behaviour, also helps to convert types on the fly
        https://discuss.hangfire.io/t/could-not-cast-or-convert-from-system-string-to-system-type/4240/5
    */

    public class ScenarioTypeConverter : BaseScenarioJsonConverter<Type>
    {
        public override void WriteJson(JsonWriter writer, Type value, JsonSerializer serializer)
        {
            writer.WriteValue(ScenarioTypeParser.Serialize(value));
        }
        
        public override Type ReadJson(JsonReader reader, Type objectType, Type existingValue, 
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is not string typeName)
            {
                if (Settings.LogWarnings)
                    Debug.LogWarning($"[R] Can't parse value <b>{reader.Value}</b> " +
                                     $"in JSON at path <b>{reader.Path}</b>");
                // I can't deserialize null as type, returned typeof(object) as null type
                return TypesReflection.SerializationNullType;
            }

            if (!ScenarioTypeParser.TryDeserialize(typeName, out var type))
            {
                if (Settings.LogWarnings)
                    Debug.LogWarning($"[R] Can't find type <b>{typeName}</b> in JSON at path <b>{reader.Path}</b>. " + 
                                     $"Try using Tools/Scenario/Update namespaces");
                return TypesReflection.SerializationNullType;
            }
            
            return type;
        }
    }
}