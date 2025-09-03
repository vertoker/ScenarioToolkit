using System;
using NaughtyAttributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Utilities
{
    [ExecuteAlways]
    public class ColliderDisabler : MonoBehaviour
    {
        [SerializeField] private Collider[] colliders;

        public Collider[] Colliders
        {
            get => colliders;
            set => colliders = value;
        }

        private void OnValidate()
        {
            if(TryGetComponent(out Collider collider))
                colliders = new[] {collider};
        }

        private void OnEnable()
        {
            if (colliders == null) 
                return;
            foreach (var collider in colliders)
                collider.enabled = true;
        }

        private void OnDisable()
        {
            if (colliders == null) 
                return;
            foreach (var collider in colliders)
                collider.enabled = false;
        }

        [Button]
        private void AddSelfColliders()
        {
            colliders = GetComponents<Collider>();
        }
    }
}