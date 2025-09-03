using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;
using VRF.BNG_Framework.Scripts.Helpers;
using VRF.Scenario.Components.Actions;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Игрок был телепортирован", typeof(TeleportPlayer), typeof(PlayerTeleport))]
    public struct PlayerTeleported : IScenarioCondition
    {
        public TeleportDestination Destination;
    }
}