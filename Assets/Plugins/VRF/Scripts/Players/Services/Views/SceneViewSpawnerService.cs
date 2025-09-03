using Zenject;

namespace VRF.Players.Services.Views
{
    public class SceneViewSpawnerService : BaseViewSpawnerService
    {
        public SceneViewSpawnerService(IInstantiator instantiator, 
            ViewsSpawnedContainer container) : base(instantiator, container)
        {
            
        }
    }
}