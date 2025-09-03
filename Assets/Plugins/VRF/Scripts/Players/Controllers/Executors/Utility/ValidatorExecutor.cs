using UnityEngine;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Executors.Utility
{
    public class ValidatorExecutor<TModel, TComponent> : BaseModelHandler<TModel>, IModelExecutor
        where TModel : IComponentValidator<TComponent>
        where TComponent : Component
    {
        private TComponent Component { get; }

        public ValidatorExecutor(TModel model, TComponent component) : base(model)
        {
            Component = component;
        }

        public void Enable()
        {
            Validate();
        }
        public void Disable()
        {
            
        }

        public void Validate()
        {
            Model.Validate(Component);
        }
    }
}