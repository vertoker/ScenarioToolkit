using System;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VRF.Utilities.Attributes;
using VRF.Utilities.Extensions;

#if UNITY_EDITOR
using UnityEditor;
using VRF.Scenes.Project;
#endif

namespace VRF.Scenes.Scriptables
{
    [CreateAssetMenu(fileName = nameof(ClientScenesConfig), menuName = "VRF/Scenes/" + nameof(ClientScenesConfig))]
    public class ClientScenesConfig : ScriptableObject
    {
        [SerializeField, SceneReference] private string[] scenes = Array.Empty<string>();
        [SerializeField, ReadOnly] private int[] hashCodes = Array.Empty<int>();

        public string[] Scenes => scenes;
        public int[] HashCodes => hashCodes;

        public bool GetScene(int hashCode, out string scenePath)
        {
            var index = hashCodes.IndexOfEquals(hashCode);

            if (index == -1)
            {
                scenePath = string.Empty;
                return false;
            }

            scenePath = scenes[index];
            return true;
        }

        public bool Contains(string scenePath) => scenes.Contains(scenePath);
        public bool Contains(int hashCode) => hashCodes.Contains(hashCode);

        private void OnValidate()
        {
            hashCodes = scenes.Select(s => s.GetHashCode()).ToArray();
        }

#if UNITY_EDITOR
        [Button("Add all scenes from Assets/Scenes/")]
        private void AddAllScenes()
        {
            var guids = AssetDatabase.FindAssets($"t:scene", new []{"Assets/Scenes/"});
            var assets = guids.Select(AssetDatabase.GUIDToAssetPath).ToList();
            assets.AddRange(scenes);
            scenes = assets.Distinct().ToArray();
        }
        
        [Button]
        private void ValidateBuildScenes()
        {
            var buildScenes = EditorBuildSettings.scenes.ToList();

            int notFoundCounter = 0, addedCounter = 0, enabledCounter = 0;

            foreach (var scene in scenes)
            {
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene);
                if (!sceneAsset)
                {
                    notFoundCounter++;
                    continue;
                }

                var buildScene = buildScenes.FirstOrDefault(bs => bs.path == scene);
                if (buildScene == null)
                {
                    var newScene = new EditorBuildSettingsScene(scene, true);
                    buildScenes.Add(newScene);
                    addedCounter++;
                    continue;
                }

                if (!buildScene.enabled)
                {
                    buildScene.enabled = true;
                    enabledCounter++;
                    continue;
                }
            }

            var indexOfTransition = buildScenes
                .Select(bs => bs.path.Split('/').Last())
                .IndexOfEquals(ScenesService.TransitionSceneFileName);

            if (indexOfTransition == -1)
            {
                var path = FindTransitionFilePath();
                buildScenes.Add(new EditorBuildSettingsScene(path, true));
                Debug.Log($"Add <b>{ScenesService.TransitionSceneFileName}</b> scene (required)");
            }
            else if (!buildScenes[indexOfTransition].enabled)
            {
                buildScenes[indexOfTransition].enabled = true;
                Debug.Log($"Enable <b>{ScenesService.TransitionSceneFileName}</b> scene (required)");
            }

            if (addedCounter != 0 || enabledCounter != 0 || notFoundCounter != 0)
                Debug.Log($"Added = {addedCounter}, Enabled = {enabledCounter}, Not Found = {notFoundCounter}");

            EditorBuildSettings.scenes = buildScenes.ToArray();
        }
        private string FindTransitionFilePath()
        {
            var guids = AssetDatabase.FindAssets(ScenesService.TransitionSceneName);

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.Contains("VRF")) continue;
                if (!path.Contains(ScenesService.TransitionSceneFileName)) continue;

                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                if (sceneAsset) return path;
            }

            throw new FileNotFoundException();
        }
#endif
    }
}