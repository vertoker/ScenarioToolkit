using ScenarioToolkit.Shared.VRF;

namespace ScenarioToolkit.Core.Scriptables
{
    public interface IGetScenarioLaunch
    {
        public ScenarioModule GetModule();
        public bool GetUseNetwork();
        public bool GetUseLog();
        public int GetIdentityHash();
        public int GetIdentityHash(IdentityService identityService);
    }
}