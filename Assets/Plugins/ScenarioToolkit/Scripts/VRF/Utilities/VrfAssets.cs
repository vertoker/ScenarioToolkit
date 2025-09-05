using System.Collections.Generic;
using VRF.Utilities.Extensions;
using Object = UnityEngine.Object;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#else
using VRF.Utilities.Exceptions;
#endif

namespace VRF.Utilities
{
    /// <summary>
    /// Класс, предоставляющий методы по работе с ассетами
    /// </summary>
    public static class VrfAssets
    {
        public const string FilterSearchNot = "*.*";
        public const string AssetExtension = ".asset";
        public const string MetaExtension = ".meta";

        /// <summary>
        /// Получить ассет через путь
        /// </summary>
        /// <param name="abstractPath">Путь до ассета</param>
        /// <param name="relative">В каком формате предоставляется abstractPath</param>
        /// <returns></returns>
        public static Object GetObjectByPath(string abstractPath, PathRelative relative = PathRelative.Project)
        {
#if UNITY_EDITOR
            abstractPath = relative switch
            {
                PathRelative.Absolute => VrfPath.ConvertPath(abstractPath, PathRelative.Absolute,
                    PathRelative.Project),
                PathRelative.Assets => VrfPath.ConvertPath(abstractPath, PathRelative.Assets,
                    PathRelative.Project),
                _ => abstractPath
            };
            
            return AssetDatabase.LoadAssetAtPath<Object>(abstractPath);
#else
            throw new OnlyUnityEditorException();
#endif
        }
        
        /// <summary>
        /// Получить GUID'ы всех ассетов по пути
        /// </summary>
        /// <param name="abstractPath">Путь до ассета</param>
        /// <param name="relative">В каком формате предоставляется abstractPath</param>
        /// <returns></returns>
        public static IEnumerable<string> LoadGUIDs(string abstractPath, PathRelative relative = PathRelative.Project)
        {
#if UNITY_EDITOR
            var root = GetObjectByPath(abstractPath, relative);
            var objects = root.FindAllAssets<Object>();
            return LoadGUIDs(objects);
#else
            throw new OnlyUnityEditorException();
#endif
        }
        
        /// <summary>
        /// Возвращает GUID'ы всех ассетов в исходном списке
        /// </summary>
        /// <param name="objects">Список ассетов</param>
        /// <returns>Список GUID'ов для исходного списка</returns>
        public static IEnumerable<string> LoadGUIDs(this IEnumerable<Object> objects)
        {
#if UNITY_EDITOR
            return objects.Select(obj => AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
#else
            throw new OnlyUnityEditorException();
#endif
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Получить ассет через импортер
        /// </summary>
        /// <param name="importer"></param>
        /// <typeparam name="TAsset"></typeparam>
        /// <returns>Ассет либо null</returns>
        public static TAsset GetFromImporterToAsset<TAsset>(AssetImporter importer) where TAsset : Object
        {
            var asset = GetFromImporterToAsset(importer);
            if (asset is TAsset result) 
                return result;
            return null;
        }
#endif
        
#if UNITY_EDITOR
        /// <summary>
        /// Получить импортер через ассет
        /// </summary>
        /// <param name="asset"></param>
        /// <typeparam name="TImporter"></typeparam>
        /// <returns>Импортер ассетов либо null</returns>
        public static TImporter GetFromAssetToImporter<TImporter>(Object asset) where TImporter : AssetImporter
        {
            var importer = GetFromAssetToImporter(asset);
            if (importer is TImporter result) 
                return result;
            return null;
        }
#endif
        
#if UNITY_EDITOR
        /// <summary>
        /// Получить ассет через импортер
        /// </summary>
        public static Object GetFromImporterToAsset(AssetImporter importer)
        {
            var assetPath = AssetDatabase.GetAssetPath(importer);
            return AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        }
#endif
        
#if UNITY_EDITOR
        /// <summary>
        /// Получить импортер через ассет
        /// </summary>
        public static AssetImporter GetFromAssetToImporter(Object asset)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset);
            return AssetImporter.GetAtPath(assetPath);
        }
#endif
        
    }
}