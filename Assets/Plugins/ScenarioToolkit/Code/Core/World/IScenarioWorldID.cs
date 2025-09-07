using ScenarioToolkit.Shared;
using UnityEngine;

namespace ScenarioToolkit.Core.World
{
    public interface IScenarioWorldID
    {
        public string GetID();
        public void SetID(string newID);
        
        public void EditorRefreshID();
        public void RefreshID();

        public static void EditorRefreshID(IScenarioWorldID self, Object selfObj)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(selfObj, "Refresh ID of ScenarioBehaviour");
#endif
            RefreshID(self, selfObj);
        }
        public static void RefreshID(IScenarioWorldID self, Object selfObj)
        {
            // Имя + 6 знаков это оптимальный способ идентификации, коллизий не было ни разу
            self.SetID($"{selfObj.name}_{CryptoUtility.GetRandomString()}");
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(selfObj);
#endif
        }
    }
}