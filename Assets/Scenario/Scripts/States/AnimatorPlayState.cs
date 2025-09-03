using System.Collections.Generic;
using Scenario.Core.Systems.States;
using UnityEngine;

namespace Scenario.States
{
    public class AnimatorPlayState : IState
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public Dictionary<Animator, Data> Animators = new();
        
        public class Data : NetworkContinuousData
        {
            public string StateName;
            public int Layer;

            public Data(string stateName, float seconds) : base(seconds)
            {
                StateName = stateName;
            }
        }
        
        public void Clear()
        {
            // TODO Reset не работает на Continuous данные
            Animators.Clear();
        }
    }
}