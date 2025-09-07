using ScenarioToolkit.Shared.VRF.Utilities.VRF;
using UnityEditor;

namespace ScenarioToolkit.Editor.Tools.Files
{
    public static class ScenarioImportTemplates
    {
        [MenuItem("Tools/Scenario Files/Scenario: Import Templates", false, 0)]
        public static void ImportTemplates()
        {
            const string src = "Assets/Modules/Scenario/Templates/";
            const string dst = "Assets/Resources/Scenario/Jsons/Templates/";
            
            ScriptableTools.CopyFolderEditor(src, dst);
        }
    }
}