using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Executors.Base
{
    public abstract class BaseModelExecutor<TModel> : BaseModelHandler<TModel>, IModelExecutor where TModel : IPlayerModel
    {
        private bool enabledExecutor;
        
        protected bool Enabled => enabledExecutor && Model.Enabled;

        protected BaseModelExecutor(TModel model) : base(model)
        {
            
        }
        
        public virtual void Enable()
        {
            enabledExecutor = true;
        }
        public virtual void Disable()
        {
            enabledExecutor = false;
        }
    }
}