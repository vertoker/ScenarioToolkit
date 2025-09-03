using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Executors.Movement.Interfaces;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Enums;
using Zenject;

namespace VRF.Players.Controllers.Executors.Management
{
    public class PlayerModeContainer : BaseModelExecutor<PlayerModeModel>,
        IPlayerModeContainer, IInitializable, IDisposable
    {
        private readonly InputAction switchMode;
        private readonly Dictionary<PlayerMode, List<IModelExecutor>> executors;

        public PlayerMode CurrentMode { get; private set; }
        public IEnumerable<PlayerMode> AvailableModes => executors.Keys;


        public event Action<PlayerMode> OnEnable;
        public event Action OnDisable;

        public PlayerModeContainer(PlayerModeModel model, InputAction switchMode) : base(model)
        {
            this.switchMode = switchMode;
            executors = new Dictionary<PlayerMode, List<IModelExecutor>>();
            CurrentMode = Model.ModeOnStart;
        }


        public override void Enable()
        {
            base.Enable();
            
            if (TryGetExecutorsInternal(CurrentMode, out var executorsList))
            {
                EnableInternal(executorsList);
            }
        }
        public override void Disable()
        {
            base.Disable();
            DisableInternal();
        }

        public void Add(IPlayerModeExecutor executor)
        {
            Add(executor.ExecutableMode, executor);
        }
        public void Add(PlayerMode mode, IModelExecutor executor)
        {
            if (!executors.TryGetValue(mode, out var list))
            {
                list = new List<IModelExecutor>();
                executors[mode] = list;
            }
            
            list.Add(executor);
        }

        public void Initialize()
        {
            switchMode.performed += SwitchPerformed;
            switchMode.Enable();
        }
        public void Dispose()
        {
            switchMode.performed -= SwitchPerformed;
            switchMode.Disable();
        }
        
        private void SwitchPerformed(InputAction.CallbackContext ctx) => SwitchNext();
        
        public void SwitchNext()
        {
            SwitchTo(Model.ToggleSwitchMode(CurrentMode));
        }
        public void SwitchTo(PlayerMode playerMode)
        {
            Debug.Log($"{nameof(SwitchTo)} {playerMode.ToString()}");
            
            if (TryGetExecutorsInternal(playerMode, out var executorsList))
            {
                DisableInternal();
                CurrentMode = playerMode;
                EnableInternal(executorsList);
            }
        }

        private void DisableInternal()
        {
            foreach (var executor in executors[CurrentMode])
                executor.Disable();
            OnDisable?.Invoke();
        }
        private bool TryGetExecutorsInternal(PlayerMode playerMode, out List<IModelExecutor> executorsList)
        {
            if (!executors.TryGetValue(playerMode, out executorsList))
            {
                Debug.LogError($"Can't find {playerMode} mode in {nameof(PlayerModeContainer)}, drop enable");
                return false;
            }
            return true;
        }
        private void EnableInternal(IEnumerable<IModelExecutor> executorsEnumerable)
        {
            foreach (var executor in executorsEnumerable)
                executor.Enable();
            OnEnable?.Invoke(CurrentMode);
        }
    }
}