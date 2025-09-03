using NaughtyAttributes;
using UnityEngine;
using VRF.Identities.Init;
using VRF.Players.Scriptables;
using VRF.Utilities;
using Zenject;

namespace VRF.Identities.Core
{
    public class IdentityInstaller : MonoInstaller
    {
        [SerializeField, Required] private PlayerIdentityConfig defaultIdentity;
        [SerializeField, Required] private IdentitiesConfig identities;
        
        public IdentitiesConfig Identities => identities;

        #region Preset Editor
        private bool IdentitiesIsNull => !identities;
        private bool DefaultIdentityIsNull => !defaultIdentity;

#if UNITY_EDITOR
        [Button, ShowIf(nameof(IdentitiesIsNull))]
        private void CreateDefaultIdentities()
        {
            ScriptableTools.CopyFolderEditor("Assets/Modules/VRF/Configs/Identities/", "Assets/Configs/VRF/Identities/");
            ScriptableTools.CreatePresetEditor(ref identities, "Assets/Configs/VRF/Identities/", "Default_Identities");
            UnityEditor.EditorUtility.SetDirty(this);
        }
        [Button, ShowIf(nameof(DefaultIdentityIsNull))]
        private void CreateDefaultIdentity()
        {
            ScriptableTools.CreatePresetEditor(ref defaultIdentity, "Assets/Configs/VRF/Identities/", "Identity_All");
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        #endregion
        
        public override void InstallBindings()
        {
            Container.EnsureBind(defaultIdentity);
            
            // Core
            if (identities)
                Container.BindInstance(identities).AsSingle();
            Container.Bind<IdentityService>().AsSingle();
        }
    }
}