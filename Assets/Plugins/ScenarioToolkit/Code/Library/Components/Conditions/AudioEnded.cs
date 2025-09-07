using Scenario.Base.Components.Actions;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Conditions
{
    [ScenarioMeta("Окончание проигрывания Audio", typeof(StopAudio), typeof(AudioStarted), typeof(PlayAudio))]
    public struct AudioEnded : IScenarioCondition 
    {
        //public AudioSource AudioSource;
        public AudioClip AudioClip;
    }
}