using Mirror;
using VRF.Networking.Messages;

namespace VRF.Networking.Players
{
    public class NetPlayerInitializer : NetworkBehaviour
    {
        public override void OnStartClient()
        {
            
        }
        public override void OnStopClient()
        {
            
        }

        private void SendRequest()
        {
            //NetworkClient.Send(new SceneUpdateMessage());
        }
    }
}