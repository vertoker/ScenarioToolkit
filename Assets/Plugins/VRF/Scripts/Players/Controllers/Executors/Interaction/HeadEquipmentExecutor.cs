using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Core;
using VRF.Players.Controllers.Executors.Interfaces;

namespace VRF.Players.Controllers.Executors.Interaction
{
    public class HeadEquipmentExecutor : IModelExecutor
    {
        private readonly PlayerWASDView view;
        private readonly InputAction equipAction;

        public HeadEquipmentExecutor(PlayerWASDView view, InputAction equipAction)
        {
            this.view = view;
            this.equipAction = equipAction;
        }

        public void Enable()
        {
            equipAction.Enable();
            equipAction.performed += EquipAction_OnPerformed;
        }

        public void Disable()
        {
            equipAction.performed -= EquipAction_OnPerformed;
            equipAction.Disable();
        }

        private void EquipAction_OnPerformed(InputAction.CallbackContext obj)
        {
            var grabber = view.VirtualHandGrabber;
            var heldGrabbable = grabber.HeldGrabbable;
            var zone = view.HeadEquipmentZone;
            
            if (heldGrabbable && CanSnap(heldGrabbable))
            {
                heldGrabbable.transform.position = zone.transform.position;
                grabber.TryRelease();
            }
            else
            {
                zone.GrabEquipped(grabber);
            }
        }

        private bool CanSnap(Grabbable grabbable)
        {
            var excludeConditions = view.HeadEquipmentZone.ExcludeConditions;
            // Check for condition exclusion (taken from SnapZone)
            if (excludeConditions is { Count: > 0 })
            {
                var matchFound = excludeConditions.Any(excludeCondition 
                    => excludeCondition.Invoke(grabbable));

                if (matchFound)
                    return false;
            }

            return true;
        }
    }
}