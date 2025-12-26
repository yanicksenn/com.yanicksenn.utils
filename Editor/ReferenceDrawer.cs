using UnityEditor;
using UnityEngine;

namespace YanickSenn.Utils.Editor
{
    [CustomPropertyDrawer(typeof(Reference<,>), true)]
    public class ReferenceDrawer : PropertyDrawer
    {
        private static readonly string[] _options = { "Use Constant", "Use Variable" };

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var useVariable = property.FindPropertyRelative("useVariable");
            var targetProperty = useVariable.boolValue 
                ? property.FindPropertyRelative("_variable") 
                : property.FindPropertyRelative("_const");
                
            return EditorGUI.GetPropertyHeight(targetProperty, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            var useVariable = property.FindPropertyRelative("useVariable");
            var constProperty = property.FindPropertyRelative("_const");
            var variableProperty = property.FindPropertyRelative("_variable");

            var buttonWidth = 18f;
            var spacing = 2f;
            
            var fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - spacing, position.height);
            var buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);

            var targetProperty = useVariable.boolValue ? variableProperty : constProperty;
            EditorGUI.PropertyField(fieldRect, targetProperty, label, true);

            if (EditorGUI.DropdownButton(buttonRect, GUIContent.none, FocusType.Passive, EditorStyles.popup))
            {
                ShowMenu(useVariable);
            }

            EditorGUI.EndProperty();
        }

        private void ShowMenu(SerializedProperty useVariableProperty)
        {
            var menu = new GenericMenu();
            
            menu.AddItem(new GUIContent(_options[0]), !useVariableProperty.boolValue, () => 
            {
                useVariableProperty.boolValue = false;
                useVariableProperty.serializedObject.ApplyModifiedProperties();
            });
            
            menu.AddItem(new GUIContent(_options[1]), useVariableProperty.boolValue, () => 
            {
                useVariableProperty.boolValue = true;
                useVariableProperty.serializedObject.ApplyModifiedProperties();
            });
            
            menu.ShowAsContext();
        }
    }
}
