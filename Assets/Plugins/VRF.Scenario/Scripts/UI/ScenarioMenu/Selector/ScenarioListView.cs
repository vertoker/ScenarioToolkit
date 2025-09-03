using System;
using System.Collections.Generic;
using Scenario.Core.Scriptables;
using SimpleUI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRF.Utils.Pool;
using Zenject;

namespace VRF.Scenario.UI.ScenarioMenu.Selector
{
    public class ScenarioListView : UIView
    {
        [SerializeField] private float buttonHeight = 125;
        [SerializeField] private RectTransform content;
        [SerializeField] private int startCapacity = 30;
        [SerializeField] private RectTransform scenarioButton;
        [SerializeField] private float buttonOffset = 20;

        public float ButtonHeight => buttonHeight;
        public RectTransform Content => content;
        public float ButtonOffset => buttonOffset;

        public RectTransform ScenarioButton => scenarioButton;
        public int StartCapacity => startCapacity;
        
        public override Type GetControllerType() => typeof(ScenarioListController);
    }

    public class ScenarioListController : UIController<ScenarioListView>, IInitializable, IDisposable
    {
        private readonly ComponentPool<RectTransform> pool;
        private readonly Stack<ScenarioButton> actives;

        private ScenarioViewerController viewerController;
        
        public ScenarioListController(ScenarioListView view) : base(view)
        {
            pool = new ComponentPool<RectTransform>(view.ScenarioButton, view.Content, view.StartCapacity);
            actives = new Stack<ScenarioButton>(view.StartCapacity);
        }
        public void Initialize()
        {
            if (!View.Screen.TryGetController<ScenarioViewerView,
                    ScenarioViewerController>(false, out viewerController))
            {
                Debug.LogError($"Can't find {nameof(ScenarioViewerController)}");
                return;
            }
            
            View.Screen.Opened += OnOpened;
            View.Screen.Closed += OnClosed;
        }
        public void Dispose()
        {
            View.Screen.Opened -= OnOpened;
            View.Screen.Closed -= OnClosed;
            
            Clear();
            pool.Clear();
        }
        
        private void OnOpened(ScreenBase screen)
        {
            
        }
        private void OnClosed(ScreenBase screen)
        {
            Clear();
        }
        
        public void Setup(IEnumerable<ScenarioModule> modules)
        {
            if (modules == null) return;
            Clear(); // TODO неэффективно, но мне лень править

            var counter = 0;
            foreach (var module in modules)
            {
                var moduleLocal = module;

                void Callback()
                {
                    if (viewerController.CurrentModule != moduleLocal)
                        viewerController.Show(moduleLocal);
                    //else viewerController.Clear();
                }

                actives.Push(new ScenarioButton(pool.Get(), this, module.ModuleName, counter, Callback, View.ButtonOffset));

                counter++;
            }
            
            View.Content.sizeDelta = new Vector2(View.Content.sizeDelta.x, View.ButtonHeight * counter + View.ButtonOffset);
        }
        public void Clear()
        {
            while (actives.TryPop(out var item))
            {
                pool.Release(item.Content);
                item.Dispose();
            }
        }

        private readonly struct ScenarioButton
        {
            public readonly RectTransform Content;
            private readonly Button btn;
            
            public ScenarioButton(RectTransform content, ScenarioListController controller, 
                string moduleName, int index, UnityAction btnCallback, float offset = 0)
            {
                Content = content;
                var text = content.GetComponentInChildren<TMP_Text>();
                btn = content.GetComponentInChildren<Button>();
                
                var counter = -index;
                Content.anchoredPosition = new Vector2(0, controller.View.ButtonHeight * counter - offset);
                text.text = moduleName;
                btn.onClick.AddListener(btnCallback);
                
                // костыль, но имеет право на жизнь
                if (counter == 0)
                {
                    btnCallback();
                    btn.Select();
                }
            }
            public void Dispose()
            {
                btn.onClick.RemoveAllListeners();
            }
        }
    }
}