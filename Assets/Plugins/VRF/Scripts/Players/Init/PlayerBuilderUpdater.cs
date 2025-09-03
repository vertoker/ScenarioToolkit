using VRF.Players.Core;
using Zenject;

namespace VRF.Players.Init
{
    public class PlayerBuilderUpdater : ITickable, IFixedTickable, ILateTickable
    {
        private readonly PlayersContainer container;

        public PlayerBuilderUpdater(PlayersContainer container)
        {
            this.container = container;
        }

        public void Tick()
        {
            container.CurrentValue.Builder?.Tick();
        }
        public void FixedTick()
        {
            container.CurrentValue.Builder?.FixedTick();
        }
        public void LateTick()
        {
            container.CurrentValue.Builder?.LateTick();
        }
    }
}