using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Utilities.Providers
{
    [CreateAssetMenu(fileName = nameof(UssEditorProvider), 
        menuName = "Scenario Toolkit/Editor/" + nameof(UssEditorProvider))]
    public class UssEditorProvider : ScriptableSingleton<UssEditorProvider>
    {
        [field: SerializeField] public StyleSheet ContextEditor { get; private set; }
        [field: SerializeField] public StyleSheet GraphEditor { get; private set; }
        [field: SerializeField] public StyleSheet NodeEditor { get; private set; }
        [field: SerializeField] public StyleSheet ReflexFunctions { get; private set; }
    }
}