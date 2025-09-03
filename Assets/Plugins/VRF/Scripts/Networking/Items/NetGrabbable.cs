using Mirror;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;

namespace VRF.Networking.Items
{
    [RequireComponent(typeof(Grabbable))]
    public class NetGrabbable : NetworkBehaviour
    {
        [SerializeField] private bool debug;

        [SerializeField, NaughtyAttributes.ReadOnly]
        private Grabbable grabbable;

        protected override void OnValidate()
        {
            base.OnValidate();

            grabbable = GetComponent<Grabbable>();
            //grabbable.enabled = false;
        }

        private void OnEnable()
        {
            grabbable.OnGrabbed += OnGrabbed;
            grabbable.OnDropped += OnDropped;
        }
        private void OnDisable()
        {
            grabbable.OnGrabbed -= OnGrabbed;
            grabbable.OnDropped -= OnDropped;
        }

        private void OnGrabbed()
        {
            // TODO если схватит 2 игрок, то это может вызвать ошибку

            if (debug) Debug.Log($"{nameof(OnGrabbed)} {name}");
            CmdStartAuthority();
        }
        private void OnDropped()
        {
            if (debug) Debug.Log($"{nameof(OnDropped)} {name}");
            //CmdStopAuthority();
        }

        [Command(requiresAuthority = false)]
        private void CmdStartAuthority(NetworkConnectionToClient sender = null)
        {
            netIdentity.RemoveClientAuthority();
            netIdentity.AssignClientAuthority(sender);
            if (debug) Debug.Log($"{nameof(CmdStartAuthority)}");
        }

        [Command(requiresAuthority = false)]
        private void CmdStopAuthority()
        {
            netIdentity.RemoveClientAuthority();
            if (debug) Debug.Log($"{nameof(CmdStopAuthority)}");
        }

        // TODO можно схватить предмет, который не принадлежит ему (локальному игроку)
        public override void OnStartAuthority()
        {
            //grabbable.enabled = true;
        }
        public override void OnStopAuthority()
        {
            //grabbable.enabled = false;
        }
    }
}