using System;
using Scenario.Utilities;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Core;
using VRF.Inventory.Equipment;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Interfaces;
using Zenject;

namespace VRF.Scenario.MonoBehaviours
{
    [RequireComponent(typeof(Collider))]
    public class SelfSaverFailable : Failable
    {
        [SerializeField] private InventoryItemConfig selfSaver;
        
        [SerializeField] private float timeTillDeath = 20f;

        private HeadEquipmentService headEquipmentService;
        
        private float remainingTime;

        [Inject]
        public void Construct(HeadEquipmentService headEquipmentService)
        {
            this.headEquipmentService = headEquipmentService;
        }
        
        private void Start()
        {
            remainingTime = timeTillDeath;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!enabled)
                return;
            
            if (!other.TryGetComponent(out BasePlayerView _) &&
                !other.TryGetComponent(out BNGPlayerController _)) 
                return;

            if (headEquipmentService.Contains(selfSaver))
                return;
            
            remainingTime -= Time.fixedDeltaTime;

            if (remainingTime <= 0)
            {
                Debug.Log("failed");
                OnExamFailed();
            }
        }
    }
}