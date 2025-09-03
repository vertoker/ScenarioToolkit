using SimpleUI.Core;
using TMPro;
using UnityEngine;

namespace VRF.UI.CameraMessage
{
    public class CameraMessageScreen : ScreenBase
    {
        [SerializeField] private TMP_Text messageText; 
    
        public void SetMessage(string message)
        {
            messageText.text = message;
        }
    }
}