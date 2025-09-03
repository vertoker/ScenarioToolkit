using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Включает телепорт для игрока", typeof(ForbidTeleport), typeof(PlayerTeleport))]
    public struct AllowTeleport : IScenarioAction
    {
        
    }
}