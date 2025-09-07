using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Scenario.Editor.Utilities;
using Scenario.Editor.Utilities.Providers;
using Scenario.Utilities;
using UnityEditor;
using UnityEngine;

namespace Scenario.Editor.Tools.Files.Update
{
    public static class ScenarioNamespaceTools
    {
        //[MenuItem("Tools/Scenario Files/Update Scenarios Separately/Namespaces (File)", false, 11)]
        public static void UpdateNamespacesFile()
        {
            const string title = "Select file to update namespaces...";
            if (!ProviderUtils.SelectScenario(title, out var filePath)) return;
            
            var text = File.ReadAllText(filePath);
            UpdateNamespaces(ref text, new FileInfo(filePath).Name);
            File.WriteAllText(filePath, text);
        }
        //[MenuItem("Tools/Scenario Files/Update Scenarios Separately/Namespaces (All Folder)", false, 12)]
        public static void UpdateNamespacesFolder()
        {
            const string title = "Select folder to update namespaces...";
            if (!ProviderUtils.SelectScenarioFolder(title, out var folderPath)) return;
            
            foreach (var filePath in ProviderUtils.EnumerateAll(folderPath))
            {
                Debug.Log($"<b>File</b>: {filePath}");
                var text = File.ReadAllText(filePath);
                UpdateNamespaces(ref text, new FileInfo(filePath).Name);
                File.WriteAllText(filePath, text);
            }
        }

        /// <summary>
        /// Костыль для нашил текущих сценариев, исправляет ошибки
        /// сериализации, которые требуют исправления
        /// </summary>
        public static void UpdateNamespaces(ref string json, string scenarioName = null)
        {
            UpdateNames_v2_19_1(ref json);
            UpdateNames_v3_0_0(ref json);
            UpdateTypeNamespaces(ref json);
            Debug.Log($"Tools: Update namespaces for {scenarioName}");
        }

        public static void UpdateNames_v2_19_1(ref string json)
        {
            const string actions = "\"Actions\": [";
            const string conditions = "\"Conditions\": [";
            const string components = "\"Components\": [";

            json = json.Replace(actions, components);
            json = json.Replace(conditions, components);
        }
        public static void UpdateNames_v3_0_0(ref string json)
        {
            const string indexedComponent1 = "Scenario.Core.Serialization.Converters.ScenarioComponentConverter+IndexedComponent, Scenario";
            const string indexedComponent2 = "Scenario.Core.Services.Converters.ScenarioComponentConverter+IndexedComponent, Scenario";
            const string newIndexedComponent = "Scenario.Core.Model.IndexedComponent, Scenario";

            json = json.Replace(indexedComponent1, newIndexedComponent);
            json = json.Replace(indexedComponent2, newIndexedComponent);
        }
        public static void UpdateTypeNamespaces(ref string json)
        {
            const RegexOptions options = RegexOptions.Multiline;
            const string pattern = @"""\$type"": ""(.*)""";
            
            var names = Regex.Matches(json, pattern, options).Select(m => m.Groups[1].Value).Distinct().ToArray();
            
            foreach (var name in names)
            {
                if (ScenarioTypeParser.TryDeserialize(name, out var type))
                {
                    //Debug.Log($"Replaced <b>{type.Name}</b> from <b>{name}</b>");
                    json = json.Replace(name, ScenarioTypeParser.Serialize(type));
                }
            }
        }
    }
}