using System;
using SimpleUI.Core;
using UnityEngine;

namespace VRF.UI.GameMenu
{
    public class GameMenuView : UIView
    {
        [field:SerializeField] public AudioSource NotificationPlayer { get; private set; }
        [field:Space]
        [field:SerializeField] public GameObject InventoryBtn { get; private set; }
        [field:SerializeField] public GameObject DialogBtn { get; private set; }
        [field:Space]
        [field:SerializeField] public GameObject ControlsBtn { get; private set; }
        [field:SerializeField] public GameObject AboutBtn { get; private set; }
        [field:Space]
        [field:SerializeField] public GameObject SettingsBtn { get; private set; }
        [field:SerializeField] public GameObject ExitGameBtn { get; private set; }
        [field:SerializeField] public GameObject ExitMenuBtn { get; private set; }
        
        public override Type GetControllerType() => typeof(GameMenuController);
    }
}