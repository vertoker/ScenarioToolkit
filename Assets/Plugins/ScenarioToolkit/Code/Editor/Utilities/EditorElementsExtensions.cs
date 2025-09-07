using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews;
using UnityEngine;
using ZLinq;

namespace ScenarioToolkit.Editor.Utilities
{
    public static class EditorElementsExtensions
    {
        public static Vector2 GetCenter(this IEnumerable<IEditorNode> nodes, int count)
        {
            if (count == 0) return Vector2.zero;
            
            var nodeCenter = nodes.Select(n => n.Position)
                .Aggregate(Vector2.zero, (accum, value) => accum + value);
            nodeCenter /= count;
            return nodeCenter;
        }
        public static Vector2 GetCenter<TEnumerator>(this ValueEnumerable<TEnumerator, IEditorNode> nodes, int count)
            where TEnumerator : struct, IValueEnumerator<IEditorNode>
        {
            if (count == 0) return Vector2.zero;
            
            var nodeCenter = nodes.Select(n => n.Position)
                .Aggregate(Vector2.zero, (accum, value) => accum + value);
            nodeCenter /= count;
            return nodeCenter;
        }
        public static Vector2 GetCenter(this IReadOnlyList<IScenarioGraphElement> nodes)
        {
            if (nodes.Count == 0) return Vector2.zero;
            
            var nodeCenter = nodes.Select(n => n.GetCenterPosition())
                .Aggregate(Vector2.zero, (accum, value) => accum + value);
            nodeCenter /= nodes.Count;
            return nodeCenter;
        }
    }
}