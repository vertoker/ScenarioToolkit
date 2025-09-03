using System;
using Mirror;
using ModestTree;
using UnityEngine;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Players.Scriptables;
using VRF.Scenario.Systems;
using VRF.VRBehaviours.TerrainSystem;
using Zenject;
using Random = UnityEngine.Random;

namespace VRF.Scenario.MonoBehaviours
{
    public class TerrainMoveNetwork : NetworkBehaviour
    {
        [SerializeField] private PlayerIdentityConfig[] priorities = Array.Empty<PlayerIdentityConfig>();

        private int randomId;
        private TerrainLoader loader;
        private TerrainMoveSystem move;


        [Inject]
        public void Construct(TerrainLoader terrainLoaderSystem,
                              TerrainMoveSystem terrainMoveSystem,
                              IdentityService identityService)
        {
            loader = terrainLoaderSystem;
            move = terrainMoveSystem;
            randomId = priorities.IndexOf(identityService.SelfIdentity);

            move.SetSpeedOverTime += (_, _) => { Send(loader.CurrentPosition); };
            move.SetPosition += Send;
        }

        [Client]
        private void Send(float position)
        {
            CmdSendMessage(randomId, position);
        }

        [Command(requiresAuthority = false)]
        private void CmdSendMessage(int senderID, float position)
        {
            RpcHandleMessage(senderID, position);
        }

        [ClientRpc]
        private void RpcHandleMessage(int senderID, float position)
        {
            if (senderID <= randomId)
                return; //random authority
            loader.SetCurrentPosition(position);
        }
    }
}