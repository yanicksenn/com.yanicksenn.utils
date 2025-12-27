using UnityEditor;
using UnityEngine;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Editor {
    [CustomPropertyDrawer(typeof(Vector3Mask))]
    public class Vector3MaskDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            Rect contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float toggleSize = 16f;
            float labelTextWidth = 15f;
            float spacing = 5f;

            float currentX = contentPosition.x;

            // X
            Rect xToggleRect = new Rect(currentX, contentPosition.y, toggleSize, contentPosition.height);
            SerializedProperty propX = property.FindPropertyRelative("x");
            propX.boolValue = EditorGUI.Toggle(xToggleRect, propX.boolValue);
            currentX += toggleSize;

            Rect xLabelRect = new Rect(currentX, contentPosition.y, labelTextWidth, contentPosition.height);
            EditorGUI.LabelField(xLabelRect, "X");
            currentX += labelTextWidth + spacing;

            // Y
            Rect yToggleRect = new Rect(currentX, contentPosition.y, toggleSize, contentPosition.height);
            SerializedProperty propY = property.FindPropertyRelative("y");
            propY.boolValue = EditorGUI.Toggle(yToggleRect, propY.boolValue);
            currentX += toggleSize;

            Rect yLabelRect = new Rect(currentX, contentPosition.y, labelTextWidth, contentPosition.height);
            EditorGUI.LabelField(yLabelRect, "Y");
            currentX += labelTextWidth + spacing;

            // Z
            Rect zToggleRect = new Rect(currentX, contentPosition.y, toggleSize, contentPosition.height);
            SerializedProperty propZ = property.FindPropertyRelative("z");
            propZ.boolValue = EditorGUI.Toggle(zToggleRect, propZ.boolValue);
            currentX += toggleSize;

            Rect zLabelRect = new Rect(currentX, contentPosition.y, labelTextWidth, contentPosition.height);
            EditorGUI.LabelField(zLabelRect, "Z");

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}