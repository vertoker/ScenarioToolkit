using System;
using UnityEngine;
// ReSharper disable once CheckNamespace
namespace Scenario.Utilities
{
    // [RequireComponent(typeof(Collider))]
    public class TriggerZone : MonoBehaviour
    {
        private void Start() { }

        private void OnCollisionEnter(Collision other) => OnEntered?.Invoke(other.gameObject);
        private void OnCollisionExit(Collision other) => OnExited?.Invoke(other.gameObject);
        
        private void OnTriggerEnter(Collider other) => OnEntered?.Invoke(other.gameObject);
        private void OnTriggerExit(Collider other) => OnExited?.Invoke(other.gameObject);

        public event Action<GameObject> OnEntered;
        public event Action<GameObject> OnExited;
    }
}