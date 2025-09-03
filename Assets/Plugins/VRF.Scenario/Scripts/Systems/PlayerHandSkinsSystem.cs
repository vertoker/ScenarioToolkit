using Scenario.Core.Systems;
using VRF.Components.Players.Views.Player;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PlayerHandSkinsSystem : BaseScenarioSystem
    {
        private readonly PlayerVRView playerVRView;

        public PlayerHandSkinsSystem([InjectOptional] PlayerVRView playerVRView, SignalBus bus) : base(bus)
        {
            if (!playerVRView) return;
            this.playerVRView = playerVRView;
            
            bus.Subscribe<CreateNewHandSkins>(CreateNewHandSkins);
            bus.Subscribe<PopActiveHandSkins>(PopActiveHandSkins);
            bus.Subscribe<ClearInactiveHandSkins>(ClearInactiveHandSkins);
        }

        private void CreateNewHandSkins(CreateNewHandSkins component)
        {
            if (component.Left)
                playerVRView.LeftSkinsHand.CreateSkin(component.Left);
            if (component.Right)
                playerVRView.RightSkinsHand.CreateSkin(component.Right);
        }
        private void PopActiveHandSkins(PopActiveHandSkins component)
        {
            if (component.Left)
                playerVRView.LeftSkinsHand.PopSkin();
            if (component.Right)
                playerVRView.RightSkinsHand.PopSkin();
        }
        private void ClearInactiveHandSkins(ClearInactiveHandSkins component)
        {
            if (component.Left)
                playerVRView.LeftSkinsHand.ClearInactive();
            if (component.Right)
                playerVRView.RightSkinsHand.ClearInactive();
        }
    }
}