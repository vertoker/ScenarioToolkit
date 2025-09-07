using System.Collections.Generic;
using ScenarioToolkit.Core.Systems.States;
using UnityEngine;

namespace ScenarioToolkit.Library.States
{
    public class AudioPlayState : IState
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public Dictionary<AudioSource, NetworkClipData> Audios = new();

        public class NetworkClipData : NetworkContinuousData
        {
            // ReSharper disable once FieldCanBeMadeReadOnly.Global
            public AudioClip Clip;
            
            public NetworkClipData(AudioClip clip) : base(clip.length)
            {
                Clip = clip;
            }
        }

        public void Clear()
        {
            Audios.Clear();
        }
    }
}