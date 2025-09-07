using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Добавляет значения к Transform.position")]
    public struct AddPosition : IScenarioAction, IComponentDefaultValues
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

        public Vector3 GetPosition()
        {
            if (Local) return Transform.localPosition + Position;
            return Transform.position + Position;
        }
    }
}