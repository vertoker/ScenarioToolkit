using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.GraphEditor.Behaviours
{
    public class GraphEditorBehaviour
    {
        private readonly GraphEditorWindow editorWindow;
        private readonly VisualElement root;

        public GraphEditorBehaviour(GraphEditorWindow editorWindow, VisualElement root)
        {
            this.editorWindow = editorWindow;
            this.root = root;
            Init();
        }
        private void Init()
        {
            root.StretchToParentSize();
            root.Q<VisualElement>("graph-container").Add(editorWindow.GraphView);
            editorWindow.GraphController.GraphChanged += editorWindow.SetDirtyScenario;
        }
    }
}