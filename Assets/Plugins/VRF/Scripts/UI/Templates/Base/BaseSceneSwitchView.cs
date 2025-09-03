using System;
using NaughtyAttributes;
using SimpleUI.Templates.Base;
using UnityEngine;
using UnityEngine.Events;
using VRF.Scenes.Project;
using VRF.Utilities.Attributes;

namespace VRF.UI.Templates.Base
{
    public abstract class BaseSceneSwitchView : BaseButtonView
    {
        public enum SceneLoadMode
        {
            Custom,
            Next,
        }
        
        [SerializeField] private SceneLoadMode loadMode = SceneLoadMode.Custom;
        [ShowIf(nameof(ShowSceneField))]
        [SerializeField, SceneReference] private string scene;
        
        private bool ShowSceneField => loadMode is SceneLoadMode.Custom;

        public SceneLoadMode LoadMode
        {
            get => loadMode;
            set => loadMode = value;
        }
        public string Scene
        {
            get => scene;
            set => scene = value;
        }
    }

    public abstract class BaseSceneSwitchController<TView> : BaseButtonController<TView> where TView : BaseSceneSwitchView
    {
        public ScenesService ScenesService { get; }
        
        protected BaseSceneSwitchController(ScenesService scenesService, TView view) : base(view)
        {
            ScenesService = scenesService;
        }
        
        protected override UnityAction GetAction() => LoadScene;

        protected virtual void LoadScene()
        {
            switch (View.LoadMode)
            {
                case BaseSceneSwitchView.SceneLoadMode.Custom:
                    ScenesService.LoadScene(View.Scene);
                    break;
                case BaseSceneSwitchView.SceneLoadMode.Next:
                    ScenesService.LoadNextScene();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}