using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Content.UXML
{
    public struct NodesFieldUxml
    {
        public readonly TemplateContainer RootContainer;
        public readonly VisualElement FieldElement;
        public readonly VisualElement VariableFieldElement;

        public readonly Toggle ToggleField;
        public readonly Label NameField;
        public readonly TextField TextField;
        public readonly Button InfoBtnComponent;

        public NodesFieldUxml(VisualTreeAsset asset)
        {
            RootContainer = asset.Instantiate();
            FieldElement = RootContainer.Q<VisualElement>("field");
            VariableFieldElement = RootContainer.Q<VisualElement>("variable-field");
                
            ToggleField = RootContainer.Q<Toggle>("override");
            NameField = RootContainer.Q<Label>("name");
            TextField = VariableFieldElement.Q<TextField>("text-field");
            InfoBtnComponent = RootContainer.Q<Button>("info");
        }
    }
}