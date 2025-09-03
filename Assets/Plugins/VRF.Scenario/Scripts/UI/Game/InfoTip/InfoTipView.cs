using System;
using SimpleUI.Core;
using SimpleUI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.UI.Game.InfoTip
{
    public class InfoTipView : UIView
    {
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private Image infoImage;
        [SerializeField] private AudioSource notificationPlayer;

        public bool IsText => infoText.gameObject.activeSelf;
        public bool IsImage => infoImage.gameObject.activeSelf;

        public TMP_Text TextComponent => infoText;
        public Image ImageComponent => infoImage;
        public AudioSource NotificationPlayer => notificationPlayer;

        public override Type GetControllerType() => typeof(InfoTipController);
    }

    public class InfoTipController : UIController<InfoTipView>, IInitializable, IDisposable
    {
        private readonly ScreenSettingsModel settingsModel;

        public InfoTipController(SignalBus bus, InfoTipView imageView,
            [InjectOptional] ScreenSettingsModel settingsModel) : base(imageView)
        {
            bus.Subscribe<SetInfoText>(SetInfo);
            bus.Subscribe<SetInfo>(SetInfo);

            this.settingsModel = settingsModel;
            if (settingsModel == null) return;

            var baseAnim = View.Screen.SpawnScreenAnim(settingsModel.AnimType);
            if (baseAnim)
            {
                baseAnim.AnimTime = settingsModel.AnimTime;
                baseAnim.EasingType = settingsModel.EasingType;
            }
        }

        public void Initialize()
        {
            View.Screen.Opened += OnOpen;
            View.Screen.Closed += OnClose;
        }

        public void Dispose()
        {
            View.Screen.Opened -= OnOpen;
            View.Screen.Closed -= OnClose;
        }

        private void OnOpen(ScreenBase screen)
        {
            if (!IsReady())
                return;

            if (settingsModel.EnableSound)
                PlaySound(settingsModel.EnableSound);
            else StopSound();
        }

        private void OnClose(ScreenBase screen)
        {
            if (!IsReady())
                return;
            if (settingsModel.DisableSound)
                PlaySound(settingsModel.DisableSound);
            else StopSound();
        }

        private void SetInfo(SetInfoText setInfo)
        {
            if (View.TextComponent)
            {
                View.TextComponent.text = setInfo.Text;
                View.TextComponent.gameObject.SetActive(true);
            }

            if (View.ImageComponent)
            {
                View.ImageComponent.gameObject.SetActive(false);
            }

            TryPlayNotification();
        }

        private void SetInfo(SetInfo setInfo)
        {
            if (View.TextComponent)
            {
                View.TextComponent.text = setInfo.Text;
                View.TextComponent.gameObject.SetActive(true);
            }

            if (View.ImageComponent)
            {
                View.ImageComponent.sprite = setInfo.Sprite;
                View.ImageComponent.gameObject.SetActive(true);
            }

            TryPlayNotification();
        }

        private void TryPlayNotification()
        {
            if (!IsReady())
                return;
            if (settingsModel.NotificationSound)
                PlaySound(settingsModel.NotificationSound);
            else StopSound();
        }

        private void StopSound()
        {
            if (View.NotificationPlayer.clip)
                View.NotificationPlayer.Stop();
            View.NotificationPlayer.clip = null;
        }

        private void PlaySound(AudioClip clip)
        {
            View.NotificationPlayer.clip = clip;
            View.NotificationPlayer.Play();
        }

        private bool IsReady() =>
            View && View.NotificationPlayer
                 && View.gameObject.activeInHierarchy
                 && settingsModel is { UseOnEnable: true };
    }
}