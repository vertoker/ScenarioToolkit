using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR

#else
using VRF.Utilities.Exceptions;
#endif

namespace ScenarioToolkit.Shared.VRF.Utilities.Extensions
{
    public static class VrfEditorScenesExtensions
    {
        /// <summary>
        /// Проходит по всем сценам из списка, открывает их и возвращает в виде списка сцен
        /// </summary>
        public static IEnumerable<Scene> ScenesIterator(IEnumerable<string> scenes)
        {
#if UNITY_EDITOR
            foreach (var scenePath in scenes)
            {
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                yield return scene;
                EditorSceneManager.CloseScene(scene, true);
            }
#else
            throw new OnlyUnityEditorException();
#endif
        }
    }
}