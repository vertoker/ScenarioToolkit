using Mirror;

namespace ScenarioToolkit.Core.Systems.States
{
    public struct StatesMessage : NetworkMessage
    {
        public byte[] StatesBytes;
    }
}