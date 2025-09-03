using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Проигрывает Unity AudioSource (если в нём есть AudioClip)", typeof(PlayAudio), typeof(AudioStarted))]
    public struct PlayAudioSource : IScenarioAction
    {
        public AudioSource AudioSource;
    }
}