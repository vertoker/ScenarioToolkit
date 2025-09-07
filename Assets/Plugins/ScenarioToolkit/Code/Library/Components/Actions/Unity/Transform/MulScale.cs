using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Умножает значения на Transform.scale")]
    public struct MulScale : IScenarioAction, IComponentDefaultValues
    {
        public Transform Transform;
        public Vector3 Scale;
        [ScenarioMeta("Выбор между lossyScale и localScale")]
        public bool Local;
        
        public void SetDefault()
        {
            Transform = null;
            Scale = Vector3.one;
            Local = true;
        }

        public Vector3 GetScale()
        {
            if (Local) return Vector3.Scale(Transform.localScale, Scale);
            return Vector3.Scale(Transform.lossyScale, Scale);
        }
    }
}