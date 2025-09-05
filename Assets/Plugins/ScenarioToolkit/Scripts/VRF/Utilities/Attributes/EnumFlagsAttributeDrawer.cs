#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VRF.Utilities.Attributes
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //var last = property.intValue;
            var next = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            property.intValue = next;
        }
    }
}
#endif