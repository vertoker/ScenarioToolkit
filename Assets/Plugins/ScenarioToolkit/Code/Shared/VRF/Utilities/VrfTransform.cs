using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities
{
    public static class VrfTransform
    {
        public static void SwapSublingIndexes(Transform tr1, Transform tr2)
        {
            var index1 = tr1.GetSiblingIndex(); // 0
            var index2 = tr2.GetSiblingIndex(); // 2
            
            if (index2 == index1) return;
            if (index2 > index1)
            {
                tr1.SetSiblingIndex(index2);
                tr2.SetSiblingIndex(index1);
            }
            else
            {
                tr2.SetSiblingIndex(index1);
                tr1.SetSiblingIndex(index2);
            }
        }
    }
}