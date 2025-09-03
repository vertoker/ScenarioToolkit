using JetBrains.Annotations;
using Scenario.Base.Components.Actions;
using Scenario.Core.Systems;
using VRF.Components.Players.Views.Player;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class FixHandGraphicsSystem : BaseScenarioSystem
    {
        [CanBeNull] private readonly PlayerVRView playerVR;

        public FixHandGraphicsSystem(SignalBus bus, [InjectOptional] PlayerVRView playerVR) : base(bus)
        {
            this.playerVR = playerVR;
            Bus.Subscribe<SetGameObjectActivity>(SetGameObjectActivity);
        }

        private void SetGameObjectActivity(SetGameObjectActivity component)
        {
            //if (AssertLog.NotNull<SetGameObjectActivity>(component.GameObject, nameof(component.GameObject))) return;

            FixHandGraphics();
        }

        private void FixHandGraphics()
        {
            if (playerVR)
            {
                playerVR.LeftGrabber.TryResetHandGraphics();
                playerVR.RightGrabber.TryResetHandGraphics();
            }
        }
    }
}