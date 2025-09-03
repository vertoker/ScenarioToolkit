using JetBrains.Annotations;
using Scenario.Base.Components.Actions;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    /// <summary>
    /// Расширения для сценария, которые нужно делать ДО операций включения/выключения
    /// </summary>
    public class ScenarioExtensionsSystem : BaseScenarioSystem
    {
        [CanBeNull] private readonly PlayerVRView playerVR;

        public ScenarioExtensionsSystem(SignalBus bus, [InjectOptional] PlayerVRView playerVR) : base(bus)
        {
            this.playerVR = playerVR;
        }
    }
}