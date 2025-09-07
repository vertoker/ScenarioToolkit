using System;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.External.Systems;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Обновление таймера (используйте аккуратно)", typeof(TimerStarted), typeof(TimerSystem))]
    public struct TimerUpdate : IScenarioCondition
    {
        [ScenarioMeta("Идентификатор таймера, должен быть уникальным в рамках промежутках проигрывания")]
        public string TimerID;
        public float Time;

        public string GetTimeText() => TimeSpan.FromSeconds(Time).ToString("mm':'ss");
    }
}