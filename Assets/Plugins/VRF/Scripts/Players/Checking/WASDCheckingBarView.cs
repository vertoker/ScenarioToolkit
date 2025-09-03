using System;
using SimpleUI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRF.Components.Players.Views.Player;
using VRF.Players.Core;
using VRF.Utilities;
using VRF.VRBehaviours.Checking;
using Zenject;

namespace VRF.Players.Checking
{
    public class WASDCheckingBarView : UIView
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text text;

        public Slider Slider => slider;
        public TMP_Text Text => text;

        public override Type GetControllerType() => typeof(WASDCheckingBarController);
    }

    public class WASDCheckingBarController : UIController<WASDCheckingBarView>, IInitializable, IDisposable
    {
        private readonly PlayersContainer playersContainer;
        private CheckingController checkingController;
        
        public WASDCheckingBarController(WASDCheckingBarView view, PlayersContainer playersContainer) : base(view)
        {
            this.playersContainer = playersContainer;
            
            if (playersContainer.CurrentValue.View is PlayerWASDView playerWASD)
            {
                checkingController = playerWASD.CheckingController;
            }
            else
            {
                playersContainer.PlayerChanged += PlayerChanged;
            }
        }

        private void PlayerChanged()
        {
            if (!playersContainer.CurrentKey.Mode.IsWASD()) return;
            checkingController = ((PlayerWASDView)playersContainer.CurrentValue.View).CheckingController;
            Initialize();
            playersContainer.PlayerChanged -= PlayerChanged;
        }

        public void Initialize()
        {
            View.Slider.gameObject.SetActive(false);
            if (!checkingController) return;
            
            checkingController.CheckProgress += OnLoadingProgress;
            checkingController.CheckStarted += OnSlider;
            checkingController.CheckStopped += OffSlider;
            checkingController.CheckSuccessful += OnSuccess;
        }
        public void Dispose()
        {
            if (!checkingController) return;
            
            checkingController.CheckProgress -= OnLoadingProgress;
            checkingController.CheckStarted -= OnSlider;
            checkingController.CheckStopped -= OffSlider;
            checkingController.CheckSuccessful -= OnSuccess;
        }

        private void OnLoadingProgress(float currentLoadingProgress)
        {
            View.Slider.value = currentLoadingProgress;
            View.Text.text = $"{(int)(currentLoadingProgress * 100)} %";
        }
        private void OnSuccess(Checkable checkable)
        {
            OnLoadingProgress(1);
        }
        
        private void OnSlider(Checkable checkableComponent)
        {
            View.Slider.gameObject.SetActive(true);
        }
        private void OffSlider(Checkable checkableComponent)
        {
            View.Slider.gameObject.SetActive(false);
        }
    }
}