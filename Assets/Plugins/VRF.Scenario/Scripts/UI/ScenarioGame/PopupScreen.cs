using System.Collections.Generic;
using NaughtyAttributes;
using SimpleUI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRF.Scenario.UI.ScenarioGame
{
    public class PopupScreen : ScreenBase
    {
        [Space]
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text buttonText;
        [SerializeField, ReadOnly] private Button[] buttons;
        
        public TMP_Text TitleComponent
        {
            get => title;
            set => title = value;
        }

        public TMP_Text DescriptionComponent
        {
            get => description;
            set => description = value;
        }

        public TMP_Text ButtonTextComponent
        {
            get => buttonText;
            set => buttonText = value;
        }

        public IReadOnlyList<Button> Buttons => buttons;

        public override void OnValidate()
        {
            base.OnValidate();
            buttons = GetComponentsInChildren<Button>();
        }
    }
}