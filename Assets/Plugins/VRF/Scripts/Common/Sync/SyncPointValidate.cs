using KBCore.Refs;
using UnityEngine;
using VRF.Utilities.Extensions;

namespace VRF.Utils.Sync
{
    /// <summary>
    /// Синхронизирует transform данные для объекта в Validate
    /// </summary>
    public class SyncPointValidate : ValidatedMonoBehaviour
    {
        [SerializeField] private bool pos = true;
        [SerializeField] private bool rot = false;
        [SerializeField] private bool sca = false;
        [SerializeField] private Transform target;
        [SerializeField, Self] private Transform self;

        protected override void OnValidate()
        {
            base.OnValidate();

            if (target)
                self.SyncFrom(target, pos, rot, sca);
        }
    }
}