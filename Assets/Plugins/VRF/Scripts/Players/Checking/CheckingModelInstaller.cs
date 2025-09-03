using UnityEngine;
using Zenject;

namespace VRF.Players.Checking
{
    public class CheckingModelInstaller : MonoInstaller
    {
        [SerializeField] private CheckingModel checkingModel;

        public override void InstallBindings()
        {
            Container.BindInstance(checkingModel).AsSingle();
        }
    }
}