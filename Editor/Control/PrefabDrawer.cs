using UnityEditor;
using UnityEngine;
using YanickSenn.Utils.Control;

namespace YanickSenn.Utils.Editor.Control
{
    [CustomPropertyDrawer(typeof(Prefab<>))]
    public class PrefabDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var assetProperty = property.FindPropertyRelative("_asset");
            return assetProperty != null 
                ? EditorGUI.GetPropertyHeight(assetProperty, label, true) 
                : EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            var assetProperty = property.FindPropertyRelative("_asset");
            if (assetProperty != null)
            {
                EditorGUI.PropertyField(position, assetProperty, label, true);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Error: _asset property not found");
            }
            EditorGUI.EndProperty();
        }
    }
}
