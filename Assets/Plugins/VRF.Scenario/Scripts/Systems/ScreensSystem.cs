using System;
using Cysharp.Threading.Tasks;
using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Core.Systems.States;
using Scenario.Utilities;
using SimpleUI.Core;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Components.Conditions;
using VRF.Scenario.States;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class ScreensSystem : BaseScenarioStateSystem<ScreensState>
    {
        public ScreensSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SetScreenActivity>(SetScreenActivity);
            bus.Subscribe<NotifyScreen>(NotifyScreen);
        }

        protected override void ApplyState(ScreensState state)
        {
            foreach (var (screenBase, value) in state.ScreensActivity)
            {
                screenBase.SetActive(value, false, true);
            }
        }

        private void SetScreenActivity(SetScreenActivity component)
        {
            if (AssertLog.NotNull<SetScreenActivity>(component.Screen, nameof(component.Screen))) return;

            if (component.Active)
            {
                component.Screen.OpenStarted += OpenStarted;
                component.Screen.OpenEnded += OpenEnded;
            }
            else
            {
                component.Screen.CloseStarted += CloseStarted;
                component.Screen.CloseEnded += CloseEnded;
            }
            
            State.ScreensActivity.SetStateActivity(component.Screen, component.Screen.ScreenActive, component.Active);
            
            // delay в 1 frame нужен для работы condition внутри scenario graph,
            // так как ход сценария полностью синхронный, и условия должны быть подписаны
            // перед операцией открытия экрана
            component.Screen.SetActive(component.Active, true, true);
        }

        private async void NotifyScreen(NotifyScreen component)
        {
            if (AssertLog.NotNull<NotifyScreen>(component.Screen, nameof(component.Screen))) return;
            if (AssertLog.Above<NotifyScreen>(component.Time, 0, nameof(component.Time))) return;

            SetScreenActivity(new SetScreenActivity { Active = true, Screen = component.Screen });
            
            await UniTask.Delay(TimeSpan.FromSeconds(component.Time));
            
            SetScreenActivity(new SetScreenActivity { Active = false, Screen = component.Screen });
        }

        private void OpenStarted(ScreenBase screen)
        {
            Bus.Fire(new ScreenAnimStarted { Screen = screen });
            screen.OpenStarted -= OpenStarted;
        }

        private void OpenEnded(ScreenBase screen)
        {
            Bus.Fire(new ScreenAnimEnded { Screen = screen });
            screen.OpenEnded -= OpenEnded;
        }

        private void CloseStarted(ScreenBase screen)
        {
            Bus.Fire(new ScreenAnimStarted { Screen = screen });
            screen.CloseStarted -= CloseStarted;
        }

        private void CloseEnded(ScreenBase screen)
        {
            Bus.Fire(new ScreenAnimEnded { Screen = screen });
            screen.CloseEnded -= CloseEnded;
        }
    }
}