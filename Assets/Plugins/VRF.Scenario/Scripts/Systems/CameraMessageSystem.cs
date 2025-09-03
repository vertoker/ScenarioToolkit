using Scenario.Core.Systems;
using SimpleUI.Core;
using VRF.BNG_Framework.Scripts.Components;
using VRF.Players.Core;
using VRF.Scenario.Components.Actions;
using VRF.UI.CameraMessage;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class CameraMessageSystem : BaseScenarioSystem
    {
        private readonly PlayersContainer playersContainer;
        private ScreensManagerInstance screensManagerInstance;
        private ScreensManager manager;
        private ScreenFader fader;

        public CameraMessageSystem(SignalBus bus, PlayersContainer playersContainer) : base(bus)
        {
            this.playersContainer = playersContainer;
            playersContainer.PlayerChanged += PlayerChanged;
            bus.Subscribe<ShowCameraMessage>(ShowMessage);
            bus.Subscribe<HideCameraMessage>(HideMessage);
        }

        private void HideMessage() =>
            manager.Find<CameraMessageScreen>().Close();
        private void ShowMessage(ShowCameraMessage obj)
        {
            manager.Find<CameraMessageScreen>().SetMessage(obj.Message);
            manager.Find<CameraMessageScreen>().Open();
        }

        private void PlayerChanged()
        {
            var currentPlayerView = playersContainer.CurrentValue.View;
            screensManagerInstance = currentPlayerView != null ? currentPlayerView.CameraUI : null;
            if (screensManagerInstance != null) manager = screensManagerInstance.Manager;
        }
    }
}