using UnityEditor;
using YanickSenn.Utils.VContainer;

namespace YanickSenn.Utils.Editor.VContainer
{
    [CustomEditor(typeof(PlainLifetimeScope), true)]
    public class PlainLifetimeScopeEditor : UnityEditor.Editor
    {
        private bool _showVContainerSettings;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();
        
            DrawPropertiesExcluding(serializedObject, 
                "m_Script", "parentReference", "autoRun", "autoInjectGameObjects", "scriptableObjectInstallers");

            _showVContainerSettings = EditorGUILayout.Foldout(_showVContainerSettings, "Advanced VContainer Settings", true);
            if (_showVContainerSettings)
            {
                EditorGUI.indentLevel++;
                DrawPropertyIfFound("parentReference");
                DrawPropertyIfFound("autoRun");
                DrawPropertyIfFound("autoInjectGameObjects");
                DrawPropertyIfFound("scriptableObjectInstallers");
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPropertyIfFound(string propertyName)
        {
            var property = serializedObject.FindProperty(propertyName);
            if (property != null)
            {
                EditorGUILayout.PropertyField(property);
            }
        }
    }
}
