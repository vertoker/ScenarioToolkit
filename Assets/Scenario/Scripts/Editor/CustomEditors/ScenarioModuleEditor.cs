using Scenario.Core.Scriptables;
using Scenario.Editor.Windows.GraphEditor;
using UnityEditor;
using UnityEngine;

namespace Scenario.Editor.CustomEditors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScenarioModule))]
    public class ScenarioModuleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var module = (ScenarioModule)target;

            if (module.ScenarioAsset && GUILayout.Button("Open In Editor"))
                GraphEditorWindow.OpenWindow(module.ScenarioAsset);
        }
    }
}