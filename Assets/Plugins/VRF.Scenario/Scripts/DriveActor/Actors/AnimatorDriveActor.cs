using UnityEngine;
using VRF.Scenario.DriveActor.Core;

namespace VRF.Scenario.DriveActor.Actors
{
    public class AnimatorDriveActor : BaseDriveActor
    {
        [SerializeField] private Animator animator;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }
        protected override void UpdateValue()
        {
            // TODO закончить
        }
    }
}