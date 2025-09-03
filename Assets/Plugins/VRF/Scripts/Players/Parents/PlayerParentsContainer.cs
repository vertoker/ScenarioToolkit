using System;
using UnityEngine;

namespace VRF.Players.Parents
{
    [Serializable]
    public class PlayerParentsContainer : MonoBehaviour
    {
        [SerializeField] private Transform mirrorPlayers;
        [SerializeField] private Transform netPlayers;
        [SerializeField] private Transform playersIK;

        public Transform MirrorPlayers => mirrorPlayers;
        public Transform NetPlayers => netPlayers;
        public Transform PlayersIK => playersIK;
    }
}