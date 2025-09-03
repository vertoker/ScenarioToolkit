using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает значения для Transform")]
    public struct SetTransform : IScenarioAction, IComponentDefaultValues
    {
        public Transform Transform;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        [ScenarioMeta("Выбор между transform и localTransform")]
        public bool Local;
        
        public void SetDefault()
        {
            Transform = null;
            Position = Vector3.zero;
            Rotation = Vector3.zero;
            Scale = Vector3.one;
            Local = true;
        }

        public SetPosition GetPosition()
        {
            return new SetPosition
            {
                Position = Position,
                Transform = Transform,
                Local = Local,
            };
        }
        public SetEuler GetEuler()
        {
            return new SetEuler
            {
                Euler = Rotation,
                Transform = Transform,
                Local = Local,
            };
        }
        public SetScale GetScale()
        {
            return new SetScale
            {
                Scale = Scale,
                Transform = Transform,
                Local = Local,
            };
        }
    }
}