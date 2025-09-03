using System;
using UnityEngine;
using UnityEngine.UI;
using VRF.Players.Services.Settings;
using VRF.UI.Settings.Core;
using VRF.UI.Settings.Core.Interfaces;

namespace VRF.UI.Settings.Parameters
{
    public class MouseSensitivityParameterView : BaseParameterView
    {
        [SerializeField] private Slider sliderX, sliderY;

        public Slider SliderX => sliderX;
        public Slider SliderY => sliderY;
        
        public override Type GetControllerType() => typeof(MouseSensitivityParameterController);
    }

    /// <summary>
    /// Контроллер для прокидывания чувствительности мышки в специальный сервис вне UI систем
    /// </summary>
    public class MouseSensitivityParameterController : BaseParameterController<MouseSensitivityParameterView>
    {
        private readonly MouseSensitivityParameter mouseSensitivity;

        public MouseSensitivityParameterController(MouseSensitivityParameter mouseSensitivity,
            SettingsParametersController controller, MouseSensitivityParameterView view) : base(controller, view)
        {
            this.mouseSensitivity = mouseSensitivity;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            View.SliderX.onValueChanged.AddListener(OnValueChangedX);
            View.SliderY.onValueChanged.AddListener(OnValueChangedY);
        }
        public override void Dispose()
        {
            base.Dispose();
            
            View.SliderX.onValueChanged.RemoveListener(OnValueChangedX);
            View.SliderY.onValueChanged.RemoveListener(OnValueChangedY);
        }
        
        public override void SetupModel(IReadOnlySettingsModel model)
        {
            View.SliderX.value = model.MouseSensitivityX;
            View.SliderY.value = model.MouseSensitivityY;
            var sensitivity = new Vector2(model.MouseSensitivityX, model.MouseSensitivityY);
            // Прокидывание данных во внешний мир
            mouseSensitivity.Update(sensitivity);
        }
        
        private void OnValueChangedX(float valueX)
        {
            // Данные сразу же сохраняются в модель для сохранения, так просто удобнее
            Controller.Model.MouseSensitivityX = valueX;
            OnValueChanged();
        }
        private void OnValueChangedY(float valueY)
        {
            Controller.Model.MouseSensitivityY = valueY;
            OnValueChanged();
        }
        private void OnValueChanged()
        {
            var sensitivity = new Vector2(Controller.Model.MouseSensitivityX,
                Controller.Model.MouseSensitivityY);
            // Прокидывание данных во внешний мир
            mouseSensitivity.Update(sensitivity);
            // Попытка сохранить новые данные
            Controller.Save();
        }
    }
}