using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRF.Scenes.Scriptables;
using Zenject;

namespace VRF.Scenes.Project
{
    /// <summary>
    /// Главный сервис, который загружает сцены по вызову.
    /// Важно, чтобы ВСЕ запросы по загрузке сцены проходило через него,
    /// особенно это критично при сетевой загрузке сцены
    /// </summary>
    public class ScenesService
    {
        public const string BootstrapSceneName = "Bootstrap";
        public const string TransitionSceneName = "Transition";
        public const string BootstrapSceneFileName = "Bootstrap.unity";
        public const string TransitionSceneFileName = "Transition.unity";
        
        private bool inLoading;
        private string targetScene;

        private bool debug;
        public void SetDebug(bool newDebug) => debug = newDebug;

        public ClientScenesConfig ClientScenes { get; }

        public UnityEngine.SceneManagement.Scene CurrentScene { get; private set; }
        public int SceneIndex { get; private set; }
        public bool IsValidSceneIndex() => SceneIndex != -1;

        public event Action<string> OnSceneLoaded;
        public event Action OnSceneUpdated;
        
        public ScenesService([InjectOptional] ClientScenesConfig clientScenes)
        {
            ClientScenes = clientScenes;
            ValidateScenes();
            
            UpdateCurrentScene(SceneManager.GetActiveScene());
        }

        private static void ValidateScenes()
        {
#if UNITY_EDITOR
            var scenes = UnityEditor.EditorBuildSettings.scenes;
            var assets = scenes
                .Select(scene => UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(scene.path))
                .ToArray();

            //if (!assets.FirstOrDefault(s => s.name == VRFScenes.BootstrapSceneName)) // TODO derpecated
            //    Debug.LogError($"Couldn't find scene {nameof(VRFScenes.BootstrapSceneName)} in build scenes");
            if (!assets.FirstOrDefault(s => s.name == TransitionSceneName))
                Debug.LogError($"Couldn't find scene <b>{TransitionSceneName}</b> in build scenes");
#endif
        }

        public void LoadScene(string sceneName)
        {
            if (IsLoading()) return;
            LoadSceneInternalViaTransition(sceneName);
        }

        public void LoadNextScene()
        {
            if (IsLoading()) return;

            if (!FindNextClientScene(out var nextScene))
            {
                Debug.LogWarning("Couldn't find next scene, abort method");
                return;
            }

            LoadSceneInternalViaTransition(nextScene);
        }
        
        private void LoadSceneInternalViaTransition(string sceneName)
        {
            if (debug) Debug.Log($"{nameof(LoadSceneInternalViaTransition)} - {sceneName}");
            
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            SceneManager.sceneLoaded += OnTransitionLoaded;
            targetScene = sceneName;
            SceneManager.LoadSceneAsync(TransitionSceneName);
        }

        private void OnTransitionLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            if (debug) Debug.Log($"{nameof(OnTransitionLoaded)} - {scene.name} {TransitionSceneName}");
            
            if (scene.name != TransitionSceneName) return;

            SceneManager.sceneLoaded -= OnTransitionLoaded;
            SceneManager.sceneLoaded += OnCurrentSceneLoaded;
            SceneManager.LoadSceneAsync(targetScene);
        }

        private void OnCurrentSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            if (debug) Debug.Log($"{nameof(OnCurrentSceneLoaded)} - {scene.name} {targetScene}");

            if (scene.name != targetScene && scene.path != targetScene) return;
            
            SceneManager.sceneLoaded -= OnCurrentSceneLoaded;

            UpdateCurrentScene(scene);
            OnSceneLoaded?.Invoke(targetScene);
            OnSceneUpdated?.Invoke();

            ResetLoading();
        }

        private bool FindNextClientScene(out string nextScene)
        {
            var scenes = ClientScenes.Scenes;
            var currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == BootstrapSceneName && scenes.Length != 0)
            {
                nextScene = scenes[0];
                return true;
            }
            
            nextScene = scenes.FirstOrDefault(s => s == currentScene);
            return !string.IsNullOrEmpty(nextScene);
        }

        private void UpdateCurrentScene(UnityEngine.SceneManagement.Scene scene)
        {
            SceneIndex = scene.path.GetHashCode();
            CurrentScene = scene;
        }
        private bool IsLoading()
        {
            if (inLoading)
            {
                Debug.LogWarning("Scene in loading, abort method");
                return true;
            }

            if (debug) Debug.Log($"{nameof(IsLoading)}");
            
            inLoading = true;
            return false;
        }
        private void ResetLoading()
        {
            if (debug) Debug.Log($"{nameof(ResetLoading)}");
            
            inLoading = false;
        }
    }
}