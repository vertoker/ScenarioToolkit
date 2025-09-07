using UnityEditor;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities
{
    /// <summary>
    /// Стандартные функции редактора, но которые нормально компилируются в Runtime
    /// и заменяются на пустые функции-болванки.
    /// Создано исключительно, чтобы лишний раз не писать #if UNITY_EDITOR
    /// </summary>
    public static class VrfRuntimeEditor
    {
        public static void SetDirty(Object obj)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
#endif
        }
    }
}