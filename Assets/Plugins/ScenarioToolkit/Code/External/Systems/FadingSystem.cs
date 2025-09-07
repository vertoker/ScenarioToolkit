using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.External.Components.Actions.Player;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace ScenarioToolkit.External.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class FadingSystem : BaseScenarioSystem
    {
        public FadingSystem(SignalBus listener) : base(listener)
        {
            listener.Subscribe<FadeIn>(FadeIn);
            listener.Subscribe<FadeOut>(FadeOut);
            listener.Subscribe<FadeInOverTime>(FadeInOverTime);
            listener.Subscribe<FadeOutOverTime>(FadeOutOverTime);
        }

        private void FadeOutOverTime(FadeOutOverTime obj)
        {
            // if (!fader) return;
            // fader.DoFadeOut(obj.Duration);
        }

        private void FadeInOverTime(FadeInOverTime obj)
        {
            // if (!fader) return;
            // fader.DoFadeIn(obj.Duration);
        }

        private void PlayerChanged()
        {
            // if (fader)
            // {
            //     fader.FadeCompleted -= ResetFaderSpeed;
            // }
            //
            // fader = playersContainer.CurrentValue.View != null ? playersContainer.CurrentValue.View.ScreenFader : null;
            //
            // if (fader)
            // {
            //     fader.FadeCompleted += ResetFaderSpeed;
            // }
        }

        private void ResetFaderSpeed()
        {
            // if (!fader) return;
            // fader.ResetSpeed();
        }
        
        private void FadeIn(FadeIn component)
        {
            // if (!fader) return;
            // fader.CanvasGroup.alpha = 0f;
            // fader.FadeInSpeed = component.FadeInSpeed;
            // fader.DoFadeIn();
        }
        private void FadeOut(FadeOut component)
        {
            // if (!fader) return;
            // fader.CanvasGroup.alpha = 1f;
            // fader.FadeOutSpeed = component.FadeOutSpeed;
            // fader.DoFadeOut();
        }
    }
}