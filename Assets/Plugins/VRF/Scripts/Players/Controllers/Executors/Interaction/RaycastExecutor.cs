using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Interaction;
using VRF.Players.Controllers.Interaction.RaycastInteractionControllers;
using VRF.Players.Raycasting;
using Zenject;

namespace VRF.Players.Controllers.Executors.Interaction
{
    public class RaycastExecutor : IModelExecutor
    {
        private readonly PhysicsRaycast raycast;
        
        private readonly ControllersContainer<IRaycastInteractionController> controllersContainer;

        private Transform targetTransform;
        private IRaycastInteractionController currentController;

        public RaycastExecutor(PhysicsRaycast raycast)
        {
            this.raycast = raycast;

            controllersContainer = new ControllersContainer<IRaycastInteractionController>();
        }

        public void Enable()
        {
            raycast.ButtonPress += Raycast_OnButtonPress;
            raycast.ButtonRelease += Raycast_OnButtonRelease;
            raycast.HoverEnd += Raycast_OnHoverEnd;
        }

        public void Disable()
        {
            raycast.ButtonPress -= Raycast_OnButtonPress;
            raycast.ButtonRelease -= Raycast_OnButtonRelease;
            raycast.HoverEnd -= Raycast_OnHoverEnd;
            
            TryRelease(targetTransform);
        }

        private void Raycast_OnButtonPress(Transform transform)
        {
            currentController = controllersContainer.GetController(transform, true);

            if (currentController != null)
            {
                targetTransform = transform;
                currentController.Press();
            }
        }

        private void Raycast_OnButtonRelease(Transform transform)
        {
            TryRelease(transform);
        }

        private void Raycast_OnHoverEnd(Transform transform)
        {
            TryRelease(transform);
        }

        private void TryRelease(Transform transform)
        {
            if (transform != targetTransform)
            {
                return;
            }
            
            currentController?.Release();
            currentController = null;
            targetTransform = null;
        }
    }
}