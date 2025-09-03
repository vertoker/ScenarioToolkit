using System;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace VRF.Utils.Rendering
{
    public class RenderListInstaller : MonoInstaller
    {
        [SerializeField, ReadOnly] private RenderTrigger[] triggers = Array.Empty<RenderTrigger>();
        
        public override void InstallBindings()
        {
            foreach (var trigger in triggers)
            {
                trigger.UpdateRenderers();
                trigger.SetActiveRenderers(false);
            }
        }

        private void OnValidate()
        {
            triggers = FindObjectsByType<RenderTrigger>(FindObjectsSortMode.None);
        }
    }
}