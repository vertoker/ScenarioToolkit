using ScenarioToolkit.Shared.VRF;

namespace ScenarioToolkit.Core.Scriptables
{
    public interface ISetScenarioLaunch
    {
        public void SetModule(ScenarioModule newModule);
        public void SetUseNetwork(bool newUseNetwork);
        public void SetUseLog(bool newUseLog);
        public void SetPlayerIdentity(PlayerIdentityConfig newIdentity);
    }
}