using UnityEditor;

namespace YanickSenn.Utils.Editor
{
    [CustomEditor(typeof(PlainLifetimeScope), true)]
    public class PlainLifetimeScopeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();
        
            DrawPropertiesExcluding(serializedObject, "m_Script", "parentReference", "autoRun", "autoInjectGameObjects");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
