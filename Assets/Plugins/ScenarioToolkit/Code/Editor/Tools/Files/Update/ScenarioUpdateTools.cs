using System.Collections.Generic;
using System.IO;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Serialization;
using ScenarioToolkit.Editor.Utilities.Providers;
using ScenarioToolkit.Shared.Extensions;
using UnityEditor;
using UnityEngine;
using ZLinq;

namespace ScenarioToolkit.Editor.Tools.Files.Update
{
    public static class ScenarioUpdateTools
    {
        [MenuItem("Tools/Scenario Files/Update scenario (File)", false, 5)]
        public static void UpdateFile()
        {
            const string title = "Select file to fix...";
            if (!ProviderUtils.SelectScenario(title, out var filePath)) return;
            
            var text = File.ReadAllText(filePath);
            UpdateScenarioByTool(ref text, filePath, out _);
            File.WriteAllText(filePath, text);
        }
        [MenuItem("Tools/Scenario Files/Update scenario (File Recursively)", false, 5)]
        public static void UpdateFileRecursively()
        {
            const string title = "Select file to fix...";
            if (!ProviderUtils.SelectScenario(title, out var filePath)) return;

            Queue<string> subgraphs = new();
            subgraphs.Enqueue(filePath);

            while (subgraphs.TryDequeue(out filePath))
            {
                var text = File.ReadAllText(filePath);
                UpdateScenarioByTool(ref text, filePath, out var newModel);
                File.WriteAllText(filePath, text);
                
                foreach (var subgraphNode in newModel.Graph.NodesValuesAVE.OfType<ISubgraphNode>())
                    subgraphs.Enqueue(subgraphNode.GetFullPath());
            }
        }
        [MenuItem("Tools/Scenario Files/Update scenarios (TopDirectoryOnly)", false, 5)]
        public static void UpdateFolderTopDirectoryOnly()
        {
            const string title = "Select folder to fix...";
            if (!ProviderUtils.SelectScenarioFolder(title, out var folderPath)) return;
            
            foreach (var filePath in ProviderUtils.EnumerateTopOnly(folderPath))
            {
                Debug.Log($"<b>File</b>: {filePath}");
                var text = File.ReadAllText(filePath);
                UpdateScenarioByTool(ref text, filePath, out _);
                File.WriteAllText(filePath, text);
            }
        }
        [MenuItem("Tools/Scenario Files/Update scenarios (AllDirectories)", false, 5)]
        public static void UpdateFolderAllDirectories()
        {
            const string title = "Select folder to fix...";
            if (!ProviderUtils.SelectScenarioFolder(title, out var folderPath)) return;
            
            foreach (var filePath in ProviderUtils.EnumerateAll(folderPath))
            {
                Debug.Log($"<b>File</b>: {filePath}");
                var text = File.ReadAllText(filePath);
                UpdateScenarioByTool(ref text, filePath, out _);
                File.WriteAllText(filePath, text);
            }
        }

        private static void UpdateScenarioByTool(ref string json, string absolutePath, out IScenarioModel newModel)
        {
            if (!ScenarioLoadService.CheckValidJson(json))
            {
                newModel = null;
                return;
            }
            
            var fileName = new FileInfo(absolutePath).Name;
            
            var oldType = ScenarioLoadService.GetModelType(json);
            UpdateScenario(ref json, null, null, fileName, out newModel);
            var newType = ScenarioLoadService.GetModelType(json);
            
            Debug.Log($"Convert {fileName} from {oldType.Name} to {newType.Name}");
        }
        public static void UpdateScenario(ref string json, 
            ScenarioSerializationService serialization, 
            ScenarioLoadService loadService, 
            string scenarioName, out IScenarioModel newModel)
        {
            ScenarioNamespaceTools.UpdateNamespaces(ref json, scenarioName);
            ScenarioModelTools.UpdateModel(ref json, serialization, loadService, scenarioName, out newModel);
            Debug.Log($"Tools: Update scenario {scenarioName}");
        }
    }
}