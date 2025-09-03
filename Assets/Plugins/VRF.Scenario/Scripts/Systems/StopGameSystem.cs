using Cysharp.Threading.Tasks;
using Scenario.Core.Model;
using Scenario.Core.Player;
using Scenario.Core.Services;
using Scenario.Core.Systems;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Components;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class StopGameSystem : BaseScenarioSystem
    {
        private readonly ScenarioPlayer scenarioPlayer;
        private readonly ScreenFader fader;
        private readonly PlayerTeleport teleport;

        public StopGameSystem(SignalBus listener, [InjectOptional] PlayerVRView playerVRView,
            ScenarioPlayer scenarioPlayer) : base(listener)
        {
            this.scenarioPlayer = scenarioPlayer;
            listener.Subscribe<StopGame>(StopGame);
            
            if (!playerVRView) return;
            teleport = playerVRView.PlayerTeleport;
            fader = playerVRView.ScreenFader;
        }

        private async void StopGame(StopGame component)
        {
            scenarioPlayer.Stop();

            if (fader)
            {
                fader.FadeColor = Color.black;
                fader.FadeInSpeed = 10f;
                fader.DoFadeIn();
            }

            if (teleport)
                teleport.DisableTeleportation();

            await UniTask.WaitForSeconds(5);
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}