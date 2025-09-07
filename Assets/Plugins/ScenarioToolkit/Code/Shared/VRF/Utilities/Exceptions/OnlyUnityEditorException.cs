using System;

namespace ScenarioToolkit.Shared.VRF.Utilities.Exceptions
{
    /// <summary>
    /// Сокращение для частного вида ошибки NotImplementedException, что объект работает только в Unity Editor
    /// </summary>
    public class OnlyUnityEditorException : NotImplementedException
    {
        public const string ErrorMessage = "Behaviour implemented only in Unity Editor";
        
        public OnlyUnityEditorException() : base(ErrorMessage)
        {
            
        }
    }
}