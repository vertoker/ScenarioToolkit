using System;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Scenario.Core.Serialization.Converters.Base;
using Scenario.Core.World;
using Scenario.Utilities;
using Scenario.Utilities.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scenario.Core.Serialization.Converters
{
    public class ScenarioGameObjectConverter : BaseScenarioJsonConverter<GameObject>
    {
        [CanBeNull]
        public readonly ScenarioSceneProvider Provider = Object.FindFirstObjectByType<ScenarioSceneProvider>(); // This is shit
        
        public override void Set(ScenarioSerializationSettings settings)
        {
            base.Set(settings);
            
            if (!Provider)
            {
                if (Settings.LogErrors)
                    Debug.LogWarning($"[W] Can't find any {nameof(ScenarioSceneProvider)} on scene");
                return;
            }
        }

        public override void WriteJson(JsonWriter writer, GameObject value, JsonSerializer serializer)
        {
            if (!Provider)
            {
                if (Settings.LogErrors)
                    Debug.LogError($"[W] No ScenarioBehavioursProvider. Write null for object <b>{value.name}</b>");
                writer.WriteNull();
                return;
            }
            
            if (!value)
            {
                writer.WriteNull();
                return;
            }

            if (!value.TryGetComponent<ScenarioBehaviour>(out var scenarioBehaviour))
            {
                scenarioBehaviour = value.AddComponent<ScenarioBehaviour>();
                Provider.CacheScene();
#if UNITY_EDITOR
                EditorUtility.SetDirty(scenarioBehaviour);
                EditorUtility.SetDirty(Provider);
#endif
                if (Settings.LogMessages)
                    Debug.Log($"[W] No ScenarioBehaviour found. Created new one: <b>{scenarioBehaviour.GetID()}</b>");
            }
            
            writer.WriteValue(scenarioBehaviour.GetID());
        }
        
        public override GameObject ReadJson(JsonReader reader, Type objectType, 
            GameObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (!Provider)
            {
                if (Settings.LogErrors)
                    Debug.LogError($"[W] No ScenarioBehavioursProvider. Read null object");
                return null;
            }
            if (reader.AssertNotNull(objectType, Settings.LogWarnings)) return null;
            
            var id = (string)reader.Value;

            var gameObject = Provider.Get(id);
            if (!gameObject)
            {
                if (Settings.LogWarnings)
                    Debug.LogWarning($"[R] Can't find {nameof(ScenarioBehaviour)} " + 
                                     $"with ID <b>{id}</b> in JSON at path <b>{reader.Path}</b>");
                return null;
            }

            return gameObject;
        }
    }
}