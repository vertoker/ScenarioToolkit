using System;
using System.Collections.Generic;
using System.Linq;
using SimpleUI.Core;
using TMPro;
using UnityEngine;
using VRF.Players.Services.Settings;
using VRF.UI.Settings.Core;
using VRF.UI.Settings.Core.Interfaces;

namespace VRF.UI.Settings.Parameters
{
    public class InputAudioDeviceParameterView : BaseParameterView
    {
        [SerializeField] private TMP_Dropdown dropdown;

        public TMP_Dropdown Dropdown => dropdown;

        public override Type GetControllerType() => typeof(InputAudioDeviceParameterController);
    }

    public class InputAudioDeviceParameterController : BaseParameterController<InputAudioDeviceParameterView>
    {
        private readonly InputAudioDeviceParameter inputAudioDeviceParameter;
        private readonly List<string> microphoneDevices = new();

        public InputAudioDeviceParameterController(SettingsParametersController controller,
            InputAudioDeviceParameterView view, InputAudioDeviceParameter inputAudioDeviceParameter) : base(controller,
            view)
        {
            this.inputAudioDeviceParameter = inputAudioDeviceParameter;
        }

        public override void Initialize()
        {
            base.Initialize();

            View.Dropdown.onValueChanged.AddListener(OnValueChanged);
            View.Screen.Opened += RefreshMicrophones;
        }

        public override void Dispose()
        {
            base.Dispose();

            View.Dropdown.onValueChanged.RemoveListener(OnValueChanged);
            View.Screen.Opened -= RefreshMicrophones;
        }

        public override void SetupModel(IReadOnlySettingsModel model)
        {
            if (!Microphone.devices.Contains(model.MicrophoneDevice))
            {
                UpdateDropdownValue(0);
            }

            UpdateDropdownValue(Array.IndexOf(Microphone.devices, model.MicrophoneDevice));
            inputAudioDeviceParameter.Update(model.MicrophoneDevice);
        }

        private void RefreshMicrophones(ScreenBase settingsScreen)
        {
            microphoneDevices.Clear();
            View.Dropdown.ClearOptions();

            if (Microphone.devices.Length == 0)
                return;

            foreach (var microphoneDevice in Microphone.devices)
                microphoneDevices.Add(microphoneDevice);

            View.Dropdown.AddOptions(microphoneDevices);

            var micIndex = Array.IndexOf(Microphone.devices, Controller.Model.MicrophoneDevice);
            View.Dropdown.value = micIndex == -1 ? 0 : micIndex;

            View.Dropdown.RefreshShownValue();
        }

        private void OnValueChanged(int microphoneIndex)
        {
            var newDevice = Microphone.devices[microphoneIndex];
                
            inputAudioDeviceParameter.Update(newDevice);
            Controller.Model.MicrophoneDevice = newDevice;
            Controller.Save();
        }

        private void UpdateDropdownValue(int value) => 
            View.Dropdown.value = value;
    }
}