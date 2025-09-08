using System;
using System.Linq;
using ScenarioToolkit.Core.Installers.Systems;
using ScenarioToolkit.Shared;
using ScenarioToolkit.Shared.Extensions;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEditor;
using UnityEngine;

namespace ScenarioToolkit.Editor.CustomEditors
{
    /// <summary>
    /// Окно поиска для сценарных систем, отрисовывается в MonoBehaviour
    /// </summary>
    [CustomEditor(typeof(ScenarioSystemsSearch))]
    public class ScenarioSystemsSearchEditor : UnityEditor.Editor
    {
        // Step 1
        private static Type[] InstallerTypes;

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloading()
        {
            InstallerTypes = Reflection.GetImplementations<BaseSystemInstaller>().ToArray();
        }

        private string search = string.Empty;
        private const int Threshold = 50;

        private static string GetSystemName(Type type) => type.Name.Replace("Installer", "");
        
        public override void OnInspectorGUI()
        {
            var instance = (ScenarioSystemsSearch)target;

            GUILayout.Label("Search required system installer here", GUILayout.ExpandWidth(true));
            
            EditorGUILayout.BeginHorizontal();
            
            search = GUILayout.TextArea(search, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Clear", GUILayout.ExpandWidth(false)))
                search = string.Empty;
            
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(2);

            var searchPadding = search.Length < Threshold 
                ? search + new string('-', Threshold - search.Length) : search;
            
            var sortedNames = InstallerTypes
                .Where(t => !instance.GetComponentInChildren(t))
                .Where(t => !t.HasAttribute<ObsoleteAttribute>())
                .Select(Convert).OrderBy(t => t.Item1);
            
            foreach (var installer in sortedNames)
            {
                if (GUILayout.Button(installer.Item2, GUILayout.ExpandWidth(true)))
                {
                    instance.CreateSystemInstaller(installer.Item3);
                    search = string.Empty;
                }
                GUILayout.Space(2);
            }
            return;

            Tuple<int, string, Type> Convert(Type type)
            {
                var systemName = GetSystemName(type);
                var order = systemName.LevenshteinDistance(searchPadding, Threshold);
                return new Tuple<int, string, Type>(order, systemName, type);
            }
        }
    }
}