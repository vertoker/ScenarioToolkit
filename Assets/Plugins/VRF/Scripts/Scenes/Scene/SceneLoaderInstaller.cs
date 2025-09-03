using UnityEngine;
using Zenject;

namespace VRF.Scenes.Scene
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoaderParameters editor;
        [SerializeField] private SceneLoaderParameters runtime;
        
        public override void InstallBindings()
        {
            var parameters = Application.isEditor ? editor : runtime;
            Container.Bind<SceneLoaderParameters>().FromInstance(parameters).AsSingle();
            
            var loader = Container.TryResolve<SceneLoader>();
            if (loader == null) Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
        }
    }
}