using ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.GraphEditor.Behaviours
{
    public class KeyInputGraphBehaviour
    {
        private readonly ScenarioGraphView view;
        private readonly VisualElement rootVisualElement;

        private static readonly Vector2 MoveSpeed = new(-10f, 10f);
        private bool left, right, up, down;
        
        public KeyInputGraphBehaviour(ScenarioGraphView view, VisualElement rootVisualElement)
        {
            this.view = view;
            this.rootVisualElement = rootVisualElement;
            Register();
        }
        private void Register()
        {
            rootVisualElement.RegisterCallback<KeyDownEvent>(evt => { ProcessDown(evt.keyCode); });
            rootVisualElement.RegisterCallback<KeyUpEvent>(evt => { ProcessUp(evt.keyCode); });
        }
        
        private Vector2 GetDirection()
        {
            if (up && left) return Vector2.up + Vector2.left;
            if (up && right) return Vector2.up + Vector2.right;
            if (down && left) return Vector2.down + Vector2.left;
            if (down && right) return Vector2.down + Vector2.right;
            if (up) return Vector2.up;
            if (down) return Vector2.down;
            if (left) return Vector2.left;
            if (right) return Vector2.right;
            return Vector2.zero;
        }
        private void ProcessDown(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.LeftArrow:  left = true;  break;
                case KeyCode.RightArrow: right = true; break;
                case KeyCode.UpArrow:    up = true;    break;
                case KeyCode.DownArrow:  down = true;  break;
            }
            view.MoveInstantly(GetDirection() * MoveSpeed);
        }
        private void ProcessUp(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.LeftArrow:  left = false;  break;
                case KeyCode.RightArrow: right = false; break;
                case KeyCode.UpArrow:    up = false;    break;
                case KeyCode.DownArrow:  down = false;  break;
            }
        }
    }
}