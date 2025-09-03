using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Scenario.Interfaces;

namespace VRF.Scenario.MonoBehaviours
{
    public class AGS5 : GrabbableEvents, ITriggerable
    {
        [Header("Snap Zone")]
        [SerializeField] private SnapZone snapZone;

        [Header("Audio")]
        [SerializeField] private AudioSource removingSafetyCapAudioSource;

        private bool canExtinguish;
        private void Start() => snapZone.OnDetachEvent.AddListener(DetachedSafetyCap);

        private void DetachedSafetyCap(Grabbable arg0)
        {
            removingSafetyCapAudioSource.Play();
            canExtinguish = true;
        }

        public bool TriggerState => canExtinguish;
    }
}