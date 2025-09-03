using NaughtyAttributes;
using UnityEngine;
using VRF.Inventory.Scriptables;
using Zenject;

namespace VRF.Test
{
    public class TestEquipmentInstaller : MonoInstaller
    {
        [SerializeField] private InventoryItemConfig itemConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<TestEquipmentService>().AsSingle().WithArguments(itemConfig);
        }
        
#if UNITY_EDITOR
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void AddSnapCallback()
        {
            Container.Resolve<TestEquipmentService>().AddSnapCallback();
        }
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void AddDetachCallback()
        {
            Container.Resolve<TestEquipmentService>().AddDetachCallback();
        }
#endif
    }
}