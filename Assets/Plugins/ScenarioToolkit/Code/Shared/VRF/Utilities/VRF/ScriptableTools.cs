using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities.VRF
{
    /// <summary>
    /// Утилиты для создания, копирования ScriptableObject'ов из редактора
    /// </summary>
    public static class ScriptableTools
    {
        public static void CreatePresetEditor<TScriptable>(ref TScriptable scriptable,
            string projectFolder, string scriptableName) where TScriptable : ScriptableObject
        {
#if UNITY_EDITOR
            if (scriptable) return;
            
            var newPath = $"{projectFolder}{scriptableName}.asset";
            scriptable = UnityEditor.AssetDatabase.LoadAssetAtPath<TScriptable>(newPath);
            
            if (scriptable)
            {
                UnityEditor.EditorUtility.SetDirty(scriptable);
                UnityEditor.AssetDatabase.SaveAssets();
                return;
            }

            var absoluteFolder = VrfPath.ConvertPath(projectFolder, PathRelative.Project, PathRelative.Absolute);
            if (!Directory.Exists(absoluteFolder))
                Directory.CreateDirectory(absoluteFolder);
            
            scriptable = ScriptableObject.CreateInstance<TScriptable>();
            UnityEditor.AssetDatabase.CreateAsset(scriptable, newPath);

            UnityEditor.EditorUtility.SetDirty(scriptable);
            UnityEditor.AssetDatabase.SaveAssets();
#else
            throw new NotImplementedException($"Implemented only in editor");
#endif
        }
        public static TScriptable CreatePresetEditor<TScriptable>(TScriptable scriptable,
            string projectFolder, string scriptableName) where TScriptable : ScriptableObject
        {
#if UNITY_EDITOR
            if (scriptable) return scriptable;
            
            var newPath = $"{projectFolder}{scriptableName}.asset";
            scriptable = UnityEditor.AssetDatabase.LoadAssetAtPath<TScriptable>(newPath);
            
            if (scriptable)
            {
                UnityEditor.EditorUtility.SetDirty(scriptable);
                UnityEditor.AssetDatabase.SaveAssets();
                return scriptable;
            }

            var absoluteFolder = VrfPath.ConvertPath(projectFolder, PathRelative.Project, PathRelative.Absolute);
            if (!Directory.Exists(absoluteFolder))
                Directory.CreateDirectory(absoluteFolder);
            
            scriptable = ScriptableObject.CreateInstance<TScriptable>();
            UnityEditor.AssetDatabase.CreateAsset(scriptable, newPath);

            UnityEditor.EditorUtility.SetDirty(scriptable);
            UnityEditor.AssetDatabase.SaveAssets();
            return scriptable;
#else
            throw new NotImplementedException($"Implemented only in editor");
#endif
        }
        public static TScriptable CreatePresetEditor<TScriptable>(string projectFolder, 
            string scriptableName) where TScriptable : ScriptableObject
        {
#if UNITY_EDITOR
            var newPath = $"{projectFolder}{scriptableName}.asset";
            var scriptable = UnityEditor.AssetDatabase.LoadAssetAtPath<TScriptable>(newPath);
            
            if (scriptable)
            {
                UnityEditor.EditorUtility.SetDirty(scriptable);
                UnityEditor.AssetDatabase.SaveAssets();
                return scriptable;
            }

            var absoluteFolder = VrfPath.FromProjectToAbsolute(projectFolder);
            if (!Directory.Exists(absoluteFolder)) Directory.CreateDirectory(absoluteFolder);
            
            scriptable = ScriptableObject.CreateInstance<TScriptable>();
            UnityEditor.AssetDatabase.CreateAsset(scriptable, newPath);

            UnityEditor.EditorUtility.SetDirty(scriptable);
            UnityEditor.AssetDatabase.SaveAssets();
            return scriptable;
#else
            throw new NotImplementedException($"Implemented only in editor");
#endif
        }
        
        public static void CopyPresetEditor<TScriptable>(ref TScriptable scriptable,
            string sourcePath, string destinationPath) where TScriptable : ScriptableObject
        {
#if UNITY_EDITOR
            if (scriptable) return;
            
            scriptable = UnityEditor.AssetDatabase.LoadAssetAtPath<TScriptable>(destinationPath);
            
            if (scriptable)
            {
                UnityEditor.EditorUtility.SetDirty(scriptable);
                UnityEditor.AssetDatabase.SaveAssets();
                return;
            }
            
            var sourceScriptable = UnityEditor.AssetDatabase.LoadAssetAtPath<TScriptable>(sourcePath);

            if (!sourceScriptable)
                throw new FileNotFoundException($"Can't find source scriptable by path {sourcePath}");

            var absDestFile = VrfPath.FromProjectToAbsolute(destinationPath);
            var absDestFolder = Path.GetDirectoryName(absDestFile);
            
            if (absDestFolder == null)
                throw new DirectoryNotFoundException($"Empty directory name from file path {absDestFile}");
            if (!Directory.Exists(absDestFolder))
                Directory.CreateDirectory(absDestFolder);

            if (!UnityEditor.AssetDatabase.CopyAsset(sourcePath, destinationPath))
                throw new Exception($"Copy is failed, src={sourcePath}, dst={destinationPath}");
            
            scriptable = UnityEditor.AssetDatabase.LoadAssetAtPath<TScriptable>(destinationPath);
            UnityEditor.EditorUtility.SetDirty(scriptable);
            UnityEditor.AssetDatabase.SaveAssets();
#else
            throw new NotImplementedException($"Implemented only in editor");
#endif
        }
        
        public static void CopyFolderEditor(string sourcePath, string destinationPath)
        {
#if UNITY_EDITOR
            var absoluteSource = VrfPath.ConvertPath(sourcePath, PathRelative.Project, PathRelative.Absolute);
            var absoluteDestination = VrfPath.ConvertPath(destinationPath, PathRelative.Project, PathRelative.Absolute);

            Directory.CreateDirectory(absoluteDestination);
            foreach (var dirPath in Directory.GetDirectories(absoluteSource, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(absoluteSource, absoluteDestination));
            foreach (var newPath in Directory.GetFiles(absoluteSource, "*.*",SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(absoluteSource, absoluteDestination), true);

            var metaFiles = Directory.GetFiles(absoluteDestination, "*.meta", SearchOption.AllDirectories);
            var allFiles = Directory.GetFiles(absoluteDestination, "*.*", SearchOption.AllDirectories);
            var guidTable = new List<(string originalGuid, string newGuid)>();

            foreach (var metaFile in metaFiles)
            {
                var file = new StreamReader(metaFile);
                file.ReadLine();
                var guidLine = file.ReadLine();
                file.Close();
                
                var originalGuid = guidLine?.Substring(6, guidLine.Length - 6);
                var newGuid = UnityEditor.GUID.Generate().ToString().Replace("-", "");
                guidTable.Add((originalGuid, newGuid));
            }
            foreach (var fileToModify in allFiles)
            {
                var content = File.ReadAllText(fileToModify);
                foreach (var guidPair in guidTable)
                    content = content.Replace(guidPair.originalGuid, guidPair.newGuid);
                File.WriteAllText(fileToModify, content);
            }

            UnityEditor.AssetDatabase.Refresh();
#else
            throw new NotImplementedException($"Implemented only in editor");
#endif
        }
    }
}