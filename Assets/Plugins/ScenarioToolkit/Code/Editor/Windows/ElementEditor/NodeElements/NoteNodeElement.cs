using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Editor.Windows.GraphEditor;
using ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.ElementEditor.NodeElements
{
    public class NoteNodeElement : BaseNodeElement
    {
        private readonly INoteNode noteNode;

        public NoteNodeElement(ScenarioNodeView nodeView, GraphEditorWindow graphEditor)
            : base(nodeView, graphEditor)
        {
            noteNode = (INoteNode)nodeView.ScenarioNode;

            var field = new TextField("Note")
            {
                multiline = true,
                isPasswordField = false,
                value = noteNode.Text,
            };
            field.RegisterValueChangedCallback(NoteUpdatedCallback);
            
            Root.Add(field);
        }

        private void NoteUpdatedCallback(ChangeEvent<string> evt)
        {
            noteNode.Text = evt.newValue;
            OnDataUpdated();
        }
    }
}