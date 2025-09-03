using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Models;
using Zenject;

namespace VRF.Players.Controllers.Executors.Interaction
{
    public class ZoomExecutor : BaseModelExecutor<ZoomModel>, IInitializable, ITickable, IDisposable
    {
        private readonly InputAction zoomAction;
        private readonly Camera cam;

        private bool activeZoom;
        private int targetZoom = 1;
        private float currentZoom = 1;
        
        public ZoomExecutor(ZoomModel model, Camera cam, InputAction zoomAction) : base(model)
        {
            this.cam = cam;
            this.zoomAction = zoomAction;
        }

        public override void Enable()
        {
            base.Enable();
            zoomAction.Enable();
        }
        public override void Disable()
        {
            base.Disable();
            zoomAction.Disable();
        }

        public void Initialize()
        {
            zoomAction.performed += OnZoomPerformed;
            zoomAction.canceled += OnZoomCancelled;
        }
        public void Dispose()
        {
            zoomAction.performed -= OnZoomPerformed;
            zoomAction.canceled -= OnZoomCancelled;
        }
        
        private void OnZoomPerformed(InputAction.CallbackContext context)
        {
            activeZoom = true;
            targetZoom = Model.MinZoom;
        }
        private void OnZoomCancelled(InputAction.CallbackContext context)
        {
            activeZoom = false;
            targetZoom = 1;
        }
        
        public void Tick()
        {
            if (Enabled && cam)
            {
                currentZoom = Mathf.Lerp(currentZoom, targetZoom, Model.LerpZoom);
                cam.fieldOfView = Model.DefaultFOV - Model.EachZoomFOV * currentZoom;

                if (activeZoom)
                {
                    var scroll = Mouse.current.scroll.ReadValue().y;

                    targetZoom = scroll switch
                    {
                        > 0 => Mathf.Min(targetZoom + 1, Model.MaxZoom),
                        < 0 => Mathf.Max(targetZoom - 1, Model.MinZoom),
                        _ => targetZoom
                    };
                }
            }
        }
    }
}