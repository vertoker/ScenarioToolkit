using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Content.UXML
{
    public struct SRFWindowUxml
    {
        public readonly TemplateContainer RootContainer;
        
        public readonly TextField Search;
        public readonly Button Reset;
        public readonly ScrollView Items;
        
        public SRFWindowUxml(VisualTreeAsset asset)
        {
            RootContainer = asset.Instantiate();
            
            Search = RootContainer.Q<TextField>("search");
            Reset = Search.Q<Button>("reset");
            Items = RootContainer.Q<ScrollView>("items");
        }
    }
}