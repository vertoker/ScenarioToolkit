using UnityEngine.UIElements;

namespace Scenario.Editor.Content.UXML
{
    public struct SRFFuncUxml
    {
        public readonly TemplateContainer RootContainer;
        //public readonly VisualElement Root;
        
        public readonly Foldout Foldout;
        public readonly VisualElement GroupButtons;
        public readonly Button Info;
        
        public SRFFuncUxml(VisualTreeAsset asset)
        {
            RootContainer = asset.Instantiate();
            //Root = RootContainer.Q<TextField>("root");
            
            Foldout = RootContainer.Q<Foldout>("foldout");
            GroupButtons = RootContainer.Q<VisualElement>("group-buttons");
            Info = GroupButtons.Q<Button>("info");
        }
    }
}