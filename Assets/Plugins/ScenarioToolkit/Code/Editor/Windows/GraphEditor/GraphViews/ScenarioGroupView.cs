using Scenario.Core.Model.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Scenario.Editor.Windows.GraphEditor.GraphViews
{
    public class ScenarioGroupView : Group, IScenarioGraphElement, IHashable
    {
        public Vector2 GetCenterPosition() => GetPosition().center;
        public GraphElement GetGraphElement() => this;
        public void SetCenterPosition(Vector2 newPos)
        {
            style.left = newPos.x;// - style.width.value.value / 2;
            style.top = newPos.y;// + style.height.value.value / 2;
        }

        public string Name
        {
            get => title;
            set => title = value;
        }
        public int Hash { get; set; }

        public ScenarioGroupView(IEditorGroup editorGroup)
        {
            title = editorGroup.Name;
            Hash = editorGroup.Hash;
            SetPosition(new Rect(editorGroup.Position, Vector2.zero));
        }
        
        public override void OnSelected()
        {
            base.OnSelected();
            var nodeEditor = GraphEditorWindow.ConstructElementEditor();
            nodeEditor.BindGroup(this);
        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            var nodeEditor = GraphEditorWindow.ConstructElementEditor();
            nodeEditor.UnbindElement();
        }
    }
}