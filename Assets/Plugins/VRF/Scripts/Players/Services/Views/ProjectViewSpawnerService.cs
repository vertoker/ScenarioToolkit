using Zenject;

namespace VRF.Players.Services.Views
{
    public class ProjectViewSpawnerService : BaseViewSpawnerService
    {
        public ProjectViewSpawnerService(IInstantiator instantiator, 
            ViewsSpawnedContainer container) : base(instantiator, container)
        {
            
        }
    }
}