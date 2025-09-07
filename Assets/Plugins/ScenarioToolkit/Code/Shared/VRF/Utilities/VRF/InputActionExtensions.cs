using System;
using UnityEngine.InputSystem;

namespace ScenarioToolkit.Shared.VRF.Utilities.VRF
{
    /// <summary>
    /// Расширения для удобной подписки и отписки на события InputAction
    /// </summary>
    public static class InputActionExtensions
    {
        public static void Subscribe(this InputAction action,
            Action<InputAction.CallbackContext> performed,
            Action<InputAction.CallbackContext> canceled)
        {
            action.performed += performed;
            action.canceled += canceled;
        }
        public static void Unsubscribe(this InputAction action,
            Action<InputAction.CallbackContext> performed,
            Action<InputAction.CallbackContext> canceled)
        {
            action.performed -= performed;
            action.canceled -= canceled;
        }
        
        public static void Subscribe(this InputAction action, ButtonPressMode pressMode,
            Action<InputAction.CallbackContext> performed,
            Action<InputAction.CallbackContext> cancelled,
            Action<InputAction.CallbackContext> toggle)
        {
            switch (pressMode)
            {
                case ButtonPressMode.Hold:
                    action.performed += performed;
                    action.canceled += cancelled;
                    break;
                case ButtonPressMode.Press:
                    action.performed += toggle;
                    break;
                case ButtonPressMode.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pressMode), pressMode, null);
            }
        }
        public static void Unsubscribe(this InputAction action, ButtonPressMode pressMode,
            Action<InputAction.CallbackContext> performed,
            Action<InputAction.CallbackContext> cancelled,
            Action<InputAction.CallbackContext> toggle)
        {
            switch (pressMode)
            {
                case ButtonPressMode.Hold:
                    action.performed -= performed;
                    action.canceled -= cancelled;
                    break;
                case ButtonPressMode.Press:
                    action.performed -= toggle;
                    break;
                case ButtonPressMode.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pressMode), pressMode, null);
            }
        }
    }
}