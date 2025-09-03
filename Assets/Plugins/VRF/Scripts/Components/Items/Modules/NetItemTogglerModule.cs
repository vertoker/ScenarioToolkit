using System;
using Mirror;
using UnityEngine;
using VRF.Networking.Core.Client;
using VRF.Networking.Messages;
using Zenject;

namespace VRF.Components.Items.Modules
{
    public class NetItemTogglerModule : BaseModule
    {
        [SerializeField] private Behaviour[] behaviours = Array.Empty<Behaviour>();
        [Inject] private ClientItemsService service;
        
        private void OnEnable()
        {
            service.OnToggleBehaviourItem += Receive;
            service.OnToggleBehavioursItem += Receive;
        }
        private void OnDisable()
        {
            service.OnToggleBehaviourItem -= Receive;
            service.OnToggleBehavioursItem -= Receive;
        }
        
        public void SendToggle(bool active)
        {
            NetworkClient.Send(new NetItemToggleBehaviours_Message
            {
                AssetHashCode = View.AssetHashCode,
                RuntimeHashCode = View.RuntimeHashCode,
                Active = active,
            });
        }
        public void SendToggle(bool active, int behaviourIndex)
        {
            NetworkClient.Send(new NetItemToggleBehaviour_Message
            {
                AssetHashCode = View.AssetHashCode,
                RuntimeHashCode = View.RuntimeHashCode,
                BehaviourIndex = behaviourIndex,
                Active = active,
            });
        }
        
        private void Receive(NetItemToggleBehaviour_Message msg)
        {
            if (msg.AssetHashCode != View.AssetHashCode) return;
            if (msg.RuntimeHashCode == View.RuntimeHashCode) return;

            if (msg.BehaviourIndex < 0 && msg.BehaviourIndex >= behaviours.Length)
            {
                Debug.LogError($"Index {msg.BehaviourIndex} is out of bounds, abort", gameObject);
                return;
            }

            behaviours[msg.BehaviourIndex].enabled = msg.Active;
        }
        private void Receive(NetItemToggleBehaviours_Message msg)
        {
            if (msg.AssetHashCode != View.AssetHashCode) return;
            if (msg.RuntimeHashCode == View.RuntimeHashCode) return;

            foreach (var behaviour in behaviours)
                behaviour.enabled = msg.Active;
        }
    }
}