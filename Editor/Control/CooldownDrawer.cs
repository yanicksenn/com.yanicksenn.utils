using UnityEditor;
using UnityEngine;
using YanickSenn.Utils.Control;

namespace YanickSenn.Utils.Editor.Control
{
    [CustomPropertyDrawer(typeof(Cooldown))]
    public class CooldownDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var durationProperty = property.FindPropertyRelative("duration");
            return durationProperty != null ? EditorGUI.GetPropertyHeight(durationProperty, label, true) : EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label = EditorGUI.BeginProperty(position, label, property);
            var durationProperty = property.FindPropertyRelative("duration");
            EditorGUI.PropertyField(position, durationProperty, label, true);
            EditorGUI.EndProperty();
        }
    }
}
