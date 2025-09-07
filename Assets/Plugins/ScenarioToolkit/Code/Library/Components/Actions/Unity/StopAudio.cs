using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Останавливает аудио в плеере", typeof(PlayAudio), typeof(AudioEnded))]
    public struct StopAudio : IScenarioAction
    {
        public AudioSource AudioSource;
    }
}