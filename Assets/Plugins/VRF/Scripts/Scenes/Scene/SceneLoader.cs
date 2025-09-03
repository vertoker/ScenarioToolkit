using System;
using System.Linq;
using VRF.Scenes.Project;
using VRF.Scenes.Scriptables;
using Zenject;

namespace VRF.Scenes.Scene
{
    public class SceneLoader : IInitializable
    {
        private readonly SceneLoaderParameters parameters;
        private readonly ScenesService scenesService;
        private readonly ClientScenesConfig clientScenes;

        public SceneLoader(SceneLoaderParameters parameters,
            ScenesService scenesService, ClientScenesConfig clientScenes)
        {
            this.parameters = parameters;
            this.scenesService = scenesService;
            this.clientScenes = clientScenes;
        }
        
        public void Initialize()
        {
            var currentScene = scenesService.CurrentScene.name;
            
            switch (parameters.SceneMode)
            {
                case SceneLoaderParameters.SceneTarget.Next:
                    scenesService.LoadNextScene();
                    break;
                case SceneLoaderParameters.SceneTarget.Current:
                    scenesService.LoadScene(currentScene);
                    break;
                case SceneLoaderParameters.SceneTarget.NextToCurrent:
                    if (clientScenes.Scenes.Contains(currentScene))
                        scenesService.LoadScene(currentScene);
                    break;
                case SceneLoaderParameters.SceneTarget.Specified:
                    scenesService.LoadScene(parameters.SceneName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}