using NaughtyAttributes;
using UnityEngine;

namespace VRF.Scenes.Scene
{
    [System.Serializable]
    public class SceneLoaderParameters
    {
        public enum SceneTarget
        {
            Next,
            Current,
            NextToCurrent,
            Specified
        }
        
        [SerializeField] private bool loadOnStart;
        [SerializeField] private SceneTarget sceneMode = SceneTarget.Next;
        [SerializeField, Scene, ShowIf(nameof(ShowSpecifiedScene))] private string sceneName;
        
        private bool ShowSpecifiedScene => sceneMode == SceneTarget.Specified;
        
        public bool LoadOnStart => loadOnStart;
        public SceneTarget SceneMode => sceneMode;
        public string SceneName => sceneName;
    }
}