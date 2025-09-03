using Scenario.Core.Installers;
using Scenario.Editor.Windows.GraphEditor;
using UnityEditor;
using UnityEngine;
using VRF.Utilities;

namespace Scenario.Editor.CustomEditors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScenarioLauncherInstaller))]
    public class ScenarioLauncherInstallerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var installer = (ScenarioLauncherInstaller)target;

            var config = installer.LaunchConfig;
            if (!config)
            {
                if (GUILayout.Button("Create Default LaunchConfig"))
                {
                    installer.LaunchConfig = ScriptableTools.CreatePresetEditor(config,
                        "Assets/Configs/Scenario/", "ScenarioLaunchConfig");
                }
            }
            else
            {
                var module = config.GetModule();
                if (module && module.ScenarioAsset && GUILayout.Button("Open In Editor"))
                    GraphEditorWindow.OpenWindow(module.ScenarioAsset, installer);
            }
        }
    }
}