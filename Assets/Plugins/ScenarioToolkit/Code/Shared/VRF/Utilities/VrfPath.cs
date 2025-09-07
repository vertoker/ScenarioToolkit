using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities
{
    /// <summary>
    /// Репрезентует разные типы путей, которые могут быть.
    /// Для пример пускай будет путь: D:/Projects/UnityProject/Assets/Configs/Settings.asset
    /// </summary>
    public enum PathRelative
    {
        /// <summary> Пример: Configs/Settings.asset </summary>
        Assets,
        /// <summary> Пример: Assets/Configs/Settings.asset </summary>
        Project,
        /// <summary> Пример: D:/Projects/UnityProject/Assets/Configs/Settings.asset </summary>
        Absolute,
    }
    
    /// <summary>
    /// Предоставляет методы по конвертации между разными типами путей
    /// </summary>
    public static class VrfPath
    {
        public const int AssetFileExtensionLength = 6; // ".asset".length = 6
        public const int AssetsEndsFolderLength = 6; // "Assets".length = 6
        public const int AssetsBeginsFolderLength = 7; // "Assets/".length = 7
        public const string AssetsFolder = "Assets/";
        
        public static string ApplicationProjectPath
        {
            get
            {
                var path = Application.dataPath;
                return path[..^AssetsEndsFolderLength];
            }
        }

        /// <summary>
        /// Конвертирует путь из одного типа в другой
        /// </summary>
        public static string ConvertPath(string abstractPath,
            PathRelative source, PathRelative to)
        {
            var converter = GetConverter(source, to);
            return converter.Invoke(abstractPath);
        }

        public static IEnumerable<string> ConvertPaths(IEnumerable<string> abstractPaths,
            PathRelative source, PathRelative to)
        {
            var converter = GetConverter(source, to);
            return abstractPaths.Select(abstractPath => converter.Invoke(abstractPath));
        }
        
        public static Converter<string, string> GetConverter
            (PathRelative source = PathRelative.Project, PathRelative to = PathRelative.Absolute)
        {
            return source switch
            {
                PathRelative.Assets when to == PathRelative.Project => FromAssetsToProject,
                PathRelative.Assets when to == PathRelative.Absolute => FromAssetsToAbsolute,
                PathRelative.Project when to == PathRelative.Assets => FromProjectToAssets,
                PathRelative.Project when to == PathRelative.Absolute => FromProjectToAbsolute,
                PathRelative.Absolute when to == PathRelative.Assets => FromAbsoluteToAssets,
                PathRelative.Absolute when to == PathRelative.Project => FromAbsoluteToProject,
                _ => DefaultConverter
            };
        }
        
        public static Func<string, string> GetFunc
            (PathRelative source = PathRelative.Project, PathRelative to = PathRelative.Absolute)
        {
            return source switch
            {
                PathRelative.Assets when to == PathRelative.Project => FromAssetsToProject,
                PathRelative.Assets when to == PathRelative.Absolute => FromAssetsToAbsolute,
                PathRelative.Project when to == PathRelative.Assets => FromProjectToAssets,
                PathRelative.Project when to == PathRelative.Absolute => FromProjectToAbsolute,
                PathRelative.Absolute when to == PathRelative.Assets => FromAbsoluteToAssets,
                PathRelative.Absolute when to == PathRelative.Project => FromAbsoluteToProject,
                _ => DefaultConverter
            };
        }

        #region Converters
        public static string DefaultConverter(string abstractPath) => abstractPath;
        public static string FromAssetsToProject(string assetsPath)
        {
            return AssetsFolder + assetsPath;
        }
        public static string FromAssetsToAbsolute(string assetsPath)
        {
            return Application.dataPath + assetsPath;
        }
        public static string FromProjectToAssets(string projectPath)
        {
            return projectPath.Substring(AssetsBeginsFolderLength, projectPath.Length - AssetsBeginsFolderLength);
        }
        public static string FromProjectToAbsolute(string projectPath)
        {
            return ApplicationProjectPath + projectPath;
        }
        public static string FromAbsoluteToAssets(string absolutePath)
        {
            var assetsPath = Application.dataPath;
            var length = assetsPath.Length;
            return absolutePath.Substring(length, absolutePath.Length - length);
        }
        public static string FromAbsoluteToProject(string absolutePath)
        {
            var projectPath = ApplicationProjectPath;
            var length = projectPath.Length;
            return absolutePath.Substring(length, absolutePath.Length - length);
        }
        #endregion
    }
}