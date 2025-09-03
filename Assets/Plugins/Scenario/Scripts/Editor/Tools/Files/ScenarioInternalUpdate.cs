using System.IO;
using Scenario.Core.Serialization;
using Scenario.Editor.Tools.Files.Update;
using Scenario.Editor.Utilities;
using Scenario.Editor.Utilities.Providers;
using UnityEditor;
using UnityEngine;

namespace Scenario.Editor.Tools.Files
{
    public static class ScenarioInternalUpdate
    {
        [MenuItem("Tools/Scenario Files/Scenario: Update Internals", false, 1)]
        public static void UpdateInternals()
        {
            var serialization = new ScenarioSerializationService();
            var loadService = new ScenarioLoadService(serialization);
            
            foreach (var templatePath in ProviderUtils.EnumerateTemplates())
                Update(templatePath);
            foreach (var presetsPath in ProviderUtils.EnumeratePresets())
                Update(presetsPath);
            return;

            void Update(string path)
            {
                Debug.Log($"<b>File</b>: {path}");
                var json = File.ReadAllText(path);
                var fileName = DirectoryFileHelper.GetFileName(path);
                ScenarioUpdateTools.UpdateScenario(ref json, serialization, loadService, fileName, out _);
                File.WriteAllText(path, json);
            }
        }
    }
}