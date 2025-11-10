using UnityEditor;
using UnityEngine;

namespace YanickSenn.Utils.Editor
{
    [CustomPropertyDrawer(typeof(Observable<>))]
    public class ObservableDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var valueProperty = property.FindPropertyRelative("value");
            return valueProperty != null ? EditorGUI.GetPropertyHeight(valueProperty, label, true) : EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label = EditorGUI.BeginProperty(position, label, property);
            var valueProperty = property.FindPropertyRelative("value");
            EditorGUI.PropertyField(position, valueProperty, label, true);
            EditorGUI.EndProperty();
        }
    }
}
