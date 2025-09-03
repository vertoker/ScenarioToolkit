using System;
using NaughtyAttributes;
using UnityEngine;
using VRF.Utils.Colliders;

namespace VRF.Utils.Rendering
{
    public class RenderTrigger : ColliderProvider
    {
        [SerializeField] private bool invertRender = false;
        [SerializeField, ReadOnly] private Renderer[] renderers = Array.Empty<Renderer>();
        
        private void OnEnable()
        {
            TriggerEnter += RenderEnter;
            TriggerExit += RenderExit;
            SetActiveRenderers(invertRender); // false
        }
        private void OnDisable()
        {
            TriggerEnter -= RenderEnter;
            TriggerExit -= RenderExit;
            SetActiveRenderers(!invertRender); // true
        }

        private void RenderEnter(Collider other)
        {
            SetActiveRenderers(!invertRender); // true
        }
        private void RenderExit(Collider other)
        {
            SetActiveRenderers(invertRender); // false
        }

        public void UpdateRenderers()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }
        public void SetActiveRenderers(bool active)
        {
            foreach (var rend in renderers)
            {
                rend.forceRenderingOff = !active;
            }
        }
    }
}