using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Utilities
{
    public class DrawColliderGizmo : MonoBehaviour
    {
#if UNITY_EDITOR
        private Collider _collider;

        private void OnValidate()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);
            // Handles.Label(_collider.bounds.center + Vector3.up * _collider.bounds.size.y, gameObject.name,
            //     new GUIStyle() { alignment = TextAnchor.MiddleCenter, normal = new GUIStyleState() { textColor = Color.cyan } });
        }
#endif
    }
}