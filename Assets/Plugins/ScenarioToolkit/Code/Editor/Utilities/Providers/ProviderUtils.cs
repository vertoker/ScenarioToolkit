using System.Collections.Generic;
using System.IO;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEditor;

namespace ScenarioToolkit.Editor.Utilities.Providers
{
    public static class ProviderUtils
    {
        // TODO это стоит сделать динамическими путями с зависимости от названия и расположения модуля
        public const string ScenarioTemplates = "Assets/Modules/Scenario/Templates/";
        public const string ScenarioPresets = "Assets/Modules/Scenario/Editor/Presets/";
        
        public static bool SelectScenario(string title, out string filePath)
        {
            filePath = EditorUtility.OpenFilePanel(title, DirectoryFileHelper.SaveDirectory, "json");
            return !string.IsNullOrWhiteSpace(filePath);
        }
        public static bool SelectScenarioFolder(string title, out string folderPath)
        {
            folderPath = EditorUtility.OpenFolderPanel(title, DirectoryFileHelper.SaveDirectory, string.Empty);
            return !string.IsNullOrWhiteSpace(folderPath);
        }

        public static IEnumerable<string> EnumerateAll(string folderPath) 
            => Directory.EnumerateFiles(folderPath, "*.json", SearchOption.AllDirectories);
        public static IEnumerable<string> EnumerateTopOnly(string folderPath) 
            => Directory.EnumerateFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly);
        
        public static IEnumerable<string> EnumerateTemplates()
            => Directory.EnumerateFiles(VrfPath.FromProjectToAbsolute(ScenarioTemplates), "*.json", SearchOption.TopDirectoryOnly);
        public static IEnumerable<string> EnumeratePresets()
            => Directory.EnumerateFiles(VrfPath.FromProjectToAbsolute(ScenarioPresets), "*.json", SearchOption.TopDirectoryOnly);
    }
}