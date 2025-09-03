using UnityEditor;
using UnityEngine;
using VRF.VRBehaviours;

namespace VRF.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GOTransformStates))]
    public class GOTransformStatesEditor : UnityEditor.Editor
    {
        private SerializedProperty positions;
        private readonly Rect addStateButtonRect = new(60, 4, 80, 17);
        private readonly Rect nextStateButtonRect = new(60 + 80, 4, 17, 17);
        private readonly Rect addParentButtonRect = new(60 + 80 + 17, 4, 80, 17);

        public override void OnInspectorGUI()
        {
            if (GUI.Button(addStateButtonRect, "Add current"))
                foreach (var t in targets)
                    (t as GOTransformStates)!.AddCurrentState();
            if (GUI.Button(nextStateButtonRect, "+"))
                foreach (var t in targets)
                {
                    (t as GOTransformStates)!.SetStateImmediate((t as GOTransformStates)!.State + 1);
                    EditorUtility.SetDirty(t);
                }
            if (GUI.Button(addParentButtonRect, "Add parent"))
                foreach (var t in targets)
                {
                    
                }
            
            base.OnInspectorGUI();
        }
    }
}