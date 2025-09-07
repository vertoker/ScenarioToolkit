using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Двигает объект до точки по заданному времени через Lerp (позиции кэшируются)")]
    public struct Move : IScenarioAction, IComponentDefaultValues
    {
        public Transform MovingObject;
        public Transform Target;
        public float Time;
        [ScenarioMeta("Модификатор для lerp, меняет стиль анимации")]
        public Easings.Type Ease;
        
        public void SetDefault()
        {
            MovingObject = null;
            Target = null;
            Time = 1;
            Ease = Easings.Type.Linear;
        }
    }
}
