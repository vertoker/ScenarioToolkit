using UnityEngine;
using UnityEngine.InputSystem.UI;
using VRF.BNG_Framework.Scripts.UI;
using Zenject;

namespace VRF.UI.Installers
{
    public class UISystemInstaller : MonoInstaller
    {
        [SerializeField] private InputSystemUIInputModule uiSystem;

        public override void InstallBindings()
        {
            Container.InstantiatePrefab(uiSystem);
        }
    }
}