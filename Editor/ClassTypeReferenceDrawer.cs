using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace YanickSenn.Utils.Editor
{
    [CustomPropertyDrawer(typeof(ClassTypeReference))]
    public class ClassTypeReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            var assemblyQualifiedNameProperty = property.FindPropertyRelative("_assemblyQualifiedName");
            var currentTypeName = GetTypeName(assemblyQualifiedNameProperty.stringValue);
            
            if (EditorGUI.DropdownButton(position, new GUIContent(currentTypeName), FocusType.Keyboard))
            {
                ShowDropdown(position, assemblyQualifiedNameProperty);
            }
            
            EditorGUI.EndProperty();
        }

        private string GetTypeName(string assemblyQualifiedName)
        {
            if (string.IsNullOrEmpty(assemblyQualifiedName)) return "(None)";
            var type = Type.GetType(assemblyQualifiedName);
            return type != null ? type.Name : "(Missing Type)";
        }

        private void ShowDropdown(Rect rect, SerializedProperty property)
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(ClassTypeConstraintAttribute), true);
            var baseType = attributes.Length > 0 
                ? ((ClassTypeConstraintAttribute)attributes[0]).BaseType 
                : typeof(UnityEngine.Object);
            
            var allowAbstract = attributes.Length > 0 && ((ClassTypeConstraintAttribute)attributes[0]).AllowAbstract;

            var dropdown = new ClassTypeReferenceDropdown(new AdvancedDropdownState(), baseType, allowAbstract);
            dropdown.OnItemSelected += (type) =>
            {
                property.stringValue = type?.AssemblyQualifiedName ?? "";
                property.serializedObject.ApplyModifiedProperties();
            };
            dropdown.Show(rect);
        }
    }

    public class ClassTypeReferenceDropdown : AdvancedDropdown
    {
        public event Action<Type> OnItemSelected;
        
        private readonly Type _baseType;
        private readonly bool _allowAbstract;

        public ClassTypeReferenceDropdown(AdvancedDropdownState state, Type baseType, bool allowAbstract) : base(state)
        {
            _baseType = baseType;
            _allowAbstract = allowAbstract;
            
            // Set minimum size
            minimumSize = new Vector2(0, 300);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("Types");
            
            // Use TypeCache for performance
            var types = TypeCache.GetTypesDerivedFrom(_baseType)
                .Where(t => t.IsVisible && !t.IsGenericType);
                
            if (!_allowAbstract)
            {
                types = types.Where(t => !t.IsAbstract);
            }

            var typesList = types.ToList();
            
            // Add base type if applicable
            if ((_allowAbstract || !_baseType.IsAbstract) && _baseType.IsVisible && !_baseType.IsGenericType)
            {
                typesList.Add(_baseType);
            }
            
            typesList.Sort((a, b) => string.Compare(a.FullName, b.FullName, StringComparison.Ordinal));

            // Populate the tree
            foreach (var type in typesList)
            {
                if (string.IsNullOrEmpty(type.FullName)) continue;
                
                // Exclude Unity internal types or non-relevant ones if needed, but let's keep it broad for now.
                
                var parts = type.FullName.Split('.');
                var parent = root;
                
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    var part = parts[i];
                    var child = parent.children.FirstOrDefault(c => c.name == part);
                    if (child == null)
                    {
                        child = new AdvancedDropdownItem(part)
                        {
                            icon = EditorGUIUtility.FindTexture("Folder Icon")
                        };
                        parent.AddChild(child);
                    }
                    parent = child;
                }
                
                var item = new ClassTypeReferenceDropdownItem(parts.Last(), type);
                // Try to find an icon for the type
                Texture2D icon = null;
                try {
                    // ObjectContent might return null or default
                    // EditorGUIUtility.ObjectContent(null, type).image; // This requires an instance for some lookups, or type.
                    // Another way is AssetPreview.GetMiniTypeThumbnail(type)
                    icon = AssetPreview.GetMiniTypeThumbnail(type);
                } catch { /* ignore */ }
                
                if (icon == null) icon = EditorGUIUtility.FindTexture("cs Script Icon");
                
                item.icon = icon;
                
                parent.AddChild(item);
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is ClassTypeReferenceDropdownItem typeItem)
            {
                OnItemSelected?.Invoke(typeItem.Type);
            }
        }
    }

    public class ClassTypeReferenceDropdownItem : AdvancedDropdownItem
    {
        public Type Type { get; }

        public ClassTypeReferenceDropdownItem(string name, Type type) : base(name)
        {
            Type = type;
        }
    }
}
