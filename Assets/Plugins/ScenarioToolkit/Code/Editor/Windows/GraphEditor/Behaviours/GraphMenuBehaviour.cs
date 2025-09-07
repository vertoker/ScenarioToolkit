using ScenarioToolkit.Editor.Utilities;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.GraphEditor.Behaviours
{
    public class GraphMenuBehaviour
    {
        private readonly GraphEditorWindow editorWindow;
        private readonly VisualElement root;

        public GraphMenuBehaviour(GraphEditorWindow editorWindow, VisualElement root)
        {
            this.editorWindow = editorWindow;
            this.root = root;
            Init();
        }
        private void Init()
        {
            var graphMenu = root.Q<Foldout>("graph-menu");
            graphMenu.value = true;
            
            root.Q<Button>("search").clicked += editorWindow.ElementSearch.Open;
            root.Q<Button>("ping-player").clicked += PingPlayer;
            root.Q<Button>("ping-source").clicked += PingSource;
            root.Q<Button>("element-editor").clicked += () => GraphEditorWindow.ConstructElementEditor(true);
            root.Q<Button>("context-editor").clicked += () => GraphEditorWindow.ConstructContextEditor(true);
            root.Q<Button>("reflex-functions").clicked += WindowsStatic.OpenReflexFunctions;
        }
        
        public void PingPlayer()
        {
            if (!editorWindow.SceneProvider) return;
            var obj = editorWindow.SceneProvider.Get(editorWindow.Session.SceneContextID);
            if (!obj) return;
            EditorGUIUtility.PingObject(obj);
        }
        public void PingSource()
        {
            if (string.IsNullOrWhiteSpace(editorWindow.Session.LastOpenedPath)) return;
            var projectPath = VrfPath.FromAbsoluteToProject(editorWindow.Session.LastOpenedPath);
            var jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>(projectPath);
            
            if (!jsonFile)
            {
                Debug.LogWarning($"<b>Scenario not founded</b>: {editorWindow.Session.LastOpenedPath}");
                return;
            }
            EditorGUIUtility.PingObject(jsonFile);
        }
    }
}