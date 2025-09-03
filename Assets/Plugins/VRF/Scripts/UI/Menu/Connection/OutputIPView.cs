using System;
using System.Net;
using System.Net.Sockets;
using SimpleUI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRF.Networking.Core;
using Zenject;

namespace VRF.UI.Menu.Connection
{
    public class OutputIPView : UIView
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;

        public TMP_Text Text => text;

        public Button Button => button;

        public override Type GetControllerType() => typeof(OutputIPController);
    }

    public class OutputIPController : UIController<OutputIPView>, IInitializable, IDisposable
    {
        private readonly VRFNetworkManager networkManager;
        private readonly string ipAddress;
        
        public OutputIPController(OutputIPView view, VRFNetworkManager networkManager) : base(view)
        {
            this.networkManager = networkManager;
            ipAddress = GetLocalIPAddress();
        }

        public void Initialize()
        {
            View.Button.onClick.AddListener(ConnectAsHost);
            
            View.Text.text = ipAddress;
        }
        public void Dispose()
        {
            View.Button.onClick.RemoveListener(ConnectAsHost);
        }

        private void ConnectAsHost()
        {
            networkManager.StartHost();
        }
        
        // https://stackoverflow.com/questions/6803073/get-local-ip-address
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}