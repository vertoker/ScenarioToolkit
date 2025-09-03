using System;
using KBCore.Refs;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;

namespace VRF.Components.Items.Modules
{
    public class ItemTogglerModule : BaseModule
    {
        [SerializeField] private bool invertBehaviours;
        [SerializeField] private Behaviour[] behaviours = Array.Empty<Behaviour>();
        [SerializeField, Self] private Grabbable grabbable;
        [SerializeField] private NetItemTogglerModule netModule;
        
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
            if (!netModule) return;
            
            netModule.SendToggle(!invertBehaviours);
            foreach (var behaviour in behaviours)
                behaviour.enabled = !invertBehaviours;
        }
        private void OnDropped()
        {
            if (!netModule) return;
            
            netModule.SendToggle(invertBehaviours);
            foreach (var behaviour in behaviours)
                behaviour.enabled = invertBehaviours;
        }
    }
}