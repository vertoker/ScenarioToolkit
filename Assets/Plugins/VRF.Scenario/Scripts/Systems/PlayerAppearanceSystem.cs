using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Networking.Core.Client;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PlayerAppearanceSystem : BaseScenarioSystem
    {
        private readonly ClientPlayerAppearances playerAppearances;
        private readonly IdentityService identityService;

        public PlayerAppearanceSystem(SignalBus bus,
            [InjectOptional] ClientPlayerAppearances playerAppearances,
            [InjectOptional] IdentityService identityService) : base(bus)
        {
            if (playerAppearances == null || identityService == null)
                return;
            
            this.playerAppearances = playerAppearances;
            this.identityService = identityService;

            bus.Subscribe<SetAppearance>(SetAppearance);
            bus.Subscribe<ResetAppearance>(ResetAppearance);
        }

        private void SetAppearance(SetAppearance component)
        {
            if (AssertLog.NotNull<SetAppearance>(component.Appearance, nameof(component.Appearance))) return;

            playerAppearances.SendResetAppearance(identityService.SelfIdentity); //TODO: Deep fix
            playerAppearances.SendUpdateAppearance(identityService.SelfIdentity, component.Appearance);
        }

        private void ResetAppearance(ResetAppearance component)
        {
            playerAppearances.SendResetAppearance(identityService.SelfIdentity);
        }
    }
}