using UnityEngine;

namespace VRF.Players.Controllers.Interaction.VirtualHandInteractionControllers
{
    public interface IVirtualHandInteractionController
    {
        public Vector3 GetPosition();
        public Quaternion GetRotation();
    }
}