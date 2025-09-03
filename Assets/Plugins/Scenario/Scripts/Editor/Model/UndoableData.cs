using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Editor.Model
{
    public class UndoableData : ScriptableObject, ISerializationCallbackReceiver
    {
        public event Action UndoPreformed;

        public string Data = string.Empty;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            UndoPreformed?.Invoke();
        }
    }
}