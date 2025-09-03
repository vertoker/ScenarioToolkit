using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Utilities
{
    [ExecuteAlways]
    public class GOMultiEnabler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> gameObjects = new();

        public List<GameObject> GameObjects
        {
            get => gameObjects;
            set => gameObjects = value;
        }

        private void OnEnable()
        {
            foreach (var go in gameObjects)
                go.SetActive(true);
        }

        private void OnDisable()
        {
            foreach (var go in gameObjects)
                go.SetActive(false);
        }
    }
}