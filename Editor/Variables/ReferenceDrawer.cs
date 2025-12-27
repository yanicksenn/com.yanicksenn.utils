using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace YanickSenn.Utils.Editor.Variables
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
            // Handle context menu
            if (Event.current.type == EventType.ContextClick && position.Contains(Event.current.mousePosition))
            {
                var useVariable = property.FindPropertyRelative("useVariable");
                // Only active if const value is active (useVariable is false)
                if (!useVariable.boolValue) 
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Create Variable from Value"), false, () => CreateVariable(property));
                    menu.ShowAsContext();
                    Event.current.Use();
                }
            }

            label = EditorGUI.BeginProperty(position, label, property);

            var useVariableProp = property.FindPropertyRelative("useVariable");
            var constProperty = property.FindPropertyRelative("_const");
            var variableProperty = property.FindPropertyRelative("_variable");

            var buttonWidth = 18f;
            var spacing = 2f;
            
            var fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - spacing, position.height);
            var buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);

            var targetProperty = useVariableProp.boolValue ? variableProperty : constProperty;
            EditorGUI.PropertyField(fieldRect, targetProperty, label, true);

            if (EditorGUI.DropdownButton(buttonRect, GUIContent.none, FocusType.Passive, EditorStyles.popup))
            {
                ShowMenu(useVariableProp);
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

        private void CreateVariable(SerializedProperty property)
        {
            // 1. Get the generic types
            var referenceInstance = GetTargetObjectOfProperty(property);
            if (referenceInstance == null)
            {
                Debug.LogError("Could not find reference instance.");
                return;
            }

            var referenceType = referenceInstance.GetType();
            var variableType = GetVariableType(referenceType);

            if (variableType == null)
            {
                Debug.LogError($"Could not determine Variable type for {referenceType}.");
                return;
            }

            // 2. Ask for save path
            var snakeCaseName = CreateSnakeCaseName(property);
            var path = EditorUtility.SaveFilePanelInProject("Save Variable", snakeCaseName, "asset", "Save the new variable asset");
            if (string.IsNullOrEmpty(path)) return;

            // 3. Create instance
            var newVariable = ScriptableObject.CreateInstance(variableType);

            // 4. Set value
            var constProp = property.FindPropertyRelative("_const");
            var newVariableSo = new SerializedObject(newVariable);
            var valueProp = newVariableSo.FindProperty("value");
            
            valueProp.boxedValue = constProp.boxedValue;
            newVariableSo.ApplyModifiedProperties();

            // 5. Save asset
            AssetDatabase.CreateAsset(newVariable, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // 6. Assign to property
            property.FindPropertyRelative("_variable").objectReferenceValue = newVariable;
            property.FindPropertyRelative("useVariable").boolValue = true;
            property.serializedObject.ApplyModifiedProperties();
        }

        private static string CreateSnakeCaseName(SerializedProperty property)
        {
            var rawName = property.displayName;
            var target = property.serializedObject.targetObject;

            if (target is Component component)
            {
                rawName = component.gameObject.name + " " + rawName;
            }
            else if (target is GameObject go)
            {
                rawName = go.name + " " + rawName;
            }

            var parts = rawName.Split(new[] { ' ', '-', '_', '.', ':' }, StringSplitOptions.RemoveEmptyEntries);
            var snakeCaseParts = new string[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                snakeCaseParts[i] = parts[i].ToLowerInvariant();
            }

            return string.Join("_", snakeCaseParts);
        }

        private Type GetVariableType(Type referenceType)
        {
            var currentType = referenceType;
            while (currentType != null)
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(Reference<,>))
                {
                    return currentType.GetGenericArguments()[1];
                }
                currentType = currentType.BaseType;
            }
            return null;
        }

        // Reflection helper
        private static object GetTargetObjectOfProperty(SerializedProperty property)
        {
            if (property == null) return null;

            var path = property.propertyPath.Replace(".Array.data[", "[");
            object obj = property.serializedObject.targetObject;
            var elements = path.Split('.');

            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            return obj;
        }

        private static object GetValue_Imp(object source, string name)
        {
            if (source == null) return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null) return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null) return p.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }

        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            
            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }
    }
}