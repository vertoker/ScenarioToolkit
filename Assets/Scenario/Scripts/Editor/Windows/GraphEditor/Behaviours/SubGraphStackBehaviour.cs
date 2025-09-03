using System.Collections.Generic;
using Scenario.Editor.Utilities;
using UnityEngine.UIElements;

namespace Scenario.Editor.Windows.GraphEditor.Behaviours
{
    public class SubGraphStackBehaviour
    {
        private readonly GraphEditorWindow editorWindow;
        private readonly VisualElement subgraphStackParent;

        public SubGraphStackBehaviour(GraphEditorWindow editorWindow, VisualElement root)
        {
            this.editorWindow = editorWindow;
            subgraphStackParent = root.Q<VisualElement>("subgraph-stack");
            Init();
        }
        private void Init()
        {
            subgraphStackParent.Clear();
            var session = editorWindow.Session;
            session.PathStack ??= new List<string>();
            session.HashStack ??= new List<int>();
            var length = session.PathStack.Count;

            for (var i = 0; i < length; i++)
            {
                var path = session.PathStack[i];
                var hash = session.HashStack[i];
                
                void Act()
                {
                    session.UpdateStack(path);
                    editorWindow.TryLoadSession(path);
                }
                var btn = new Button(Act) 
                {
                    text = DirectoryFileHelper.GetFileName(path),
                    tooltip = $"SubNode Hash: {hash}",
                };
                subgraphStackParent.Add(btn);
            }
            
            var currentBtn = new Button
            {
                text = DirectoryFileHelper.GetFileName(session.LastOpenedPath),
            };
            currentBtn.SetEnabled(false);
            subgraphStackParent.Add(currentBtn);
        }
    }
}