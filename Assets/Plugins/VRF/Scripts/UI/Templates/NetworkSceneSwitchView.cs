using System;
using UnityEngine;
using VRF.Networking.Core.Server;
using VRF.Scenes.Project;
using VRF.UI.Templates.Base;
using Zenject;

namespace VRF.UI.Templates
{
    public class NetworkSceneSwitchView : BaseSceneSwitchView
    {
        public override Type GetControllerType() => typeof(NetworkSceneSwitchController);
    }

    public class NetworkSceneSwitchController : BaseSceneSwitchController<NetworkSceneSwitchView>
    {
        private readonly ServerScenesService serverScenesService;

        public NetworkSceneSwitchController(ScenesService scenesService, NetworkSceneSwitchView view,
            [InjectOptional] ServerScenesService serverScenesService) : base(scenesService, view)
        {
            this.serverScenesService = serverScenesService;
        }

        protected override void LoadScene()
        {
            if (serverScenesService == null)
            {
                base.LoadScene();
                return;
            }
            
            switch (View.LoadMode)
            {
                case BaseSceneSwitchView.SceneLoadMode.Custom:
                    serverScenesService.LoadScene(View.Scene);
                    break;
                case BaseSceneSwitchView.SceneLoadMode.Next:
                    Debug.LogWarning("Doesn't support");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}