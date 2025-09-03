using UnityEngine;
using UnityEngine.UIElements;

namespace VRF.Editor.MethodTools.Models
{
    public class StaticMethodVisualElement : VisualElement
    {
        public Label NameLabel { get; }
        public Button InvokeButton { get; }

        public static StaticMethodVisualElement Construct() => new();
        
        public StaticMethodVisualElement()
        {
            var root = new VisualElement
            {
                style =
                {
                    paddingTop = 3f,
                    paddingRight = 0f,
                    paddingBottom = 15f,
                    paddingLeft = 3f,
                    borderBottomColor = Color.gray,
                    borderBottomWidth = 1f
                }
            };

            NameLabel = new Label
            {
                name = nameof(NameLabel),
                style = { fontSize = 14f, },
            };

            InvokeButton = new Button
            {
                name = nameof(InvokeButton),
                text = "Invoke",
            };

            root.Add(NameLabel);
            root.Add(InvokeButton);
            Add(root);
        }
    }
}