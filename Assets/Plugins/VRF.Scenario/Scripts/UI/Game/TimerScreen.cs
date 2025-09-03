using SimpleUI.Core;
using TMPro;
using UnityEngine;

namespace VRF.Scenario.UI.Game
{
    public class TimerScreen : ScreenBase
    {
        [SerializeField] private TMP_Text timerText;
        
        public TMP_Text TextComponent => timerText;
    }
}