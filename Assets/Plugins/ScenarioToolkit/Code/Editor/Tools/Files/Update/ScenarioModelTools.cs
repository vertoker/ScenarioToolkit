using System.IO;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Serialization;
using ScenarioToolkit.Editor.Utilities.Providers;
using UnityEngine;

namespace ScenarioToolkit.Editor.Tools.Files.Update
{
    public static class ScenarioModelTools
    {
        //[MenuItem("Tools/Scenario Files/Update Scenarios Separately/Model (File)", false, 13)]
        public static void UpdateNamespacesFile()
        {
            const string title = "Select file to update model...";
            if (!ProviderUtils.SelectScenario(title, out var filePath)) return;
            
            var json = File.ReadAllText(filePath);
            UpdateModelByTool(ref json, filePath);
            File.WriteAllText(filePath, json);
        }
        //[MenuItem("Tools/Scenario Files/Update Scenarios Separately/Models (All Folder)", false, 14)]
        public static void UpdateNamespacesFolder()
        {
            const string title = "Select folder to update models...";
            if (!ProviderUtils.SelectScenarioFolder(title, out var folderPath)) return;
            
            foreach (var filePath in ProviderUtils.EnumerateAll(folderPath))
            {
                Debug.Log($"<b>File</b>: {filePath}");
                var json = File.ReadAllText(filePath);
                UpdateModelByTool(ref json, filePath);
                File.WriteAllText(filePath, json);
            }
        }
        
        private static void UpdateModelByTool(ref string json, string absolutePath)
        {
            var oldType = ScenarioLoadService.GetModelType(json);
            var fileName = new FileInfo(absolutePath).Name;
            UpdateModel(ref json, null, null, fileName, out _);
            var newType = ScenarioLoadService.GetModelType(json);
            Debug.Log($"Convert {fileName} from {oldType.Name} to {newType.Name}");
        }
        public static void UpdateModel(ref string json, 
            ScenarioSerializationService serialization, 
            ScenarioLoadService loadService, 
            string scenarioName, out IScenarioModel newModel)
        {
            serialization ??= new ScenarioSerializationService();
            loadService ??= new ScenarioLoadService(serialization);

            newModel = loadService.LoadModelFromJson(json, true);
            json = serialization.Serialize(newModel, IScenarioModel.GetModelType);
            Debug.Log($"Tools: Update model for {scenarioName}");
        }
    }
}