using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Scenario.Base.Components.Actions;
using Scenario.Base.Components.Conditions;
using Scenario.Core.Systems;
using Scenario.States;
using Scenario.Utilities;
using UnityEngine;
using Zenject;

namespace Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    /// <summary>
    /// State-timer система для запуска аудио в AudioSource (по умолчанию перезаписывает играющее аудио)
    /// </summary>
    public class AudioSystem : BaseScenarioAsyncStateSystem<AudioPlayState>
    {
        private readonly Dictionary<AudioSource, CancellationTokenSource> tokenSources = new();
        
        public AudioSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<PlayAudio>(PlayAudio);
            bus.Subscribe<PlayAudioSource>(PlayAudioSource);
            bus.Subscribe<StopAudio>(StopAudio);
        }

        protected override void ApplyState(AudioPlayState state)
        {
            foreach (var (audioSource, data) in state.Audios)
                PlayAudio(audioSource, data.Clip, (float)data.GetPassedTime() % data.Seconds);
        }
        private CancellationTokenSource UpdateToken(AudioSource source)
        {
            if (tokenSources.TryGetValue(source, out var tokenSource)) tokenSource.Cancel();
            var newToken = new CancellationTokenSource();
            tokenSources[source] = newToken;
            return newToken;
        }
        private void UpdateState(AudioSource source, AudioClip clip)
        {
            if (State.Audios.ContainsKey(source)) Bus.Fire(new StopAudio { AudioSource = source });
            State.Audios.Add(source, new AudioPlayState.NetworkClipData(clip));
        }

        private void PlayAudio(PlayAudio component)
        {
            if (AssertLog.NotNull<PlayAudio>(component.AudioClip, nameof(component.AudioClip))) return;
            if (AssertLog.NotNull<PlayAudio>(component.AudioSource, nameof(component.AudioSource))) return;
            
            PlayAudio(component.AudioSource, component.AudioClip);
        }
        private void PlayAudioSource(PlayAudioSource component)
        {
            if (AssertLog.NotNull<PlayAudioSource>(component.AudioSource, nameof(component.AudioSource))) return;
            if (AssertLog.NotNull<PlayAudioSource>(component.AudioSource.clip, "AudioSource.clip")) return;
            
            PlayAudioSource(component.AudioSource);
        }

        public async void PlayAudio(AudioSource source, AudioClip clip, float startTime = 0)
        {
            await UniTask.Yield();
            
            Bus.Fire(new StopAudio { AudioSource = source });
            
            UpdateState(source, clip);
            var newToken = UpdateToken(source);
            
            source.clip = clip;
            source.time = startTime;
            source.Play();
            
            Bus.Fire(new AudioStarted 
            {
                //AudioSource = audioSource,
                AudioClip = clip,
            });

            if (source.loop) return;
            WaitAudio(source, newToken.Token, startTime);
        }
        private async void PlayAudioSource(AudioSource source, float startTime = 0)
        {
            await UniTask.Yield();
            
            Bus.Fire(new StopAudio { AudioSource = source });
            
            UpdateState(source, source.clip);
            var newToken = UpdateToken(source);

            source.time = startTime;
            source.Play();
            
            Bus.Fire(new AudioStarted 
            {
                //AudioSource = audioSource,
                AudioClip = source.clip,
            });

            if (source.loop) return;
            WaitAudio(source, newToken.Token, startTime);
        }

        private async void WaitAudio(AudioSource audioSource, CancellationToken token, float startTime)
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            //await UniTask.WaitForSeconds(audioSource.clip.length - startTime);
            var timeElapsed = audioSource.clip.length - startTime;
            for (var i = 0f; i <= timeElapsed; i += Time.deltaTime)
            {
                if (token.IsCancellationRequested) return;
                await UniTask.Yield();
            }
            
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            Bus.Fire(new StopAudio { AudioSource = audioSource });
        }

        private void StopAudio(StopAudio component)
        {
            if (AssertLog.NotNull<PlayAudio>(component.AudioSource, nameof(component.AudioSource))) return;

            var audioSource = component.AudioSource;

            if (tokenSources.TryGetValue(audioSource, out var token))
            {
                token.Cancel(false);
            }
            
            audioSource.Stop();
            State.Audios.Remove(audioSource);
            tokenSources.Remove(audioSource);
            
            Bus.Fire(new AudioEnded
            {
                //AudioSource = component.AudioSource, 
                AudioClip = audioSource.clip,
            });
        }
    }
}