using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VRF.Editor.TestBase
{
    /// <summary>
    /// Базовый класс для всех тестов, в первую очередь нужен,
    /// чтобы правильно и удобно отображать множественные ошибки
    /// </summary>
    public abstract class BaseTests
    {
        private bool throwError;

        public bool ThrowError => throwError;

        /// <summary>
        /// Пишет обычный LogError, но также запоминает, что есть ошибка
        /// </summary>
        public void LogError(string message, Object context = null)
        {
            Debug.LogError(message, context);
            throwError = true;
        }
        /// <summary>
        /// Пишет обычный LogError, но также запоминает, что есть ошибка
        /// </summary>
        public void LogWarning(string message, Object context = null)
        {
            Debug.LogWarning(message, context);
        }
        /// <summary>
        /// Utility метод для вывода объекта в консоль
        /// </summary>
        public static void PrintJson(object obj)
        {
            Debug.Log(JsonUtility.ToJson(obj));
        }

        /// <summary>
        /// Вызывает ошибку если до этого в консоль выводились PrintError
        /// </summary>
        public void ThrowIfError<TException>() where TException : Exception, new()
        {
            if (throwError)
            {
                throwError = false;
                throw new TException();
            }
        }
        /// <summary>
        /// Вызывает ошибку если до этого в консоль выводились LogError
        /// </summary>
        public void ThrowIfError<TException>(TException exception) where TException : Exception
        {
            if (throwError)
            {
                throwError = false;
                throw exception;
            }
        }
        
        /// <summary>
        /// Ищет и загружает все Scriptable объекты заданного типа во всём проекте
        /// </summary>
        /// <typeparam name="TAsset"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TAsset> FindScriptables<TAsset>() where TAsset : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(TAsset).Name}");
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var assets = paths.Select(AssetDatabase.LoadAssetAtPath<TAsset>);
            return assets;
        }
        /// <summary>
        /// Ищет и загружает все компоненты со всех prefab ассетов во всём проекте
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TComponent> FindComponents<TComponent>() where TComponent : Component
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(GameObject)}");
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var prefabs = paths.Select(AssetDatabase.LoadAssetAtPath<GameObject>);

            var components = prefabs.SelectMany(p => p.GetComponentsInChildren<TComponent>());
            return components;
        }
    }
}