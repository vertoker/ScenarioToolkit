using ScenarioToolkit.Editor.Utilities;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.GraphEditor.Behaviours
{
    public class FileMenuBehaviour
    {
        private readonly GraphEditorWindow editorWindow;
        private readonly VisualElement root;

        public FileMenuBehaviour(GraphEditorWindow editorWindow, VisualElement root)
        {
            this.editorWindow = editorWindow;
            this.root = root;
            Init();
        }
        private void Init()
        {
            var fileMenu = root.Q<Foldout>("file-menu");
            fileMenu.value = true;
            
            root.Q<Button>("new").clicked += New;
            root.Q<Button>("open").clicked += Open;
            root.Q<Button>("save").clicked += editorWindow.Save;
            root.Q<Button>("save-as").clicked += () => SaveAs();
        }
        
        public void New()
        {
            editorWindow.Session.ResetSceneContext();
            editorWindow.TryLoadSession(string.Empty, true);
        }
        public void Open()
        {
            var openDirectory = DirectoryFileHelper.GetFileDirectory(editorWindow.Session.LastOpenedPath);
            if (string.IsNullOrEmpty(openDirectory)) openDirectory = DirectoryFileHelper.SaveDirectory;
            var openPath = EditorUtility.OpenFilePanel("Open", openDirectory, "json");
            
            editorWindow.Session.ClearStack();
            editorWindow.Session.ResetSceneContext();
            editorWindow.TryLoadSession(openPath);
        }
        public void Open(TextAsset scenarioAsset)
        {
            if (!scenarioAsset)
            {
                Debug.LogWarning($"Empty scenario asset, drop loading");
                return;
            }
            
            var projectPath = AssetDatabase.GetAssetPath(scenarioAsset);
            var absolutePath = VrfPath.FromProjectToAbsolute(projectPath);
            
            editorWindow.Session?.ClearStack();
            editorWindow.TryLoadSession(absolutePath);
        }
        public string SaveAs()
        {
            var savePath = EditorUtility.SaveFilePanel("Save As",
                DirectoryFileHelper.SaveDirectory, DirectoryFileHelper.GetNewFileName("new"), "json");
            if (savePath.Length == 0) return null;
            editorWindow.SaveSession(savePath);
            return savePath;
        }
    }
}