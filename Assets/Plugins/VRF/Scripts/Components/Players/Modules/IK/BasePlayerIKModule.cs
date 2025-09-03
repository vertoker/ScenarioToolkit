using System;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using UnityEngine;
using VRF.Components.Players.Modules.Visuals;
using VRF.Components.Players.Views.PlayerIK;
using VRF.IK;
using VRF.Players.Services.Views;
using VRF.Utils;
using Zenject;

namespace VRF.Components.Players.Modules.IK
{
    public abstract class BasePlayerIKModule : BaseModule
    {
        [Header("Core")]
        [SerializeField] private float timeBeforeInit = 1f;
        [SerializeField] private bool syncPosition = true;
        [SerializeField] private bool syncRotation = true;
        [SerializeField] private bool syncScale = false;
        
        [Header("IK")]
        [SerializeField] private PlayerHideIK hideIK;
        [SerializeField] private PlayerFootsIK footsIK;
        [SerializeField] private RigidbodyIKReset ikReset;
        [SerializeField] private RASCALSkinnedMeshCollider skinnedCollider;
        [SerializeField] private Collider[] colliders = Array.Empty<Collider>();
        
        public bool SyncPosition => syncPosition;
        public bool SyncRotation => syncRotation;
        public bool SyncScale => syncScale;
        
        public PlayerHideIK HideIK => hideIK;
        public PlayerFootsIK FootsIK => footsIK;
        public RigidbodyIKReset IKReset => ikReset;
        
        private BasePlayerVisualsModule visuals;
        
        [Inject] public ProjectViewSpawnerService Spawner { get; private set; }

        public override void Initialize()
        {
            if (View is not BasePlayerIKView view)
            {
                Debug.LogError($"Parent view isn't a {nameof(BasePlayerIKView)}", gameObject);
                return;
            }
            
            if (!view.ControlView.TryGetComponent(out visuals))
            {
                Debug.LogError($"Couldn't find {nameof(BasePlayerVisualsModule)}", gameObject);
                return;
            }
            
            if (Initialized) return;
            base.Initialize();
            DelayInitializeIK();
        }
        
        private void Update()
        {
            if (Initialized)
            {
                UpdateIK(visuals);
            }
        }
        
        private async void DelayInitializeIK()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeBeforeInit));
            InitializeIK();
        }

        public void SelfDestroy()
        {
            Spawner.DestroyView(View);
        }
        public async void SelfDestroyDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeBeforeInit));
            SelfDestroy();
        }

        private void InitializeIK()
        {
            if (FootsIK) FootsIK.InitIK();
            if (HideIK) HideIK.EnableRenderer();
            if (IKReset) IKReset.ResetPos(false, true, false);
            
            // Коллайдер нужен только для сетевого игрока, локальному игроку это не нужно
            if (visuals.PlayerView.IsNetPlayer)
                SetActiveColliders(true);
        }
        protected override void OnValidate()
        {
            base.OnValidate();
            SetActiveColliders(false);
        }

        private void SetActiveColliders(bool active)
        {
            if (skinnedCollider)
                skinnedCollider.enabled = active;
            foreach (var col in colliders)
                if (col) col.enabled = active;
        }

        protected abstract void UpdateIK(BasePlayerVisualsModule baseModule);
    }
}