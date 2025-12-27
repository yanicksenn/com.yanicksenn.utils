using System;
using UnityEditor;
using UnityEngine;

namespace YanickSenn.Utils.Editor {

    [Serializable]
    public struct RegistryGenerationSettings {
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
                    var disable = EditorGUILayout.Toggle(new GUIContent(
                            "Disable Registry Generation",
                            "Globally disable the automatic generation of injection registries."), 
                        settings.RegistryGenerationSettings.disabled);
                    if (EditorGUI.EndChangeCheck()) {
                        var registrySettings = settings.RegistryGenerationSettings;
                        registrySettings.disabled = disable;
                        settings.RegistryGenerationSettings = registrySettings;
                    }
                },
                keywords = new System.Collections.Generic.HashSet<string>(new[] { "Yanick", "Senn", "Utils", "Injection", "Registry", "Generation" })
            };

            return provider;
        }
    }
}
