using System;
using System.Linq;
using Newtonsoft.Json;
using Scenario.Core.Model;
using Scenario.Core.Serialization.Converters.Base;
using Scenario.Utilities.Extensions;
using UnityEngine;

namespace Scenario.Core.Serialization.Converters
{
    public class ScenarioResourceObjectConverter : BaseScenarioJsonConverter
    {
        private static readonly Type[] Types =
        {
            typeof(AudioClip),
            typeof(Material),
            typeof(ScriptableObject),
            typeof(Texture),
            typeof(TextAsset),
        };

        public override bool CanConvert(Type type)
        {
            return Types.Any(t => t.IsAssignableFrom(type));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new ResourceObject(value, Settings.LogWarnings));
        }

        public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
        {
            if (reader.AssertNotNull(type, Settings.LogWarnings)) return null;
            
            if (reader.Value is string resourcePath)
                return ResourceObject.GetObject(resourcePath, Settings.LogWarnings);
            
            var resourceObject = serializer.Deserialize<ResourceObject>(reader);
            return resourceObject.GetObject(Settings.LogWarnings);
            
            //Debug.LogError($"Not implemented behaviour, this point is unreachable, fix it ({nameof(ResourceObject)})");
        }
    }
}