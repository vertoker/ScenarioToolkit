using SimpleUI.Core;
using UnityEngine;
using VRF.UI.Templates;

namespace VRF.UI.GameMenu.Screens
{
    public class ExitScreen : ScreenBase
    {
        [field:Space]
        [field:SerializeField] public GameObject NoButton { get; private set; }
        
        [field:SerializeField] public GameObject YesGameButton { get; private set; }
        
        [field:SerializeField] public GameObject YesMenuButton { get; private set; }
        [field:SerializeField] public SceneSwitchButtonView SceneSwitch { get; private set; }
    }
}