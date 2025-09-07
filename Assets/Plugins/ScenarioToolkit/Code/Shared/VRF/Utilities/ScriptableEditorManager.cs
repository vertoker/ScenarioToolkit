#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities
{
    /// <summary>
    /// Класс с методами поиска и создания ScriptableObject'ов
    /// </summary>
    public static class ScriptableEditorManager
    {
        /// <summary>
        /// Находит первый ScriptableObject с заданным типом или создает его
        /// </summary>
        /// <typeparam name="T">ScriptableObject</typeparam>
        /// <returns>Первый ScriptableObject указанного типа в ассетах, либо новый экземпляр</returns>
        public static T FindOrCreate<T>() where T : ScriptableObject
        {
            var config = Find<T>();
            if (config) return config;
            
            var path = GetConfigPath(typeof(T).Name);
            return Create<T>(path);
        }
        
        /// <summary>
        /// Находит первый ScriptableObject с заданным типом
        /// </summary>
        /// <typeparam name="T">ScriptableObject</typeparam>
        /// <returns>Первый ScriptableObject указанного типа в ассетах либо null, если такой не найден</returns>
        public static T Find<T>() where T : ScriptableObject
        {
            var assets = AssetDatabase.FindAssets($"t:{nameof(T)}");
            if (assets.Length == 0) return null;

            var guid = assets[0];
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            
            return asset;
        }
        
        /// <summary>
        /// Находит все ScriptableObject'ы заданного типа в ассетах
        /// </summary>
        /// <typeparam name="T">ScriptableObject</typeparam>
        /// <returns>Находит все ScriptableObject'ы заданного типа в ассетах либо пустой массив</returns>
        public static T[] FindAll<T>() where T : ScriptableObject
        {
            var assets = AssetDatabase.FindAssets($"t:{nameof(T)}");
            if (assets.Length == 0) return Array.Empty<T>();
            
            var configs = assets
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>).ToArray();
            return configs;
        }

        /// <summary>
        /// Предоставляет путь к конфигу
        /// </summary>
        /// <param name="configName">Название файла конфига</param>
        /// <returns>Путь к конфигу относительно папки проекта</returns>
        public static string GetConfigPath(string configName) => $"{AssetsFolder}{ConfigFolderPath}{configName}.asset";

        private static T Create<T>(string path) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            var dataPath = GetDataPath_AssetsCut();
            var fullPath = Path.Combine(dataPath, path);
            
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);
            AssetDatabase.CreateAsset(asset, path);
            EditorUtility.SetDirty(asset);
            return asset;
        }

        private static string GetDataPath_AssetsCut()
        {
            var dataPath = Application.dataPath;
            dataPath = dataPath.Remove(dataPath.Length - AssetsFolder.Length, AssetsFolder.Length);
            return dataPath;
        }
        private const string AssetsFolder = "Assets/";
        private const string ConfigFolderPath = "Configs/VRF/";
    }
}
#endif