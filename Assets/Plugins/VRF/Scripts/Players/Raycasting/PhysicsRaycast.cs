using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Utilities;

namespace VRF.Players.Raycasting
{
    public class PhysicsRaycast : MonoBehaviour
    {
        [SerializeField] private bool logEvents = false;
        
        [Header("Core")]
        [SerializeField] private InputActionReference actionReference;
        [FormerlySerializedAs("_lineRenderer")]
        [SerializeField] private LineRenderer lineRenderer;
        [FormerlySerializedAs("_neighbourGrabber")] 
        [SerializeField] private Grabber neighbourGrabber;
        
        [Header("Raycast")]
        [SerializeField] private bool rayEnabled = true;
        [FormerlySerializedAs("_maxDistance")] 
        [SerializeField] private float maxDistance = 3;
        [FormerlySerializedAs("_mask")] 
        [SerializeField] private LayerMask mask = VrfLayerMask.EverythingMask;
        [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal;
        
        [Header("Debug")]
        [SerializeField, ReadOnly] private Transform currentTarget;
        [SerializeField, ReadOnly] private BaseRaycastable currentRaycastable;
        
        [ShowNativeProperty] public bool IsPressed => button != null && button.IsPressed() && !neighbourGrabber.HoldingItem;
        [ShowNativeProperty] public bool IsHovered => currentRaycastable != null;
        [ShowNativeProperty] public bool IsShowRay => IsPressed && rayEnabled;
        [ShowNativeProperty] public bool IsHoveredAndPressed => IsPressed && IsHovered;

        public Transform CurrentTarget => currentTarget;
        public BaseRaycastable CurrentRaycastable => currentRaycastable;

        public event Action<Transform> HoverStart;
        public event Action<Transform> HoverStay;
        public event Action<Transform> HoverEnd;

        public event Action<Transform> ButtonPress;
        public event Action<Transform> ButtonStay;
        public event Action<Transform> ButtonRelease;

        private Vector3[] linePoints;
        private InputAction button;
        
        private void OnEnable()
        {
            linePoints = new[] { Vector3.zero, Vector3.zero };
            button = actionReference.action;

            if (button != null)
            {
                button.performed += OnButtonPress;
                button.canceled += OnButtonRelease;
                button.Enable();
            }

            if (logEvents)
            {
                HoverStart += t => Debug.Log($"HoverStart {t.name}");
                HoverStay += t => Debug.Log($"HoverStay {t.name}");
                HoverEnd += t => Debug.Log($"HoverEnd {t.name}");

                ButtonPress += t => Debug.Log($"ButtonPress {t.name}");
                ButtonStay += t => Debug.Log($"ButtonStay {t.name}");
                ButtonRelease += t => Debug.Log($"ButtonRelease {t.name}");
            }
        }
        private void OnDisable()
        {
            if (button != null)
            {
                button.Disable();
                button.performed -= OnButtonPress;
                button.canceled -= OnButtonRelease;
            }
        }

        private void OnButtonPress(InputAction.CallbackContext ctx)
        {
            if (currentTarget) ButtonPress?.Invoke(currentTarget);
            if (IsHovered) currentRaycastable.OnButtonPress(this);
        }
        private void OnButtonRelease(InputAction.CallbackContext ctx)
        {
            if (currentTarget) ButtonRelease?.Invoke(currentTarget);
            if (IsHovered) currentRaycastable.OnButtonRelease(this);
        }

        private void Update()
        {
            var isHit = GetRaycast(out var hit);
            var distance = isHit ? hit.distance : maxDistance;
            
            UpdateHovered(isHit ? hit.transform : null);
            UpdatePressed();
            
            UpdateLineRenderer(distance);
        }

        private bool GetRaycast(out RaycastHit hit)
        {
            var raySource = transform;
            var ray = new Ray(raySource.position, raySource.forward);
            return Physics.Raycast(ray, out hit, maxDistance, mask, triggerInteraction);
        }

        private void UpdateLineRenderer(float distance)
        {
            if (lineRenderer)
            {
                lineRenderer.enabled = IsShowRay;
                
                if (IsShowRay)
                {
                    linePoints[1] = Vector3.forward * distance;
                    lineRenderer.SetPositions(linePoints);
                }
            }
        }

        private void UpdateHovered(Transform newTarget)
        {
            if (newTarget == currentTarget)
            {
                InvokeIfNotNull(HoverStay, currentTarget);
                if (IsHovered) currentRaycastable.OnHoverStay(this);
            }
            else
            {
                InvokeIfNotNull(HoverEnd, currentTarget);
                if (IsHovered) currentRaycastable.OnHoverEnd(this);
                
                currentRaycastable = null;
                
                if (newTarget)
                {
                    currentRaycastable = newTarget.GetComponent<BaseRaycastable>();
                    InvokeIfNotNull(HoverStart, newTarget);
                    if (IsHovered && currentRaycastable.enabled)
                        currentRaycastable.OnHoverStart(this);
                }
                
                currentTarget = newTarget;
            }
        }
        private void UpdatePressed()
        {
            if (IsPressed)
            {
                InvokeIfNotNull(ButtonStay, currentTarget);
                if (IsHovered) currentRaycastable.OnButtonStay(this);
            }
        }

        private static void InvokeIfNotNull(Action<Transform> evt, Transform t)
        {
            if (t != null)
                evt?.Invoke(t);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var raySource = transform;
            var ray = new Ray(raySource.position, raySource.forward * maxDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(ray);
        }
#endif
    }
}