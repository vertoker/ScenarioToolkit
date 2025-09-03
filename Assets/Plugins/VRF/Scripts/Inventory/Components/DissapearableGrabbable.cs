using KBCore.Refs;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;

namespace VRF.Inventory.Components
{
    /// <summary> Отключает grabbable если объект был брошен
    /// (уже не нужен, так как появился параметр в инвентаре) </summary>
    [RequireComponent(typeof(Grabbable))]
    public class DissapearableGrabbable : ValidatedMonoBehaviour
    {
        [SerializeField] private bool debug;
        [SerializeField, Self] private Grabbable grabbable;

        private void OnEnable()
        {
            grabbable.OnGrabbed += OnGrabbed;
            grabbable.OnDropped += OnDropped;
        }
        private void OnDisable()
        {
            grabbable.OnDropped -= OnDropped;
            grabbable.OnGrabbed -= OnGrabbed;
        }

        private void OnGrabbed()
        {
            if (debug)
                Debug.Log($"{nameof(DissapearableGrabbable)} {nameof(OnGrabbed)} <b>{name}</b>");
        }
        private void OnDropped()
        {
            if (debug)
                Debug.Log($"{nameof(DissapearableGrabbable)} {nameof(OnDropped)} <b>{name}</b>");
            gameObject.SetActive(false);
        }
    }
}