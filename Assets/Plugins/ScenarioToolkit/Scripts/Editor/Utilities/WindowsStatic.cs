using System.Threading.Tasks;
using Scenario.Editor.Windows.ContextEditor;
using Scenario.Editor.Windows.ElementEditor;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Editor.Windows.Other;
using UnityEditor;

namespace Scenario.Editor.Utilities
{
    public static class WindowsStatic
    {
        public const string GraphEditorTitle = "Scenario Graph";
        public const string ElementEditorTitle = "Element Editor";
        public const string ContextEditorTitle = "Context Editor";
        public const string ReflexFunctionsTitle = "Reflex Functions";
        
        // Код настолько сильно разросся, что нужно делать асинхронными те функции,
        // которые должны вызываться после полной инициализации окна
        public const int TickDelay = 30;

        public static GraphEditorWindow GetGraphEditor(bool focus = true, bool utility = false) =>
            EditorWindow.GetWindow<GraphEditorWindow>(utility, GraphEditorTitle, focus);
        public static ElementEditorWindow GetElementEditor(bool focus = true, bool utility = false) =>
            EditorWindow.GetWindow<ElementEditorWindow>(utility, ElementEditorTitle, focus);
        public static ContextEditorWindow GetContextEditor(bool focus = true, bool utility = false) =>
            EditorWindow.GetWindow<ContextEditorWindow>(utility, ContextEditorTitle, focus);
        public static ReflexFunctionsWindow GetReflexFunctions(bool focus = true, bool utility = false) =>
            EditorWindow.GetWindow<ReflexFunctionsWindow>(utility, ReflexFunctionsTitle, focus);
        
        [MenuItem("Window/Scenario/Graph Docked", false, 101)]
        public static async void OpenGraphDocked()
        {
            var graphEditor = GetGraphEditor();
            await Task.Delay(TickDelay); // нужно, так как не успевает инициализация
            var graphEditorRect = graphEditor.position;
            
            var elementEditor = GetElementEditor();
            graphEditor.Dock(elementEditor, EditorWindowUtils.Anchor.Right, graphEditorRect);
            var contextEditor = GetContextEditor();
            graphEditor.Dock(contextEditor, EditorWindowUtils.Anchor.RightBottom, graphEditorRect);
        }
        
        [MenuItem("Window/Scenario/Graph", false, 102)]
        public static void OpenGraphEditor() => GetGraphEditor();
        [MenuItem("Window/Scenario/Element Editor", false, 103)]
        public static void OpenElementEditor() => GetElementEditor();
        [MenuItem("Window/Scenario/Context Editor", false, 104)]
        public static void OpenContextEditor() => GetContextEditor();
        [MenuItem("Window/Scenario/Reflex Functions", false, 105)]
        public static void OpenReflexFunctions() => GetReflexFunctions();
    }
}