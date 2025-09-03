using Zenject;

namespace VRF.Players.Controllers.Builders
{
    public interface IPlayerBuilder : ITickable, IFixedTickable, ILateTickable
    {
        public void BuilderInitialize();
        public void BuilderDispose();
    }
}