using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Executors.Utility;
using VRF.Players.Controllers.Interaction;
using VRF.Players.Controllers.Interaction.VirtualHandInteractionControllers;
using VRF.Players.Controllers.Models;
using VRF.Players.Services;
using VRF.Players.Services.Settings;
using Zenject;

namespace VRF.Players.Controllers.Executors.Interaction
{
    public class VirtualHandExecutor : BaseModelExecutor<VirtualHandModel>,
        IInitializable, IDisposable, ITickable, ILateTickable
    {
        private readonly PlayerWASDView view;
        private readonly CursorHideoutService hideoutService;
        private readonly Grabber grabber;
        private readonly Transform grabberPivot;
        private readonly Transform cameraPivot;
        
        private readonly SphereCollider grabberTrigger;
        private readonly InputAction grabAction;
        private readonly InputAction zoomAction;
        private readonly InputAction rotationModeAction;
        private readonly InputAction forwardRotationAction;

        private readonly MouseSensitivityParameter mouseSensitivity;
        
        private readonly GrabbableParametersSpoofer spoofer;

        private readonly ControllersContainer<IVirtualHandInteractionController> controllersContainer;
        
        private IVirtualHandInteractionController currentController;
        
        private Vector3 targetPosition;
        private Quaternion targetLocalRotation;
        private float targetDistance;
        private bool activeZoom = true;
        private bool rotationMode;
        private float forwardRotation;

        public VirtualHandExecutor(PlayerWASDView view, VirtualHandModel model, CursorHideoutService hideoutService,
            InputAction grabAction, InputAction zoomAction, InputAction rotationModeAction, InputAction forwardRotationAction,
            MouseSensitivityParameter mouseSensitivity) : base(model)
        {
            this.view = view;
            this.hideoutService = hideoutService;
            grabber = view.VirtualHandGrabber;
            grabberPivot = grabber.transform;
            cameraPivot = view.CameraPivot;
            
            grabberTrigger = grabber.GetComponent<SphereCollider>();
            this.grabAction = grabAction;
            this.zoomAction = zoomAction;
            this.rotationModeAction = rotationModeAction;
            this.forwardRotationAction = forwardRotationAction;

            this.mouseSensitivity = mouseSensitivity; 

            spoofer = new GrabbableParametersSpoofer(Model);

            controllersContainer = new ControllersContainer<IVirtualHandInteractionController>(view);
        }

        #region Initialization
        public override void Enable()
        {
            grabAction.Enable();
            zoomAction.Enable();
            rotationModeAction.Enable();
            forwardRotationAction.Enable();
            base.Enable();
        }
        public override void Disable()
        {
            base.Disable();
            grabAction.Disable();
            zoomAction.Disable();
            rotationModeAction.Disable();
            forwardRotationAction.Disable();
            DisableInternal();
        }
        
        public void Initialize()
        {
            grabberTrigger.radius = Model.GrabberRadius;
            
            targetPosition = GetTargetPosition(out targetDistance);
            grabberPivot.position = targetPosition;

            grabber.onGrabEvent.AddListener(TryMoveHandGraphics);
            grabber.onGrabEvent.AddListener(spoofer.Enable);
            grabber.onReleaseEvent.AddListener(spoofer.Disable);
            zoomAction.performed += DisableGrabberZoom;
            zoomAction.canceled += EnableGrabberZoom;
            rotationModeAction.performed += EnableRotationMode;
            rotationModeAction.canceled += DisableRotationMode;
            forwardRotationAction.performed += EnableForwardRotation;
            forwardRotationAction.canceled += DisableForwardRotation;
            hideoutService.OnHide += OnHide;
            hideoutService.OnShow += OnShow;
            
            if (hideoutService.IsShow) OnHide();
        }

        public void Dispose()
        {
            OnShow();
            
            hideoutService.OnHide -= OnHide;
            hideoutService.OnShow -= OnShow;
            zoomAction.performed -= DisableGrabberZoom;
            zoomAction.canceled -= EnableGrabberZoom;
            rotationModeAction.performed -= EnableRotationMode;
            rotationModeAction.canceled -= DisableRotationMode;
            forwardRotationAction.performed -= EnableForwardRotation;
            forwardRotationAction.canceled -= DisableForwardRotation;
            grabber.onGrabEvent.RemoveListener(TryMoveHandGraphics);
            grabber.onGrabEvent.RemoveListener(spoofer.Enable);
            grabber.onReleaseEvent.RemoveListener(spoofer.Disable);
        }
        #endregion

        #region Input
        private void OnHide()
        {
            grabAction.performed += ToggleGrab;
        }
        private void OnShow()
        {
            grabAction.performed -= ToggleGrab;
        }
        
        private void EnableGrabberZoom(InputAction.CallbackContext ctx) => activeZoom = true;
        private void DisableGrabberZoom(InputAction.CallbackContext ctx) => activeZoom = false;
        private void EnableRotationMode(InputAction.CallbackContext ctx) => rotationMode = true;
        private void DisableRotationMode(InputAction.CallbackContext ctx) => rotationMode = false;
        private void EnableForwardRotation(InputAction.CallbackContext ctx) => forwardRotation = forwardRotationAction.ReadValue<float>();
        private void DisableForwardRotation(InputAction.CallbackContext ctx) => forwardRotation = 0;
        
        private void ToggleGrab(InputAction.CallbackContext ctx)
        {
            if(grabber.HoldingItem)
                DisableInternal();
            else if(grabber.TryGrab())
                EnableInternal();
        }
        #endregion

        #region Executable

        private void EnableInternal()
        {
            grabber.ForceGrab = true;
            
            currentController = controllersContainer.GetController(grabber.HeldGrabbable);
        }
        
        private void DisableInternal()
        {
            grabber.TryRelease();
            grabber.ForceGrab = false;
            currentController = null;
        }
        
        public void Tick()
        {
            if (!Enabled) return;

            if (grabber.ForceGrab && grabber.HeldGrabbable == null)
            {
                DisableInternal();
            }

            if (currentController != null)
            {
                targetPosition = currentController.GetPosition();
                targetLocalRotation = Quaternion.Inverse(grabberPivot.parent.rotation) * currentController.GetRotation();
            }
            else if (grabber.HoldingItem)
            {
                if (activeZoom)
                {
                    var scroll = Mouse.current.scroll.ReadValue().y;

                    targetDistance = scroll switch
                    {
                        > 0 => Mathf.Min(targetDistance + Model.ZoomDelta, Model.MaxDistance),
                        < 0 => Mathf.Max(targetDistance - Model.ZoomDelta, Model.MinDistance),
                        _ => targetDistance
                    };
                }

                var cameraForward = cameraPivot.forward;
                var ray = new Ray(cameraPivot.position, cameraForward);
                targetPosition = ray.GetPoint(targetDistance);
                
                var forward = grabberPivot.InverseTransformVector(cameraForward);
                var zRotation = Quaternion.AngleAxis(-forwardRotation * Model.ItemForwardRotationSpeed * Time.deltaTime, forward);

                targetLocalRotation *= zRotation;
                
                if (rotationMode)
                {
                    var rotationSpeedCorrection = Model.ItemMouseRotationSpeed * Time.deltaTime;
                    var delta = Vector2.ClampMagnitude(Mouse.current.delta.value, Model.MaxMouseDeltaMagnitude);
                    var right = grabberPivot.InverseTransformVector(cameraPivot.right);
                    var up = grabberPivot.InverseTransformVector(cameraPivot.up);
                    var yRotation = Quaternion.AngleAxis(-delta.x * mouseSensitivity.Value.x * rotationSpeedCorrection, up);
                    var xRotation = Quaternion.AngleAxis(delta.y * mouseSensitivity.Value.y * rotationSpeedCorrection, right);

                    targetLocalRotation *= yRotation * xRotation;
                }
            }
            else
            {
                targetPosition = GetTargetPosition(out targetDistance);
                targetLocalRotation = Quaternion.identity;
            }
        }
        public void LateTick()
        {
            if (!Enabled) return;
            grabberPivot.position = Vector3.LerpUnclamped(grabberPivot.position, targetPosition, Model.LerpPoint);
            grabberPivot.localRotation = targetLocalRotation;
        }
        
        private void TryMoveHandGraphics(Grabbable grabbable)
        {
            if (grabbable.GrabPoints.Count == 0)
            {
                grabber.HandsGraphics.position = grabbable.transform.position;
            }
        }

        private Vector3 GetTargetPosition(out float distance)
        {
            var ray = new Ray(cameraPivot.position, cameraPivot.forward);
            distance = Model.MaxDistance;

            if (Model.ImmutableDistance)
                return Model.GetPositionByRay(ray);

            if (Physics.Raycast(ray, out var hit, Model.MaxDistance, Model.Mask, Model.TriggerInteraction))
            {
                distance = hit.distance;
                return hit.point;
            }
            
            return Model.GetPositionByRay(ray);
        }
        #endregion
    }
}