using System.Collections.Generic;
using Scenario.Base.Components.Actions;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class AutoAudioSystem : BaseScenarioSystem
    {
        private readonly Transform parent;
        private readonly AudioSystem audioSystem;
        private readonly Stack<AudioSource> cache;
        private readonly HashSet<AudioSource> actives;
        private int counter = 1;
        
        public AutoAudioSystem(int reservedSources, Transform parent, AudioSystem audioSystem, ScenarioComponentBus bus) : base(bus)
        {
            this.parent = parent;
            this.audioSystem = audioSystem;
            cache = new Stack<AudioSource>(reservedSources);
            actives = new HashSet<AudioSource>(reservedSources);
            
            for (var i = 0; i < reservedSources; i++)
                PushNewSource();
            
            bus.Subscribe<PlayAudioAuto>(PlayAudioCached);
            bus.Subscribe<StopAudio>(StopAudio);
        }

        private void PushNewSource()
        {
            var obj = new GameObject($"Auto AudioSource ({counter++})");
            obj.transform.parent = parent;
            var source = obj.AddComponent<AudioSource>();
            source.spatialBlend = 0; // no 3D
            source.dopplerLevel = 0;
            cache.Push(source);
        }
        
        private void PlayAudioCached(PlayAudioAuto component)
        {
            if (AssertLog.NotNull<PlayAudioAuto>(component.AudioClip, nameof(component.AudioClip))) return;

            if (cache.Count == 0) PushNewSource();
            var source = cache.Pop();
            actives.Add(source);
            audioSystem.PlayAudio(source, component.AudioClip);
        }

        private void StopAudio(StopAudio component)
        {
            if (actives.Count == 0) return; // Optimization in "don't use" cases
            if (actives.Contains(component.AudioSource))
            {
                actives.Remove(component.AudioSource);
                cache.Push(component.AudioSource);
            }
        }
    }
}