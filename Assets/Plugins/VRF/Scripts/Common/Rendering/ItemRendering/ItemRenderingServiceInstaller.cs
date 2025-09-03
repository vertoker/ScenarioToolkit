using UnityEngine;
using Zenject;

namespace VRF.Utils.Rendering.ItemRendering
{
    public class ItemRenderingServiceInstaller : MonoInstaller
    {
        [SerializeField] private Camera renderCamera;
        [SerializeField] private Vector2Int textureSize;
        [SerializeField] private Vector3 cameraPosition = new(0, -1000, 0);

        public override void InstallBindings()
        {
            Container.Bind<ItemRenderingService>().AsSingle().WithArguments(renderCamera, textureSize, cameraPosition);
        }
    }
}