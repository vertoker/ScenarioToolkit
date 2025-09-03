using UnityEngine;

namespace VRF.Players.Controllers.Models.Interfaces
{
    public interface IComponentValidator<in TComponent> where TComponent : Component
    {
        public void Validate(TComponent component);
    }
}