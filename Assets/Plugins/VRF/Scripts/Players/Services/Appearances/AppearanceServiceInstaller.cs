using NaughtyAttributes;
using VRF.Players.Scriptables;
using UnityEngine;
using Zenject;

namespace VRF.Players.Services.Appearances
{
    public class AppearanceServiceInstaller : MonoInstaller
    {
#if UNITY_EDITOR
        private bool IsPlaying => Application.isPlaying;
        
        [Header("Editor Test Only"), ShowIf(nameof(IsPlaying))]
        [SerializeField] private PlayerAppearanceConfig overrideAppearance;
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void UpdateAppearance()
        {
            Container.Resolve<AppearanceService>().UpdateAppearance(overrideAppearance);
        }
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void ResetAppearance()
        {
            Container.Resolve<AppearanceService>().ResetAppearance();
        }
#endif
        
        public override void InstallBindings()
        {
            var binder = Container.BindInterfacesAndSelfTo<AppearanceService>().AsSingle();
#if UNITY_EDITOR
            if (overrideAppearance) binder.WithArguments(overrideAppearance);
#endif
        }
    }
}