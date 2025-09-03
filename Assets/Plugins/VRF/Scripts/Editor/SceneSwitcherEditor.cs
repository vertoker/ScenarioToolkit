using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace VRF.Editor
{
    public class SceneSwitcherEditor: EditorWindow
    {
        [MenuItem("Custom/Scenes Switcher")]
        private static void OpenWindow()
        {
            var window = GetWindow<SceneSwitcherEditor>();
            window.Show();
        }

        private void OnGUI()
        {
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    var scenePath = scene.path;
                    var sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                    if (GUILayout.Button(sceneName))
                    {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }
            }
        }
    }
}