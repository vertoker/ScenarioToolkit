using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Отключает телепорт для игрока", typeof(AllowTeleport), typeof(PlayerTeleport))]
    public struct ForbidTeleport : IScenarioAction
    {
        
    }
}