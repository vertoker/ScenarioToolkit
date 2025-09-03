using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRF.VRBehaviours.Checking;

namespace VRF.Players.Checking
{
    public class CheckingBarView : MonoBehaviour
    {
        [SerializeField] private CheckingController checkingController;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI text;

        private void OnEnable()
        {
            checkingController.CheckProgress += OnLoadingProgress;
            checkingController.CheckStarted += OnSlider;
            checkingController.CheckStopped += OffSlider;
            checkingController.CheckSuccessful += OnSuccess;
        }
        private void OnDisable()
        {
            checkingController.CheckProgress -= OnLoadingProgress;
            checkingController.CheckStarted -= OnSlider;
            checkingController.CheckStopped -= OffSlider;
            checkingController.CheckSuccessful -= OnSuccess;
        }

        private void OnLoadingProgress(float currentLoadingProgress)
        {
            slider.value = currentLoadingProgress;
            text.text = $"{(int)(currentLoadingProgress * 100)} %";
        }
        private void OnSuccess(Checkable checkable)
        {
            OnLoadingProgress(1);
        }

        private void OnSlider(Checkable checkableComponent)
        {
            slider.gameObject.SetActive(true);
        }
        private void OffSlider(Checkable checkableComponent)
        {
            slider.gameObject.SetActive(false);
        }
    }
}