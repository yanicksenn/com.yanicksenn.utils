using UnityEditor;
using UnityEngine;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Editor.Misc
{
    [CustomPropertyDrawer(typeof(Duration))]
    public class DurationDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var valueProp = property.FindPropertyRelative("value");
            var unitProp = property.FindPropertyRelative("unit");

            float unitWidth = 90f;
            float spacing = 2f;
            float valueWidth = position.width - unitWidth - spacing;

            var valueRect = new Rect(position.x, position.y, valueWidth, position.height);
            var unitRect = new Rect(position.x + valueWidth + spacing, position.y, unitWidth, position.height);

            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
            EditorGUI.PropertyField(unitRect, unitProp, GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}