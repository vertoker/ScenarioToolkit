using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает значения для Transform.position")]
    public struct SetPosition : IScenarioAction, IComponentDefaultValues
    {
        public Transform Transform;
        public Vector3 Position;
        [ScenarioMeta("Выбор между position и localPosition")]
        public bool Local;
        
        public void SetDefault()
        {
            Transform = null;
            Position = Vector3.zero;
            Local = true;
        }
    }
}