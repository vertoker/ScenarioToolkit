using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для поиска компонентов на сцене
    /// </summary>
    public static class VrfScenesExtensions
    {
        public static TComponent FindComponent<TComponent>(this Scene scene) where TComponent : class 
            => scene.GetRootGameObjects()
                .Select(obj => obj.GetComponentInChildren<TComponent>())
                .FirstOrDefault(component => component != null);
        public static IEnumerable<TComponent> FindComponents<TComponent>(this Scene scene) where TComponent : class
            => scene.GetRootGameObjects()
                .SelectMany(obj => obj.GetComponentsInChildren<TComponent>());
        
        public static TComponent FindComponent<TComponent>(this IEnumerable<Scene> scenes) where TComponent : class 
            => scenes
                .Select(scene => scene.FindComponent<TComponent>())
                .FirstOrDefault(component => component != null);

        public static IEnumerable<TComponent> FindComponents<TComponent>(this IEnumerable<Scene> scenes) where TComponent : class
            => scenes.SelectMany(scene => scene.FindComponents<TComponent>());
        
        /*public static async IEnumerable<Scene> ScenesIterator(IEnumerable<string> scenes)
        {
            foreach (var scenePath in scenes)
            {
                await SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive)!;
                yield return scenePath;
                SceneManager.UnloadSceneAsync(scene, true);
            }
        }*/
    }
}