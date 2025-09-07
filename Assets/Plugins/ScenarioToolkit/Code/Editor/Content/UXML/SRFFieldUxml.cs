using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Content.UXML
{
    public struct SRFFieldUxml
    {
        public readonly TemplateContainer RootContainer;
        
        public readonly Label Name;
        public readonly VisualElement Field;
        public readonly Button Info;
        
        public SRFFieldUxml(VisualTreeAsset asset)
        {
            RootContainer = asset.Instantiate();
            //Root = RootContainer.Q<TextField>("root");
            
            Name = RootContainer.Q<Label>("name");
            Field = RootContainer.Q<VisualElement>("field");
            Info = RootContainer.Q<Button>("info");
        }
    }
}