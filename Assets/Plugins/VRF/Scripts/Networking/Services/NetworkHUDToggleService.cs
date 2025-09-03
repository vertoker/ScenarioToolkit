using Mirror;
using UnityEngine;
using Zenject;

namespace VRF.Networking.Services
{
    public class NetworkHUDToggleService : ITickable
    {
        private readonly NetworkManagerHUD hud;
        private readonly KeyCode keyCode;

        public NetworkHUDToggleService(NetworkManagerHUD hud, KeyCode toggleKeyCode, bool showHUD)
        {
            this.hud = hud;
            keyCode = toggleKeyCode;
            hud.enabled = showHUD;
        }
        // TODO перенести его на InputSystem и убрать отсюда Tick
        public void Tick()
        {
            if (Input.GetKeyDown(keyCode))
                hud.enabled = !hud.enabled;
        }
    }
}