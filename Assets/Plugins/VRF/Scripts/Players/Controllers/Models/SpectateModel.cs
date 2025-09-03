using System;
using UnityEngine;
using VRF.Players.Controllers.Models.Interfaces;

namespace VRF.Players.Controllers.Models
{
    [Serializable]
    public class SpectateModel : IPlayerModel
    {
        [field:SerializeField] public bool Enabled { get; set; } = true;
        [field:SerializeField] public bool SpectateOnAdd { get; private set; } = true;
        [field:SerializeField] public bool FreeMovementOnRemove { get; private set; } = true;
    }
}