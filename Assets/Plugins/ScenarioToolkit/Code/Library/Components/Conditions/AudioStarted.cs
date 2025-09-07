using Scenario.Base.Components.Actions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Conditions
{
    [ScenarioMeta("Начало проигрывания Audio", typeof(PlayAudio), typeof(AudioEnded))]
    public struct AudioStarted : IScenarioCondition 
    {
        //public AudioSource AudioSource;
        public AudioClip AudioClip;
    }
}