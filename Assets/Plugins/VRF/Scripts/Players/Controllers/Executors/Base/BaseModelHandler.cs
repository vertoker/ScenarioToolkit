namespace VRF.Players.Controllers.Executors.Base
{
    public abstract class BaseModelHandler<TModel>
    {
        protected TModel Model { get; }

        protected BaseModelHandler(TModel model)
        {
            Model = model;
        }
    }
}