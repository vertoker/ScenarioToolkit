using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Editor.Model
{
    public class GraphCameraPose
    {
        public Vector3 Position;
        public Vector3 Scale;

        public GraphCameraPose()
        {
            Position = Vector3.zero;
            Scale = Vector3.one;
        }
        public GraphCameraPose(Vector3 position, Vector3 scale)
        {
            Position = position;
            Scale = scale;
        }
    }
}