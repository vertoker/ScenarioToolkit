using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Library.Systems;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Проигрывает аудио в индивидуальном плеере без прерываний (динамический кэш плееров)", 
        typeof(PlayAudio), typeof(AutoAudioSystem))]
    public struct PlayAudioAuto : IScenarioAction
    {
        public AudioClip AudioClip;
    }
}