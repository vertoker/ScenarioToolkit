using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using UnityEngine;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Components.Conditions;
using Zenject;

namespace ScenarioToolkit.External.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class TimerSystem : BaseScenarioSystem
    {
        private readonly Dictionary<string, Entry> entries = new();

        private struct Entry
        {
            public readonly StartTimer Component;
            public readonly Timer Timer;
            
            public Entry(StartTimer startTimer)
            {
                Component = startTimer;
                Timer = new Timer(startTimer.InGameTime, startTimer.RealTime);
            }
        }

        public TimerSystem(ScenarioComponentBus listener) : base(listener)
        {
            Bus.DeclareSignal<TimerUpdate>();
            Bus.Subscribe<StartTimer>(StartTimerBus);
            Bus.Subscribe<StopTimer>(StopTimerBus);
            Bus.Subscribe<PauseTimer>(PauseTimerBus);
            Bus.Subscribe<ResumeTimer>(ResumeTimerBus);
        }

        // protected override void ApplyState(TimerState state)
        // {
        //     foreach (var (id, data) in state.PausedTimers)
        //     {
        //         StartTimer(data.Component);
        //         var timer = entries[id].Timer;
        //         timer.SetRealTime(data.PauseRealTime);
        //         timer.Pause();
        //     }
        //
        //     foreach (var (id, data) in state.RunningTimers)
        //     {
        //         StartTimer(data.Component);
        //         var timer = entries[id].Timer;
        //         timer.SetRealTime(data.LastPauseRealTime + (float)data.GetPassedTime());
        //     }
        // }

        private async void StartTimerBus(StartTimer component)
        {
            await UniTask.Yield();
            StartTimer(component);
            // State.RunningTimers.Add(component.ID,
            //     new TimerState.RunningTimerData(component, 0, component.RealTime));
        }
        
        private void StopTimerBus(StopTimer component)
        {
            StopTimer(component.ID);
            // {
            //     State.RunningTimers.Remove(component.ID);
            //     State.PausedTimers.Remove(component.ID);
            // }
        }

        private void PauseTimerBus(PauseTimer component)
        {
            var id = component.ID;

            PauseTimer(id);
            // {
            //     var entry = entries[id];
            //
            //     State.RunningTimers.Remove(id);
            //     State.PausedTimers.Add(id, new TimerState.PausedTimerData(entry.Component, entry.Timer.RealTime));
            // }
        }

        private void ResumeTimerBus(ResumeTimer component)
        {
            var id = component.ID;

            ResumeTimer(id);
            // {
            //     var entry = entries[id];
            //
            //     State.PausedTimers.Remove(id);
            //     State.RunningTimers.Add(id,
            //         new TimerState.RunningTimerData(entry.Component, entry.Timer.RealTime, entry.Component.RealTime - entry.Timer.RealTime));
            // }
        }

        public bool StartTimer(StartTimer component)
        {
            //if (AssertLog.NotNull<StartTimer>(component.View, nameof(component.View))) return; // ?
            if (AssertLog.Above<StartTimer>(component.RealTime, 0, nameof(component.RealTime))) return false;
            if (AssertLog.Above<StartTimer>(component.InGameTime, 0, nameof(component.InGameTime))) return false;
            
            if (AssertLog.NotEmpty<StartTimer>(component.ID, nameof(component.ID))) return false;
            if (entries.ContainsKey(component.ID))
            {
                Debug.LogWarning($"ID {component.ID} already in system, drop");
                return false;
            }

            var entry = new Entry(component);
            entries.Add(component.ID, entry);

            entry.Timer.Updated += time => UpdateTimer(entry, time);
            entry.Timer.Cancelled += () => StopTimer(entry, true);
            entry.Timer.Ended += () => StopTimer(entry, false);
            
            // if (component.EnableOnStart) component.View?.Open();
            
            entry.Timer.Start();
            Bus.Fire(new TimerStarted { ID = entry.Component.ID });
            
            // if (source.Debug)
                Debug.Log($"Timer {component.ID} <b>started</b>. " +
                          $"Time: {component.InGameTime}, " +
                          $"RealTime: {component.RealTime}");

            return true;
        }

        /// <summary>
        /// Таймер может быть окончен (ended) и отменён (cancelled)<br/>
        /// Таймер считается ended если:<br/>
        ///     - Таймер сам подошёл к концу<br/>
        ///     - Был вызван StopTimer с false<br/>
        /// Таймер считается cancelled если:<br/>
        ///     - Был вызван CancelTimer с true
        /// </summary>
        public bool StopTimer(string id, bool cancelled = false)
        {
            if (AssertLog.NotEmpty<StopTimer>(id, nameof(id))) return false;
            if (!entries.TryGetValue(id, out var entry))
            {
                Debug.LogWarning("ID not in system, drop");
                return false;
            }
            entry.Timer.Stop(cancelled);
            
            return true;
        }

        private void StopTimer(Entry entry, bool cancelled)
        {
            if (cancelled)
                Bus.Fire(new TimerCancelled { ID = entry.Component.ID });
            else Bus.Fire(new TimerEnded { ID = entry.Component.ID });
            
            // if (entry.Component.DisableOnEnd) entry.Component.View?.Close();
            
            entries.Remove(entry.Component.ID);
            
            // if (source.Debug)
                Debug.Log($"Timer {entry.Component.ID} " +
                          $"<b>{(cancelled ? "cancelled" : "ended")}</b> " +
                          $"at time <b>{entry.Timer.GameTime}</b>. " +
                          $"Time: {entry.Component.InGameTime}, " +
                          $"RealTime: {entry.Component.RealTime}");
        }

        private void UpdateTimer(Entry entry, float time)
        {
            var updated = new TimerUpdate { TimerID = entry.Component.ID, Time = time };
            // entry.Component.View?.TextComponent?.SetText(updated.GetTimeText());
            Bus.Fire(updated);
        }

        public bool PauseTimer(string id)
        {
            if (AssertLog.NotEmpty<PauseTimer>(id, "ID")) return false;
            if (!entries.TryGetValue(id, out var entry))
            {
                Debug.LogWarning("ID not in system, drop");
                return false;
            }
            
            entry.Timer.Pause();
            
            // if (source.Debug)
                Debug.Log($"Timer {id} <b>paused</b> " +
                          $"at time <b>{entry.Timer.GameTime}</b>. " +
                          $"Time: {entry.Component.InGameTime}, " +
                          $"RealTime: {entry.Component.RealTime}");

            return true;
        }

        public bool ResumeTimer(string id)
        {
            if (AssertLog.NotEmpty<ResumeTimer>(id, "ID")) return false;
            if (!entries.TryGetValue(id, out var entry))
            {
                Debug.LogWarning("ID not in system, drop");
                return false;
            }
            
            entry.Timer.Resume();
            
            // if (source.Debug)
                Debug.Log($"Timer {id} <b>resumed</b> " +
                          $"at time <b>{entry.Timer.GameTime}</b>. " +
                          $"Time: {entry.Component.InGameTime}, " +
                          $"RealTime: {entry.Component.RealTime}");

            return true;
        }

        public void SetTimeScale(string id, float timeScale)
        {
            if (AssertLog.NotEmpty<TimerUpdate>(id, "ID")) return;
            if (AssertLog.Above<TimerUpdate>(timeScale, 0, nameof(timeScale))) return;
            
            if (!entries.TryGetValue(id, out var entry))
            {
                Debug.LogWarning("ID not in system, drop");
                return;
            }

            entry.Timer.TimeScale = timeScale;
            
            // if (source.Debug)
                Debug.Log($"Timer {id} <b>resumed</b> " +
                          $"at time <b>{entry.Timer.GameTime}</b>. " +
                          $"Time: {entry.Component.InGameTime}, " +
                          $"RealTime: {entry.Component.RealTime}");
        }

        [Serializable]
        private class Timer
        {
            private float targetRealTime;
            private float realTimeToGameTime;

            private float invertTimeScale = 1;
            private int pauseScale = 1;
            private CancellationTokenSource tokenSource;
            
            private bool isPlayed;
            
            public Action<float> Updated;
            public Action Ended;
            public Action Cancelled;

            public float RealTime { get; private set; }
            public float GameTime => RealTime * realTimeToGameTime;
            private float DeltaTime => pauseScale * invertTimeScale * Time.deltaTime;
            public float TimeScale
            {
                get => 1f / invertTimeScale;
                set => invertTimeScale = 1f / value;
            }

            /// <param name="inGameTime">Внутриигровое время для таймера</param>
            /// <param name="realTime">Сколько это время должно идти в реальном времени</param>
            public Timer(float inGameTime, float realTime)
            {
                targetRealTime = realTime;
                realTimeToGameTime = inGameTime / realTime;
            }

            public void Start()
            {
                if (isPlayed) return;
                
                tokenSource = new CancellationTokenSource();
                Updater(tokenSource.Token).Forget();
                isPlayed = true;
            }
            public void Stop(bool cancelled)
            {
                if (!isPlayed) return;
                isPlayed = false;
                
                tokenSource.Cancel();
                tokenSource.Dispose();
                tokenSource = null;
                
                if (cancelled)
                    Cancelled?.Invoke();
                else Ended?.Invoke();
            }

            public void SetRealTime(float value)
            {
                RealTime = value;
                Updated?.Invoke(GameTime);
            }
            
            private async UniTaskVoid Updater(CancellationToken token)
            {
                SetRealTime(0);

                for (; RealTime < targetRealTime; RealTime += DeltaTime)
                {
                    Updated?.Invoke(GameTime);

                    if (token.IsCancellationRequested)
                    {
                        Stop(true);
                        return;
                    }
                    await UniTask.Yield();
                }
                
                SetRealTime(targetRealTime);
                Stop(false);
            }

            public void Pause()
            {
                if (pauseScale == 1)
                    pauseScale = 0;
            }
            public void Resume()
            {
                if (pauseScale == 0)
                    pauseScale = 1;
            }
        }
    }
}