#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;

// ReSharper disable once CheckNamespace
namespace Scenario
{
    // Скопировано из Mirror с любовью
    internal static class PreprocessorDefine
    {
        [InitializeOnLoadMethod]
        public static void AddDefineSymbols()
        {
#if UNITY_2021_2_OR_NEWER
            var currentDefines = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup));
#else
            // Deprecated in Unity 2023.1
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
#endif
            var defines = new HashSet<string>(currentDefines.Split(';'))
            {
                "SCENARIO_FRAMEWORK",
            };
            
            var newDefines = string.Join(";", defines);
            if (newDefines != currentDefines)
            {
#if UNITY_2021_2_OR_NEWER
                PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup), newDefines);
#else
                // Deprecated in Unity 2023.1
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
#endif
            }
        }
    }
}
#endif