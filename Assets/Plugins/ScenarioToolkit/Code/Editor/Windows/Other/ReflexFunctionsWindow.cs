using ScenarioToolkit.Editor.Content.UXML;
using ScenarioToolkit.Editor.SRF;
using ScenarioToolkit.Editor.Utilities.Providers;
using UnityEditor;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.Other
{
    public class ReflexFunctionsWindow : EditorWindow
    {
        private SRFWindowUxml windowUxml;
        private VisualTreeAsset foldoutAsset;
        private VisualTreeAsset fieldAsset;
        
        private void Construct()
        {
            var asset = UxmlEditorProvider.instance.SrfWindow;
            windowUxml = new SRFWindowUxml(asset);
            rootVisualElement.Add(windowUxml.RootContainer);
            
            foldoutAsset = UxmlEditorProvider.instance.SrfFunc;
            fieldAsset = UxmlEditorProvider.instance.SrfField;
        }
        private void OnEnable()
        {
            Construct();
            windowUxml.Reset.clicked += ResetPressed;
            windowUxml.Search.RegisterCallback<ChangeEvent<string>>(SearchUpdated);
        }
        private void CreateGUI()
        {
            Refresh();
        }
        private void OnDisable()
        {
            windowUxml.Reset.clicked -= ResetPressed;
            windowUxml.Search.UnregisterCallback<ChangeEvent<string>>(SearchUpdated);
        }
        
        private void ResetPressed()
        {
            if (windowUxml.Search.value == string.Empty)
                Refresh();
            else windowUxml.Search.value = string.Empty;
        }
        private void SearchUpdated(ChangeEvent<string> evt)
        {
            Refresh(evt.newValue);
        }
        private void Refresh(string searchQuery = "")
        {
            windowUxml.Items.Clear();
            
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                foreach (var instance in SRFUtils.Instances)
                {
                    var metadata = instance.GetMetadata();
                    if (metadata.Disabled) continue;
                    var item = BuildInstance(instance, metadata);
                    windowUxml.Items.Add(item);
                }
            }
            else
            {
                foreach (var instance in SRFUtils.Instances)
                {
                    var metadata = instance.GetMetadata();
                    if (metadata.Disabled) continue;
                    if (metadata.ContainsQuery(searchQuery))
                    {
                        var item = BuildInstance(instance, metadata);
                        windowUxml.Items.Add(item);
                    }
                }
            }
        }
        
        public VisualElement BuildInstance(IScenarioReflexFunction srfInstance, SRFMetadata metadata)
        {
            // создание визуальных элементов для func
            var funcUxml = new SRFFuncUxml(foldoutAsset);
            funcUxml.Foldout.value = false;
            funcUxml.Foldout.text = metadata.FunctionName;
            if (!string.IsNullOrWhiteSpace(metadata.FunctionTooltip))
            {
                funcUxml.Info.SetEnabled(false);
                funcUxml.Info.tooltip = metadata.FunctionTooltip;
            }
            else
            {
                funcUxml.Info.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }

            // создание основного функционала
            var btn = new Button { text = "Execute" };
            var context = new SRFContext(funcUxml.Foldout, foldoutAsset, btn, fieldAsset);
            btn.clicked += OnBtnOnClicked;
            
            // финальное добавление и завершение функции
            funcUxml.Foldout.Add(btn);
            srfInstance.BuildUI(context);
            return funcUxml.RootContainer;

            void OnBtnOnClicked() => srfInstance.Execute(context);
        }
    }
}