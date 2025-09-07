#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace ScenarioToolkit.Shared.VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с ассетами (получение путей, папок, поиск ассетов/импортеров)
    /// </summary>
    public static class VrfAssetsExtensions
    {
        public static string GetProjectRelativePath(this Object obj)
        {
            return AssetDatabase.GetAssetPath(obj);
        }
        
        public static string GetAssetsRelativePath(this Object obj)
        {
            var relative = obj.GetProjectRelativePath();
            return relative.Substring(VrfPath.AssetsBeginsFolderLength, relative.Length - VrfPath.AssetsBeginsFolderLength);
        }
        
        public static string GetAbsolutePath(this Object obj)
        {
            var relative = obj.GetProjectRelativePath();
            return VrfPath.ApplicationProjectPath + relative;
        }
        
        public static bool IsDirectory(this Object obj) => 
            File.GetAttributes(obj.GetAbsolutePath()).HasFlag(FileAttributes.Directory);

        public static string GetDirectory(this Object obj)
        {
            var absolutePath = obj.GetAbsolutePath();
            var attributes = File.GetAttributes(absolutePath);
            return attributes.HasFlag(FileAttributes.Directory) ? absolutePath : Path.GetDirectoryName(absolutePath);
        }
        
        public static Object GetDirectoryObject(this Object obj)
        {
            var absolutePath = GetDirectory(obj);
            var relativePath = VrfPath.ConvertPath(absolutePath, PathRelative.Absolute, PathRelative.Project);
            return AssetDatabase.LoadAssetAtPath<Object>(relativePath);
        }
        
        public static bool TryGetParentDirectory(this Object obj, out string directory)
        {
            directory = obj.GetAbsolutePath();
            var attributes = File.GetAttributes(directory);
            return attributes.HasFlag(FileAttributes.Directory);
        }

        public static List<string> GetParentFolders(this IEnumerable<Object> objs)
        {
            return objs
                .Select(obj => obj.GetAbsolutePath())
                .Select(absolutePath => Directory.Exists(absolutePath) 
                    ? absolutePath : Path.GetDirectoryName(absolutePath)).ToList();
        }

        #region FindAll
        public static List<TAsset> FindAllAssets<TAsset>(this Object root, 
            SearchOption searchOption = SearchOption.TopDirectoryOnly) where TAsset : Object
        {
            var list = new List<TAsset>();
            FindAllInternal(root, list, FindAsset, searchOption);
            return list;
        }
        
        public static List<TAsset> FindAllAssets<TAsset>(this IEnumerable<Object> roots, 
            SearchOption searchOption = SearchOption.TopDirectoryOnly) where TAsset : Object
        {
            var list = new List<TAsset>();
            foreach (var root in roots)
                FindAllInternal(root, list, FindAsset, searchOption);
            return list;
        }
        
        public static void FindAllAssets<TAsset>(this Object root, List<TAsset> list,
            SearchOption searchOption = SearchOption.TopDirectoryOnly) where TAsset : Object
        {
            list.Clear();
            FindAllInternal(root, list, FindAsset, searchOption);
        }
        
        public static void FindAllAssets<TAsset>(this IEnumerable<Object> roots, List<TAsset> list, 
            SearchOption searchOption = SearchOption.TopDirectoryOnly) where TAsset : Object
        {
            list.Clear();
            foreach (var root in roots)
                FindAllInternal(root, list, FindAsset, searchOption);
        }

        public static List<TImporter> FindAllImporters<TImporter>(this Object root, 
            SearchOption searchOption = SearchOption.TopDirectoryOnly) where TImporter : AssetImporter
        {
            var list = new List<TImporter>();
            FindAllInternal(root, list, FindImporter, searchOption);
            return list;
        }
        
        public static List<TImporter> FindAllImporters<TImporter>(this IEnumerable<Object> roots, 
            SearchOption searchOption = SearchOption.TopDirectoryOnly) where TImporter : AssetImporter
        {
            var list = new List<TImporter>();
            foreach (var root in roots)
                FindAllInternal(root, list, FindImporter, searchOption);
            return list;
        }
        
        public static void FindAllImporters<TImporter>(this Object root, List<TImporter> list, 
            SearchOption searchOption = SearchOption.TopDirectoryOnly) where TImporter : AssetImporter
        {
            list.Clear();
            FindAllInternal(root, list, FindImporter, searchOption);
        }
        
        public static void FindAllImporters<TImporter>(this IEnumerable<Object> roots, List<TImporter> list, 
            SearchOption searchOption = SearchOption.TopDirectoryOnly) where TImporter : AssetImporter
        {
            list.Clear();
            foreach (var root in roots)
                FindAllInternal(root, list, FindImporter, searchOption);
        }
        
        private static void FindAllInternal<TAsset>(Object root, ICollection<TAsset> result, Func<Object, Object> search,
            SearchOption searchOption = SearchOption.TopDirectoryOnly) where TAsset : Object
        {
            var absolutePath = root.GetAbsolutePath();
            var attributes = File.GetAttributes(absolutePath);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                var converterAbsoluteToProject =
                    VrfPath.GetFunc(PathRelative.Absolute, PathRelative.Project);
            
                if (!Directory.Exists(absolutePath)) return;

                var paths = Directory
                    .GetFiles(absolutePath, VrfAssets.FilterSearchNot, searchOption)
                    .Where(s => !s.EndsWith(VrfAssets.MetaExtension))
                    .Select(converterAbsoluteToProject);
                
                foreach (var path in paths)
                {
                    var obj = search.Invoke(AssetDatabase.LoadAssetAtPath<Object>(path));
                    if (obj is TAsset asset)
                        result.Add(asset);
                }
            }
            else
            {
                var obj = search.Invoke(root);
                if (obj is TAsset asset)
                    result.Add(asset);
            }
        }
        
        private static Object FindAsset(Object obj) => obj;
        
        private static Object FindImporter(Object obj)
        {
            var assetPath = AssetDatabase.GetAssetPath(obj);
            return assetPath == null ? null : AssetImporter.GetAtPath(assetPath);
        }
        #endregion
    }
}
#endif