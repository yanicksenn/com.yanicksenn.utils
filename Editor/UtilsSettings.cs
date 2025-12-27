using System;
using UnityEditor;
using UnityEngine;

namespace YanickSenn.Utils.Editor {

    [Serializable]
    public struct RegistryGenerationSettings {
        [SerializeField]
        public bool disabled;
    }

    [Serializable]
    public struct VariableGenerationSettings {
        [SerializeField]
        public bool disabled;
    }
    
    [FilePath("ProjectSettings/YanickSenn.Utils.Settings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class UtilsSettings : ScriptableSingleton<UtilsSettings> {
        
        [SerializeField]
        private RegistryGenerationSettings registryGenerationSettings;
        public RegistryGenerationSettings RegistryGenerationSettings {
            get => registryGenerationSettings;
            set {
                registryGenerationSettings = value;
                Save(true);
            }
        }

        [SerializeField]
        private VariableGenerationSettings variableGenerationSettings;
        public VariableGenerationSettings VariableGenerationSettings {
            get => variableGenerationSettings;
            set {
                variableGenerationSettings = value;
                Save(true);
            }
        }
    }

    internal static class InjectionSettingsProvider {
        [SettingsProvider]
        public static SettingsProvider CreateUtilsSettingsProvider() {
            var provider = new SettingsProvider("Project/YanickSenn/Utils", SettingsScope.Project) {
                label = "Utils",
                guiHandler = (searchContext) => {
                    var settings = UtilsSettings.instance;
                    
                    EditorGUILayout.LabelField("Registry Generation", EditorStyles.boldLabel);   
                    
                    EditorGUI.BeginChangeCheck();
                    var disableRegistry = EditorGUILayout.Toggle(new GUIContent(
                            "Disable Registry Generation",
                            "Globally disable the automatic generation of injection registries."), 
                        settings.RegistryGenerationSettings.disabled);
                    if (EditorGUI.EndChangeCheck()) {
                        var registrySettings = settings.RegistryGenerationSettings;
                        registrySettings.disabled = disableRegistry;
                        settings.RegistryGenerationSettings = registrySettings;
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Variable Generation", EditorStyles.boldLabel);

                    EditorGUI.BeginChangeCheck();
                    var disableVariable = EditorGUILayout.Toggle(new GUIContent(
                            "Disable Variable Generation",
                            "Globally disable the automatic generation of variables and references."), 
                        settings.VariableGenerationSettings.disabled);
                    if (EditorGUI.EndChangeCheck()) {
                        var variableSettings = settings.VariableGenerationSettings;
                        variableSettings.disabled = disableVariable;
                        settings.VariableGenerationSettings = variableSettings;
                    }
                },
                keywords = new System.Collections.Generic.HashSet<string>(new[] { "Yanick", "Senn", "Utils", "Injection", "Registry", "Generation", "Variable" })
            };

            return provider;
        }
    }
}
