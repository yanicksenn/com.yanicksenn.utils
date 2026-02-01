using UnityEditor;
using UnityEngine;
using YanickSenn.Utils.Misc;
using YanickSenn.Utils.Editor.Extensions;

namespace YanickSenn.Utils.Editor.Misc
{
    [InitializeOnLoad]
    public static class DrawHandleSystem
    {
        static DrawHandleSystem()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (Selection.gameObjects.Length == 0) return;

            foreach (var gameObject in Selection.gameObjects)
            {
                if (gameObject == null) continue;

                var components = gameObject.GetComponents<MonoBehaviour>();
                foreach (var component in components)
                {
                    if (component == null) continue;
                    DrawHandlesForComponent(component);
                }
            }
        }

        private static void DrawHandlesForComponent(MonoBehaviour component)
        {
            var so = new SerializedObject(component);
            var prop = so.GetIterator();
            var enterChildren = true;

            while (prop.NextVisible(enterChildren))
            {
                if (prop.propertyType == SerializedPropertyType.Vector3)
                {
                    var attr = prop.GetAttribute<DrawHandleAttribute>();
                    if (attr != null)
                    {
                        DrawPositionHandle(component.transform, prop, attr);
                    }
                    enterChildren = false;
                }
                else if (prop.propertyType == SerializedPropertyType.Quaternion)
                {
                    var attr = prop.GetAttribute<DrawHandleAttribute>();
                    if (attr != null)
                    {
                        DrawRotationHandle(component.transform, prop, attr);
                    }
                    enterChildren = false;
                }
                else
                {
                    enterChildren = true;
                }
            }

            if (so.hasModifiedProperties)
            {
                so.ApplyModifiedProperties();
            }
        }

        private static void DrawPositionHandle(Transform transform, SerializedProperty property, DrawHandleAttribute attr)
        {
            var handleRotation = Tools.pivotRotation == PivotRotation.Local ? transform.rotation : Quaternion.identity;
            
            Vector3 currentPos;
            if (attr.LocalSpace)
            {
                currentPos = transform.TransformPoint(property.vector3Value);
            }
            else
            {
                currentPos = property.vector3Value;
            }

            EditorGUI.BeginChangeCheck();
            var newPos = Handles.PositionHandle(currentPos, handleRotation);
            
            if (EditorGUI.EndChangeCheck())
            {
                if (attr.LocalSpace)
                {
                    property.vector3Value = transform.InverseTransformPoint(newPos);
                }
                else
                {
                    property.vector3Value = newPos;
                }
            }
            
            Handles.Label(currentPos, property.displayName);
        }

        private static void DrawRotationHandle(Transform transform, SerializedProperty property, DrawHandleAttribute attr)
        {
            var position = transform.position;
            
            Quaternion currentRot;
            if (attr.LocalSpace)
            {
                currentRot = transform.rotation * property.quaternionValue;
            }
            else
            {
                currentRot = property.quaternionValue;
            }

            EditorGUI.BeginChangeCheck();
            var newRot = Handles.RotationHandle(currentRot, position);

            if (EditorGUI.EndChangeCheck())
            {
                if (attr.LocalSpace)
                {
                    property.quaternionValue = Quaternion.Inverse(transform.rotation) * newRot;
                }
                else
                {
                    property.quaternionValue = newRot;
                }
            }
        }
    }
}