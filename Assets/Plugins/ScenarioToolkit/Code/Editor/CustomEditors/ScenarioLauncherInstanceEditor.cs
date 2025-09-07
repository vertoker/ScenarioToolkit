using Scenario.Core.World;
using Scenario.Editor.Windows.GraphEditor;
using UnityEditor;
using UnityEngine;

namespace Scenario.Editor.CustomEditors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScenarioLauncherInstance))]
    public class ScenarioLauncherInstanceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // TODO добавить наследственность от ScenarioBehaviourEditor и
            // TODO нормально отрисовывать ID, не ломая остального
            base.OnInspectorGUI();
            
            var instance = (ScenarioLauncherInstance)target;
            
            var module = instance.GetModule();
            if (module && module.ScenarioAsset && GUILayout.Button("Open In Editor"))
                GraphEditorWindow.OpenWindow(module.ScenarioAsset, instance);

            // Нужно для невозможности запуска на Client если мультиплеер работает
            var netValid = !instance.GetNetValid();
            
            if (instance.CanPlay && netValid && GUILayout.Button("Play")) instance.Play();
            if (instance.CanStop && netValid && GUILayout.Button("Stop")) instance.Stop();
        }
    }
}