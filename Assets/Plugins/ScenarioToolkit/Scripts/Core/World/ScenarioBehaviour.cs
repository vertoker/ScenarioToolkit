using System;
using Scenario.Utilities;
using UnityEngine;

namespace Scenario.Core.World
{
    /// <summary> Идентификатор для всех GameObject на сцене, нужен для связи сценария с миром Unity.
    /// Также от него можно унаследоваться и сделать свою сценарную логику на сцене </summary>
    [DisallowMultipleComponent]
    public class ScenarioBehaviour : MonoBehaviour, IScenarioWorldID
    {
        [ContextMenuItem(nameof(RefreshID), nameof(EditorRefreshID))]
        // ReSharper disable once InconsistentNaming
        [SerializeField] private string ID;
        
        public string GetID() => ID;
        public void SetID(string newID) => ID = newID;

        [ContextMenu(nameof(Reset))]
        public virtual void Reset()
        {
            RefreshID();
            FindFirstObjectByType<ScenarioSceneProvider>().CacheScene(); // может лагать
        }

        [ContextMenu(nameof(RefreshID))]
        public void EditorRefreshID() => IScenarioWorldID.EditorRefreshID(this, this);
        public void RefreshID() => IScenarioWorldID.RefreshID(this, this);
    }
}