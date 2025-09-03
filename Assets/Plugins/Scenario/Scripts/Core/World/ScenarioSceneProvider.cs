using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using Scenario.Core.Installers;
using UnityEngine;

namespace Scenario.Core.World
{
    /// <summary>
    /// Точка связи между ядром сценария и сценарными объектами на сцене unity
    /// </summary>
    public class ScenarioSceneProvider : MonoBehaviour
    {
        // Если обновить кэш во время игры, то это
        // - Точно сломает instance для мультиплеера
        // - Возможно сломает сериализацию текущих сценариев
        
        [SerializeField, ReadOnly] private SerializedDictionary<string, GameObject> behaviours = new();
        [SerializeField, ReadOnly] private SerializedDictionary<string, ScenarioLauncherInstance> instances = new();

        public IDictionary<string, GameObject> Behaviours => behaviours;
        public IDictionary<string, ScenarioLauncherInstance> ScenarioInstances => instances;

        [Button]
        [ContextMenu(nameof(CacheScene))]
        public void CacheScene()
        {
            behaviours.Clear();
            instances.Clear();
            
            foreach (var behaviour in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (behaviour is IScenarioWorldID scenarioID)
                    Add(scenarioID, behaviour.gameObject);
            }
        }
        public void Add(IScenarioWorldID scenarioID, GameObject obj)
        {
            var id = scenarioID.GetID();
            if (behaviours.ContainsKey(id))
            {
                Debug.LogError($"Scenario Behaviour with id=<b>{id}</b> already in scene provider", obj);
                return;
            }
            
            behaviours.Add(id, obj);
            if (obj.TryGetComponent<ScenarioLauncherInstance>(out var instance))
                instances.Add(id, instance);
        }
        public void Remove(IScenarioWorldID scenarioID)
        {
            var id = scenarioID.GetID();
            if (!behaviours.ContainsKey(id))
            {
                Debug.LogError($"Scenario Behaviour with id=<b>{id}</b> is not in scene provider");
                return;
            }
            
            behaviours.Remove(id);
            instances.Remove(id);
        }

        public GameObject Get(string id) => behaviours.GetValueOrDefault(id);
        public ScenarioLauncherInstance GetScenarioInstance(string id) => instances.GetValueOrDefault(id);
    }
}