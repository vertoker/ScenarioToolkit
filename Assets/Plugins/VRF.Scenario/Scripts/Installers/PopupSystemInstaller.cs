using Scenario.Core.Installers.Systems;
using Scenario.Utilities.Extensions;
using UnityEngine;
using VRF.Scenario.Models;
using VRF.Scenario.Systems;
using VRF.Scenario.UI.ScenarioGame;

namespace VRF.Scenario.Installers
{
    public class PopupSystemInstaller : BaseSystemInstaller
    {
        [SerializeField] private ShowAtLookSettings defaultSettings = new(3, 1, 0);
        
        public override void InstallBindings()
        {
            var resolver = GetResolver();
            Container.BindScenarioSystem<PopupCreateSystem>(resolver).WithArguments(defaultSettings);
            
            Container.BindScenarioSystem<PopupActivateSystem>(resolver);
            
            var behaviours = FindObjectsByType<PopupScreen>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Container.BindScenarioSystem<PopupCounterSystem>(resolver).WithArguments(behaviours);

            Container.BindScenarioSystem<PopupScreenSystem>(resolver);
        }
    }
}