using Mirror;

namespace Scenario.Core.Systems.States
{
    public struct StatesMessage : NetworkMessage
    {
        public byte[] StatesBytes;
    }
}