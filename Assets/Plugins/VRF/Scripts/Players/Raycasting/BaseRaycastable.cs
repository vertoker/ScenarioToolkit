using System;
using VRF.Utils;

namespace VRF.Players.Raycasting
{
    public abstract class BaseRaycastable : ActivatedMonoBehaviour
    {
        public event Action HoverStart;
        public event Action HoverStay;
        public event Action HoverEnd;
        public event Action ButtonPress;
        public event Action ButtonStay;
        public event Action ButtonRelease;
        
        public virtual void OnHoverStart(PhysicsRaycast raycaster)
        {
            HoverStart?.Invoke();
        }
        public virtual void OnHoverStay(PhysicsRaycast raycaster)
        {
            HoverStay?.Invoke();
        }
        public virtual void OnHoverEnd(PhysicsRaycast raycaster)
        {
            HoverEnd?.Invoke();
        }

        public virtual void OnButtonPress(PhysicsRaycast raycaster)
        {
            ButtonPress?.Invoke();
        }
        public virtual void OnButtonStay(PhysicsRaycast raycaster)
        {
            ButtonStay?.Invoke();
        }
        public virtual void OnButtonRelease(PhysicsRaycast raycaster)
        {
            ButtonRelease?.Invoke();
        }
    }
}