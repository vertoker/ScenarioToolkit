using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Проигрывает аудио в плеере (останавливает плеер если он проигрывает)", typeof(StopAudio), typeof(AudioStarted))]
    public struct PlayAudio : IScenarioAction
    {
        public AudioSource AudioSource;
        public AudioClip AudioClip;
    }
}