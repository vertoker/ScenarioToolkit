using System.Collections.Generic;
using ScenarioToolkit.Core.Systems.States;
using UnityEngine;

namespace ScenarioToolkit.Library.States
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