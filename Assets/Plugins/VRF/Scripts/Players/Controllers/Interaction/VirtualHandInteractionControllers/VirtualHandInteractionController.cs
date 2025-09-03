using UnityEngine;
using VRF.Components.Players.Views.Player;

namespace VRF.Players.Controllers.Interaction.VirtualHandInteractionControllers
{
    public abstract class VirtualHandInteractionController<T> : InteractionController<T>, IVirtualHandInteractionController
    {
        protected readonly PlayerWASDView view;

        protected VirtualHandInteractionController(PlayerWASDView view)
        {
            this.view = view;
        }
        
        public abstract Vector3 GetPosition();
        public abstract Quaternion GetRotation();
    }
}