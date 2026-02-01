using System;
using System.Collections;
using System.Reflection;
using UnityEditor;

namespace YanickSenn.Utils.Editor.Extensions
{
    public static class SerializedPropertyExtensions
    {
        public static T GetAttribute<T>(this SerializedProperty property) where T : Attribute
        {
            var fieldInfo = property.GetFieldInfo();
            return fieldInfo?.GetCustomAttribute<T>();
        }

        public static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
            var targetObject = property.serializedObject.targetObject;
            if (targetObject == null) return null;
            
            var targetType = targetObject.GetType();
            var path = property.propertyPath.Replace(".Array.data[", "[");
            var parts = path.Split('.');

            FieldInfo field = null;
            Type currentType = targetType;

            foreach (var part in parts)
            {
                if (part.Contains("["))
                {
                    var arrayName = part.Substring(0, part.IndexOf("[", StringComparison.Ordinal));
                    field = GetFieldInHierarchy(currentType, arrayName);
                    if (field == null) return null;

                    var fieldType = field.FieldType;
                    if (fieldType.IsArray)
                    {
                        currentType = fieldType.GetElementType();
                    }
                    else if (typeof(IList).IsAssignableFrom(fieldType))
                    {
                        if (fieldType.IsGenericType)
                        {
                            currentType = fieldType.GetGenericArguments()[0];
                        }
                        else
                        {
                            currentType = typeof(object);
                        }
                    }
                }
                else
                {
                    field = GetFieldInHierarchy(currentType, part);
                    if (field == null) return null;
                    currentType = field.FieldType;
                }
            }

            return field;
        }

        private static FieldInfo GetFieldInHierarchy(Type type, string fieldName)
        {
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null) return field;

            if (type.BaseType != null)
                return GetFieldInHierarchy(type.BaseType, fieldName);

            return null;
        }
    }
}
