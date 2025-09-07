using ScenarioToolkit.Core.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenarioToolkit.Editor.Tools
{
    public static class ScenarioMenuItems
    {
        [MenuItem("GameObject/Scenario/Create Launcher Instance", false, 10)]
        public static void CreateLauncherInstance(MenuCommand menuCommand)
        {
            var root = new GameObject("LauncherInstance").AddComponent<ScenarioLauncherInstance>();
            Selection.activeGameObject = root.gameObject;
            
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}