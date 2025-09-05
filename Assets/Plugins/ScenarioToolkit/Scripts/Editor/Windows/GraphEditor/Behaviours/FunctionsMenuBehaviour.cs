using Scenario.Editor.Tools.Files;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scenario.Editor.Windows.GraphEditor.Behaviours
{
    public class FunctionsMenuBehaviour
    {
        private readonly GraphEditorWindow editorWindow;
        private readonly VisualElement root;

        public FunctionsMenuBehaviour(GraphEditorWindow editorWindow, VisualElement root)
        {
            this.editorWindow = editorWindow;
            this.root = root;
            Init();
        }
        private void Init()
        {
            var functionsMenu = root.Q<Foldout>("functions-menu");
            functionsMenu.value = true;
            
            root.Q<Button>("reload").clicked += Reload;
            root.Q<Button>("center-graph").clicked += CenterGraph;
            root.Q<Button>("detect-loop").clicked += DetectLoop;
        }
        
        public void Reload()
        {
            if (string.IsNullOrWhiteSpace(editorWindow.Session.LastOpenedPath))
            {
                Debug.Log("Scenario <b>anonymous</b> can't be reloaded");
                return;
            }
            
            if (editorWindow.IsHasAnyUnsavedChanges())
            {
                var option = EditorUtility.DisplayDialogComplex("Unsaved scenario", 
                    "You have unsaved changes, do you want to save current scenario?", "Save", "Cancel", "Discard");

                switch (option)
                {
                    case 0: editorWindow.Save(); break;
                    case 1: return;
                }
            }

            editorWindow.TryLoadSession(editorWindow.Session.LastOpenedPath, false, false);
        }
        public void CenterGraph()
        {
            editorWindow.GraphView.Show(editorWindow.GraphView.GetGraphCenter());
        }
        public void DetectLoop() => DetectLoop(true);
        public void DetectLoop(bool debug)
        {
            ScenarioRecursionTools.DetectRecursion(editorWindow.GraphController.Graph, editorWindow.LoadService);
            if (debug)
            {
                var scenarioName = editorWindow.Session.TextAsset ? editorWindow.Session.TextAsset.name : "anonymous";
                Debug.Log($"Scenario <b>{scenarioName}</b> doesn't contains recursions");
            }
        }
    }
}