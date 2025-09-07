using Scenario.Core.Model.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews
{
    // Edge = Link
    public class ScenarioLinkView : Edge, IScenarioGraphElement
    {
        public IScenarioLinkFlow ScenarioLink { get; private set; }
        
        public void Construct(IScenarioLinkFlow link)
        {
            ScenarioLink = link;
        }
        
        public Vector2 GetCenterPosition() => GetPosition().center;
        public GraphElement GetGraphElement() => this;
        public void SetCenterPosition(Vector2 newPos)
        {
            style.left = newPos.x;// - style.width.value.value / 2;
            style.top = newPos.y;// + style.height.value.value / 2;
        }
        
        public override void OnSelected()
        {
            base.OnSelected();
            var nodeEditor = GraphEditorWindow.ConstructElementEditor();
            nodeEditor.BindLink(this);
        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            var nodeEditor = GraphEditorWindow.ConstructElementEditor();
            nodeEditor.UnbindElement();
        }
    }
}