using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает значения для Transform.scale")]
    public struct SetScale : IScenarioAction, IComponentDefaultValues
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
    }
}