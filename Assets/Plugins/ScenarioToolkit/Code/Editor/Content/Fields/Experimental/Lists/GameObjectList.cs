using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenario.Editor.Content.Fields.Experimental.Lists
{
    public class GameObjectList : BaseListScriptableObject
    {
        [SerializeField] private List<GameObject> gameObjects = new();
        public override string GetListName() => nameof(gameObjects);
        public override Type GetElementType() => typeof(GameObject);
    }
}