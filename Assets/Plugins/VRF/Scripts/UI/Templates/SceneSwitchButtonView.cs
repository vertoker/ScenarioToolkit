using System;
using VRF.Scenes.Project;
using VRF.UI.Templates.Base;

namespace VRF.UI.Templates
{
    public class SceneSwitchButtonView : BaseSceneSwitchView
    {
        public override Type GetControllerType() => typeof(SceneSwitchButtonController);
    }

    public class SceneSwitchButtonController : BaseSceneSwitchController<SceneSwitchButtonView>
    {
        public SceneSwitchButtonController(ScenesService scenesService, SceneSwitchButtonView view)
            : base(scenesService, view)
        {
            
        }
    }
}