using System;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    /// <summary>
    /// Объект, который позволяет сериализовывать путь до объекта в ресурсах.
    /// Подробнее в ScenarioResourceObjectConverter
    /// </summary>
    public readonly struct ResourceObject
    {
        [JsonProperty] public readonly string ResourcePath;

        private const string ResourcesDir = "Resources/";
        
        public ResourceObject(object source, bool log = true)
        {
#if UNITY_EDITOR
            if (source is not Object resource)
            {
                Debug.LogWarning($"[W] Can't write resource, it's not an Object, <b>{source}</b>");
                ResourcePath = null;
                return;
            }
            
            var path = AssetDatabase.GetAssetPath(resource);
            if (path == null)
            {
                if (log)
                    Debug.LogWarning($"[W] Can't find object in Assets/, <b>{source}</b>");
                ResourcePath = null;
                return;
            }

            var resourcesIndex = path.IndexOf(ResourcesDir, StringComparison.InvariantCulture);
            if (resourcesIndex < 0)
            {
                if (log)
                    Debug.LogWarning($"[W] Can't find object in Resources/, <b>{source}</b>");
                ResourcePath = null;
                return;
            }

            path = path.Substring(resourcesIndex + ResourcesDir.Length);
            var filename = Path.GetFileNameWithoutExtension(path);
            var directory = Path.GetDirectoryName(path);
            ResourcePath = Path.Join(directory, filename);
#else
            ResourcePath = null;
            Debug.LogError("[W] Saving new resource object is not implemented, write null");
#endif
        }

        public Object GetObject(bool log = true)
        {
            return GetObject(ResourcePath, log);
        }
        public static Object GetObject(string resourcePath, bool log = true)
        {
            if (string.IsNullOrEmpty(resourcePath))
            {
                if (log)
                    Debug.LogWarning($"[R] Resource path is null or empty");
                return null;
            }

            var obj = Resources.Load(resourcePath);
            
            if (!obj)
            {
                if (log)
                    Debug.LogWarning($"[R] Can't load resource object by path <b>{resourcePath}</b>");
                return null;
            }

            return obj;
        }
    }
}