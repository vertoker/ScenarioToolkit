using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Networking.Core.Client;
using VRF.Players.Scriptables;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Выдаёт игроку новый облик (по сети)", typeof(ResetAppearance), typeof(ClientPlayerAppearances))]
    public struct SetAppearance : IScenarioAction
    {
        public PlayerAppearanceConfig Appearance;
    }
}