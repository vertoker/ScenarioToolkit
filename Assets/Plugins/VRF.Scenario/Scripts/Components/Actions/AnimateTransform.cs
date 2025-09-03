using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using SimpleUI.Extensions;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Изменяет transform до точки по заданному времени через Lerp (позиции кэшируются)")]
    public struct AnimateTransform : IScenarioAction, IComponentDefaultValues
    {
        public Transform MovingObject;
        public Transform Target;
        public float Time;
        public bool AnimatePosition;
        public bool AnimateRotation;
        public bool AnimateScale;
        [ScenarioMeta("Модификатор для lerp, меняет стиль анимации")]
        public Easings.Type Ease;
        
        public void SetDefault()
        {
            MovingObject = null;
            Target = null;
            Time = 1;
            AnimatePosition = true;
            AnimateRotation = true;
            AnimateScale = true;
            Ease = Easings.Type.Linear;
        }
    }
}
