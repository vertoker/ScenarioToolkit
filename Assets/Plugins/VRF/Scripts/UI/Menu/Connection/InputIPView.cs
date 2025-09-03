using System;
using System.Net;
using Mirror;
using SimpleUI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRF.Networking.Core;
using Zenject;

namespace VRF.UI.Menu.Connection
{
    public class InputIPView : UIView
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button button;

        public TMP_InputField InputField => inputField;
        public Button Button => button;

        public override Type GetControllerType() => typeof(InputIPController);
    }

    public class InputIPController : UIController<InputIPView>, IInitializable, IDisposable
    {
        private readonly VRFNetworkManager networkManager;

        public InputIPController(InputIPView view, VRFNetworkManager networkManager) : base(view)
        {
            this.networkManager = networkManager;
        }

        public void Initialize()
        {
            View.Button.onClick.AddListener(ConnectAsClient);
        }
        public void Dispose()
        {
            View.Button.onClick.RemoveListener(ConnectAsClient);
        }
        
        private void ConnectAsClient()
        {
            var ip = View.InputField.text;
            StartClient(ip);
        }
        
        private bool StartClient(string address)
        {
            if (NetworkClient.active) return false;
            
            if (IPAddress.TryParse(address, out _))
                networkManager.networkAddress = address;
            else return false;
            
            if (Transport.active is PortTransport portTransport)
                portTransport.Port = portTransport.Port;
            else return false;
            
            networkManager.StartClient();
            return true;
        }
    }
}