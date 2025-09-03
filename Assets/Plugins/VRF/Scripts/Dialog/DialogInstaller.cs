using Zenject;

namespace VRF.Dialog
{
    public class DialogInstaller : MonoInstaller
    {
        //[SerializeField, Expandable] private BaseScreenStorage storage;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DialogService>().AsSingle();
        }
    }
}