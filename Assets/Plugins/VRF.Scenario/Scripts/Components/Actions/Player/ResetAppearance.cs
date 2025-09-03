using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Networking.Core.Client;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Сбрасывает облик игроку до стандартного (по сети)", typeof(SetAppearance), typeof(ClientPlayerAppearances))]
    public struct ResetAppearance : IScenarioAction
    {
        
    }
}