using System.Linq;
using System.Reflection;
using UnityEditor;
using YanickSenn.Utils.VContainer;
using YanickSenn.Utils.VContainer.Attributes;

namespace YanickSenn.Utils.Editor.VContainer {
    [CustomEditor(typeof(ProviderLifetimeScope), true)]
    public class ProviderLifetimeScopeEditor : UnityEditor.Editor {
        private bool _showVContainerSettings;
        private bool _showProvidedRegistrations = true;

        public override void OnInspectorGUI() {
            serializedObject.Update();

            // Draw Script field
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();

            // Draw other properties excluding standard VContainer ones
            DrawPropertiesExcluding(serializedObject,
                "m_Script", "parentReference", "autoRun", "autoInjectGameObjects");

            // Standard VContainer Settings Foldout
            _showVContainerSettings = EditorGUILayout.Foldout(_showVContainerSettings, "VContainer Settings", true);
            if (_showVContainerSettings) {
                EditorGUI.indentLevel++;
                DrawPropertyIfFound("parentReference");
                DrawPropertyIfFound("autoRun");
                DrawPropertyIfFound("autoInjectGameObjects");

                // Provided Registrations Foldout nested inside
                _showProvidedRegistrations =
                    EditorGUILayout.Foldout(_showProvidedRegistrations, "Provided Registrations", true);
                if (_showProvidedRegistrations) {
                    EditorGUI.indentLevel++;
                    DrawRegistrationsFor(target.GetType(), "Current Scope");
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPropertyIfFound(string propertyName) {
            var property = serializedObject.FindProperty(propertyName);
            if (property != null) {
                EditorGUILayout.PropertyField(property);
            }
        }

        private void DrawRegistrationsFor(System.Type type, string header) {
            // Get all methods with [Provides] attribute, including private/protected
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var matchingMethods = methods
                .Where(m => m.GetCustomAttribute<ProvidesAttribute>() != null)
                .ToList();

            if (matchingMethods.Count == 0) {
                // Optionally show "None" or just skip
                // let's skip headers for parents if empty, but show for Current if we want "No registrations"
                if (header == "Current Scope") {
                    EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.HelpBox("No [Provides] registrations found.", MessageType.Info);
                    EditorGUI.indentLevel--;
                }

                return;
            }

            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            foreach (var method in matchingMethods) {
                var providesAttr = method.GetCustomAttribute<ProvidesAttribute>();

                var returnType = method.ReturnType;
                var key = providesAttr.Key;
                var lifetimeAttr = method.GetCustomAttribute<RegisterLifetimeAttribute>();

                // Display Format: [Type] [Key if any] (Lifetime)
                var typeName = returnType.Name;
                if (returnType.IsGenericType) {
                    // Make generic types readable e.g., Func`1[Int] -> Func<Int>
                    typeName =
                        $"{returnType.Name.Split('`')[0]}<{string.Join(", ", returnType.GetGenericArguments().Select(t => t.Name))}>";
                }

                var keyStr = key != null ? $" [Key: {key}]" : "";
                var lifetimeStr =
                    lifetimeAttr != null ? $" ({lifetimeAttr.Lifetime})" : " (Singleton)"; // Default is singleton

                EditorGUILayout.LabelField($"{typeName}{keyStr}{lifetimeStr}");
            }

            EditorGUI.indentLevel--;
        }
    }
}
