using System.Collections.Generic;
using System.IO;
using ScenarioToolkit.Core.DataSource;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Editor.Utilities;
using ScenarioToolkit.Editor.Utilities.Providers;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEditor;
using UnityEngine;

namespace ScenarioToolkit.Editor.Tools.Files
{
    public static class ScenarioModuleTools
    {
        [MenuItem("Tools/Scenario Files/Generate Module (File)", false, 6)]
        public static void GenerateModule()
        {
            const string title = "Select file to generate module...";
            if (!ProviderUtils.SelectScenario(title, out var filePath)) return;
            
            GenerateModule(filePath);
            AssetDatabase.Refresh();
        }
        [MenuItem("Tools/Scenario Files/Generate Modules (Top Folder)", false, 6)]
        public static void GenerateModules()
        {
            const string title = "Select folder to generate modules...";
            if (!ProviderUtils.SelectScenarioFolder(title, out var folderPath)) return;

            Dictionary<string, ScenarioModule> cache = new();
            foreach (var filePath in ProviderUtils.EnumerateTopOnly(folderPath))
            {
                Debug.Log($"<b>File</b>: {filePath}");
                GenerateModule(filePath, cache);
            }
            AssetDatabase.Refresh();
        }
        
        private static void GenerateModule(string savePath, 
            Dictionary<string, ScenarioModule> cache = null)
        {
            var jsonPath = VrfPath.FromAbsoluteToProject(savePath);
            var identifier = DirectoryFileHelper.GetFileName(savePath).Replace(' ', '-');
            
            var modulesPath = savePath.Replace(DirectoryFileHelper.SaveDirectoryLocal,
                DirectoryFileHelper.ModulesDirectoryLocal);
            var modulesDirectory = new DirectoryInfo(modulesPath).Parent?.FullName;
            DirectoryFileHelper.ValidateDirectory(modulesDirectory);

            if (!TryFindInCache(identifier, cache, out var module))
            {
                module = ValidateModule(modulesPath);
                cache?.Add(jsonPath, module);
            }
            var mode = DetectMode(savePath);
            
            //var json = File.ReadAllText(savePath);
            //var assetPath = AssetDatabase.GetAssetPath(assetScenario);

            var assetScenario = module.ScenarioAsset;
            if (!assetScenario)
            {
                assetScenario = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
                module.SetScenario(assetScenario);
                module.SetIdentifier(identifier);
                module.SetMode(mode);
                EditorUtility.SetDirty(module);
            }
        }

        private static bool TryFindInCache(string identifier, 
            Dictionary<string, ScenarioModule> cache, out ScenarioModule module)
        {
            if (cache == null)
            {
                module = null;
                return false;
            }

            return cache.TryGetValue(identifier, out module);
        }
        private static ScenarioModule ValidateModule(string modulesPath)
        {
            var projectModulePath = VrfPath.FromAbsoluteToProject(modulesPath);
            projectModulePath = projectModulePath.Replace(".json", ".asset");
            var module = AssetDatabase.LoadAssetAtPath<ScenarioModule>(projectModulePath);

            if (!File.Exists(modulesPath))
            {
                module = ScriptableObject.CreateInstance<ScenarioModule>();
                AssetDatabase.CreateAsset(module, projectModulePath);
                EditorUtility.SetDirty(module);
            }

            return module;
        }

        private static readonly Dictionary<ScenarioMode, string[]> ModeBinds = new()
        {
            {ScenarioMode.Exam, new [] { "exam" }},
        };
        private static ScenarioMode DetectMode(string modulesPath)
        {
            var lowerPath = modulesPath.ToLower();

            foreach (var bind in ModeBinds)
            {
                foreach (var item in bind.Value)
                {
                    if (lowerPath.Contains(item)) return bind.Key;
                    //if (lowerJson.Contains(item)) return bind.Key;
                }
            }
            
            return ScenarioMode.Study;
        }
    }
}