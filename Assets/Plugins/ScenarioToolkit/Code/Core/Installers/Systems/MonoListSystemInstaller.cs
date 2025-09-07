using ScenarioToolkit.Shared.Extensions;
using UnityEngine;

namespace ScenarioToolkit.Core.Installers.Systems
{
    public abstract class MonoListSystemInstaller<TSystem, TMonoBehaviour> : BaseSystemInstaller
        where TSystem : class where TMonoBehaviour : MonoBehaviour
    {
        public override void InstallBindings()
        {
            var behaviours = FindObjectsByType<TMonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Container.BindScenarioSystem<TSystem>(GetResolver()).WithArguments(behaviours);
        }
    }
}