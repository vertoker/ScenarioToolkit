using System.Collections.Generic;
using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems.States;
using UnityEngine;

namespace ScenarioToolkit.Library.States
{
    public class SetTransformState : IState
    {
        public Dictionary<Transform, PosData> Positions = new();
        public Dictionary<Transform, EulData> Eulers = new();
        public Dictionary<Transform, QuaData> Quaternions = new();
        public Dictionary<Transform, ScaData> Scales = new();

        public struct PosData
        {
            public Vector3 Position;
            public bool Local;

            public PosData(SetPosition set)
            {
                Position = set.Position;
                Local = set.Local;
            }
            public PosData(AddPosition add)
            {
                Position = add.GetPosition();
                Local = add.Local;
            }

            public SetPosition GetPosition(Transform transform)
            {
                return new SetPosition
                {
                    Position = Position,
                    Transform = transform,
                    Local = Local,
                };
            }
        }
        public struct EulData
        {
            public Vector3 Euler;
            public bool Local;

            public EulData(SetEuler set)
            {
                Euler = set.Euler;
                Local = set.Local;
            }
            public EulData(AddEuler add)
            {
                Euler = add.GetEuler();
                Local = add.Local;
            }

            public SetEuler GetEuler(Transform transform)
            {
                return new SetEuler
                {
                    Euler = Euler,
                    Transform = transform,
                    Local = Local,
                };
            }
        }
        public struct QuaData
        {
            public Quaternion Quaternion;
            public bool Local;
            
            public QuaData(SetQuaternion set)
            {
                Quaternion = set.Quaternion;
                Local = set.Local;
            }
            public QuaData(MulQuaternion mul)
            {
                Quaternion = mul.GetQuaternion();
                Local = mul.Local;
            }

            public SetQuaternion GetQuaternion(Transform transform)
            {
                return new SetQuaternion
                {
                    Quaternion = Quaternion,
                    Transform = transform,
                    Local = Local,
                };
            }
        }
        public struct ScaData
        {
            public Vector3 Scale;
            public bool Local;

            public ScaData(SetScale set)
            {
                Scale = set.Scale;
                Local = set.Local;
            }
            public ScaData(MulScale mul)
            {
                Scale = mul.GetScale();
                Local = mul.Local;
            }

            public SetScale GetScale(Transform transform)
            {
                return new SetScale
                {
                    Scale = Scale,
                    Transform = transform,
                    Local = Local,
                };
            }
        }

        public void Clear()
        {
            Positions.Clear();
            Eulers.Clear();
            Quaternions.Clear();
            Scales.Clear();
        }
    }
}