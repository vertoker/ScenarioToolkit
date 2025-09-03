using Scenario.Core.Model;
using Scenario.Core.Systems;
using Scenario.Utilities;
using Zenject;

namespace Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    // Этой системе не нужна синхронизация, так как сетевой блок всего Scenario пакета
    // уже самостоятельно отвечает за их синхронизацию
    public class ScenarioInstanceSystem : BaseScenarioSystem
    {
        public ScenarioInstanceSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SetScenarioInstance>(SetScenarioInstance);
            bus.Subscribe<StartScenarioInstance>(StartScenarioInstance);
            bus.Subscribe<StopScenarioInstance>(StopScenarioInstance);
        }

        private void SetScenarioInstance(SetScenarioInstance component)
        {
            if (AssertLog.NotNull<SetScenarioInstance>(component.Instance, nameof(component.Instance))) return;
            
            component.Instance.SetModule(component.Module);
            component.Instance.SetUseNetwork(component.UseNetwork);
            component.Instance.SetUseLog(component.UseLog);
            component.Instance.SetPlayerIdentity(component.Identity);
        }
        private void StartScenarioInstance(StartScenarioInstance component)
        {
            if (AssertLog.NotNull<StartScenarioInstance>(component.Instance, nameof(component.Instance))) return;
            
            if (component.ForcePlay)
                component.Instance.ForcePlay();
            else component.Instance.Play();
        }
        private void StopScenarioInstance(StopScenarioInstance component)
        {
            if (AssertLog.NotNull<StopScenarioInstance>(component.Instance, nameof(component.Instance))) return;
            
            component.Instance.Stop();
        }
    }
}