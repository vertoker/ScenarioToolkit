using System;
using Newtonsoft.Json;
using Scenario.Core.Model;
using ScenarioToolkit.Core.Serialization.Converters.Base;
using ScenarioToolkit.Shared.Extensions;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Services.Converters
{
    public class ScenarioUnityComponentConverter : BaseScenarioJsonConverter<Component>
    {
        public override void WriteJson(JsonWriter writer, Component value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new IndexedComponent(value, Settings.LogWarnings));
        }

        public override Component ReadJson(JsonReader reader, Type objectType,
            Component existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.AssertNotNull(objectType, Settings.LogWarnings)) return null;
            
            if (reader.ValueType == typeof(string))
                return serializer.Deserialize<GameObject>(reader).GetComponent(objectType);

            var indexed = serializer.Deserialize<IndexedComponent>(reader);
            return indexed.GetComponent(Settings.LogWarnings);
        }
    }
}