using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities
{
    public static class VrfLayerMask
    {
        // Да тут 31 бит, потому что он кодирует число без знака
        //public static readonly LayerMask EverythingMask2 = 0b1111111111111111111111111111111;
        
        public static readonly LayerMask EverythingMask = unchecked((int)0xFFFFFFFF); // -1
        //public static readonly LayerMask DefaultMask = 0x00000000;
        public static readonly LayerMask NothingMask = 0x00000000;
    }
}