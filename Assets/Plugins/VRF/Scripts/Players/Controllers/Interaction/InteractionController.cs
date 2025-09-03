using System;
using UnityEngine;

namespace VRF.Players.Controllers.Interaction
{
    public abstract class InteractionController<T> : ISetupable
    {
        protected T target;
        
        public void Setup(object target)
        {
            if (target is T targetCasted)
            {
                this.target = targetCasted;
            }
            else
            {
                throw new ArgumentException($"Target is not {typeof(T)}");
            }
            
            SetupInternal();
        }
        
        protected virtual void SetupInternal() { }
    }
}