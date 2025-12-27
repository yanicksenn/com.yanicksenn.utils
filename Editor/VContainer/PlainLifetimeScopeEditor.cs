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
                EditorGUILayout.PropertyField(serializedObject.FindProperty("parentReference"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("autoRun"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("autoInjectGameObjects"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("scriptableObjectInstallers"));
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
