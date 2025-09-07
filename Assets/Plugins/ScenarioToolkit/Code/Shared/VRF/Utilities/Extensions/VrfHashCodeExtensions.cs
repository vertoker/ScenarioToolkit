using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с хэш кодами ассетов
    /// </summary>
    public static class VrfHashCodeExtensions
    {
        /// <summary> Считает Asset идентификатор для Scriptable объекта </summary>
        /// <exception cref="NotImplementedException"> Если вызывается не в editor </exception>
        public static bool GetAssetHashCode<TScriptable>(this TScriptable obj, out int hashCode) where TScriptable : ScriptableObject
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(obj.GetInstanceID());
            if (string.IsNullOrEmpty(path))
                throw new NullReferenceException($"Can't find item in Assets/");

            var guid = new Guid(AssetDatabase.AssetPathToGUID(path));
            hashCode = guid.GetHashCode();
            return true;
#else
            throw new NotImplementedException($"Calculate hash code only in editor, call it only from OnValidate");
#endif
        }

        /// <summary> Считает Asset идентификатор для префаба в ассетах </summary>
        /// <exception cref="NotImplementedException"> Если вызывается не в editor </exception>
        public static bool GetAssetHashCode(this GameObject obj, out int hashCode)
        {
#if UNITY_EDITOR
            hashCode = 0;
            var path = obj.GetPath();
            if (string.IsNullOrEmpty(path)) return false;
            
            var guid = new Guid(AssetDatabase.AssetPathToGUID(path));
            hashCode = guid.GetHashCode();
            return true;
#else
            throw new NotImplementedException($"Calculate hash code only in editor, call it only from OnValidate");
#endif
        }

        /// <summary> Получает путь объекта в prefab контексте unity </summary>
        /// <exception cref="NullReferenceException">Если путь не будет найден</exception>
        public static string GetPath(this GameObject obj)
        {
#if UNITY_EDITOR
            var tr = obj.transform;
            while (tr.parent) tr = tr.parent;
            obj = tr.gameObject;
            
            var path = AssetDatabase.GetAssetPath(obj.GetInstanceID());
            if (!string.IsNullOrEmpty(path)) return path;
            
            return PrefabStageUtility.GetCurrentPrefabStage()?.assetPath;
#else
            throw new NotImplementedException($"Calculate hash code only in editor, call it only from OnValidate");
#endif
        }
        
        public static string GetPath(this ScriptableObject obj)
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(obj);
            path = path.Replace(".asset", "Appearance.asset");
            return path;
#else
            Debug.LogWarning($"Not implement {nameof(GetPath)} in runtime, use only in editor");
            return string.Empty;
#endif
        }

        public static TConfig LoadConfig<TConfig>(string path) where TConfig : ScriptableObject
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<TConfig>(path);
#else
            Debug.LogWarning($"Not implement {nameof(LoadConfig)} in runtime, use only in editor");
            return null;
#endif
        }

        public static TConfig CreateConfig<TConfig>(string path) where TConfig : ScriptableObject
        {
#if UNITY_EDITOR
            var config = ScriptableObject.CreateInstance<TConfig>();
            AssetDatabase.CreateAsset(config, path);
            EditorUtility.SetDirty(config);
            return config;
#else
            Debug.LogWarning($"Not implement {nameof(CreateConfig)} in runtime, use only in editor");
            return null;
#endif
        }

        public static TConfig CreateOrLoadPreset<TConfig>(string path) where TConfig : ScriptableObject
        {
#if UNITY_EDITOR
            var config = LoadConfig<TConfig>(path);
            return config ? config : CreateConfig<TConfig>(path);
#else
            Debug.LogWarning($"Not implement {nameof(CreateOrLoadPreset)} in runtime, use only in editor");
            return null;
#endif
        }
    }
}