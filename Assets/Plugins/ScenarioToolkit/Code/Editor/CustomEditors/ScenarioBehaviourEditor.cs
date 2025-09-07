using ScenarioToolkit.Core.World;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.CustomEditors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScenarioBehaviour))]
    public class ScenarioBehaviourEditor : UnityEditor.Editor
    {
        private VisualElement root;
        private ScenarioBehaviour Target => target as ScenarioBehaviour;
        
        public override void OnInspectorGUI() { }
        
        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement
            {
                style =
                {
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column)
                }
            };

            Draw();
            return root;
        }
        
        private void Draw()
        {
            root.Clear();
            root.Add(CreateID());
        }
        private VisualElement CreateID()
        {
            var container = new VisualElement
                { style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row) } };
            var field = new PropertyField(serializedObject.FindProperty("ID"), "");
            var buttonsContainer = new VisualElement
                { style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row) } };
            CreateRefreshButton(buttonsContainer);

            container.Add(field);
            container.Add(buttonsContainer);

            return container;
        }
        
        private void CreateRefreshButton(VisualElement container)
        {
            var refreshIDBtn = new Button(() => OnRefreshClicked(container)) { text = "Refresh ID" };
            container.Add(refreshIDBtn);
        }
        private void OnRefreshClicked(VisualElement container)
        {
            container.Clear();

            var sureText = new Label("Are you sure?");
            var yesBtn = new Button(() => OnYesClicked(container)) { text = "Yes" };
            var noBtn = new Button(() => OnNoClicked(container)) { text = "No" };

            container.Add(sureText);
            container.Add(yesBtn);
            container.Add(noBtn);
        }
        
        private void OnYesClicked(VisualElement container)
        {
            container.Clear();
            CreateRefreshButton(container);

            foreach (var t in targets)
                ((ScenarioBehaviour)t).RefreshID();
        }
        private void OnNoClicked(VisualElement container)
        {
            container.Clear();
            CreateRefreshButton(container);
        }
    }
}