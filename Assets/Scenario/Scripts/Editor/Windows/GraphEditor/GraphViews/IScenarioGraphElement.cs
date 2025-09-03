using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Scenario.Editor.Windows.GraphEditor.GraphViews
{
    public interface IScenarioGraphElement : ISelectable
    {
        public Vector2 GetCenterPosition();
        public void SetCenterPosition(Vector2 newPos);
        public GraphElement GetGraphElement();
    }
}