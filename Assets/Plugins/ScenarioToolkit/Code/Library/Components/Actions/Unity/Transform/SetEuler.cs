using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает значения для Transform.eulerAngles")]
    public struct SetEuler : IScenarioAction, IComponentDefaultValues
    {
        public Transform Transform;
        public Vector3 Euler;
        [ScenarioMeta("Выбор между eulerAngles и localEulerAngles")]
        public bool Local;
        
        public void SetDefault()
        {
            Transform = null;
            Euler = Vector3.zero;
            Local = true;
        }
    }
}