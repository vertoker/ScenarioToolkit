// using ScenarioToolkit.Core.Installers;
using ScenarioToolkit.Editor.Windows.GraphEditor;
using ScenarioToolkit.Shared.VRF.Utilities.VRF;
using UnityEditor;
using UnityEngine;

namespace ScenarioToolkit.Editor.CustomEditors
{
    [CanEditMultipleObjects]
    // [CustomEditor(typeof(ScenarioLauncherInstaller))]
    public class ScenarioLauncherInstallerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            /*var installer = (ScenarioLauncherInstaller)target;

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
            }*/
        }
    }
}