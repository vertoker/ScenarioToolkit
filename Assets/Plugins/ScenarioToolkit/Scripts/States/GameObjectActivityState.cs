using System.Collections.Generic;
using Scenario.Core.Systems.States;
using UnityEngine;

namespace Scenario.States
{
    public class GameObjectActivityState : IState
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public Dictionary<GameObject, bool> GameObjects = new();
        
        public void Clear()
        {
            GameObjects.Clear();
        }
    }
}