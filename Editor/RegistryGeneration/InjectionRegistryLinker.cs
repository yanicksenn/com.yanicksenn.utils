using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using YanickSenn.Utils.RegistryGeneration;
using YanickSenn.Utils.VContainer;

namespace YanickSenn.Utils.Editor.RegistryGeneration {
    public static class InjectionRegistryLinker {

        [DidReloadScripts]
        private static void OnScriptsReloaded() {
            LinkRegistries();
        }

        private static void LinkRegistries() {
            var targetTypes = TypeCache.GetTypesWithAttribute<GenerateInjectionRegistryAttribute>();
            var outputDir = "Assets/Generated/Registries";

            if (!System.IO.Directory.Exists(outputDir)) {
                return;
            }

            foreach (var type in targetTypes) {
                var attr = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<GenerateInjectionRegistryAttribute>(type);
                if (attr != null && attr.Generate) {
                    LinkRegistryForType(type, outputDir);
                }
            }
        }

        private static void LinkRegistryForType(Type type, string outputDir) {
            var className = $"{type.Name}Registry";
            var assetPath = $"{outputDir}/{className}.asset";

            // 1. Find or Create the Registry Asset
            var registry = AssetDatabase.LoadAssetAtPath<ScriptableObjectInstaller>(assetPath);
            if (registry == null) {
                // We need to find the Type of the generated class.
                // Since it's in the global namespace (as per generator), we look for it by name.
                var registryType = GetTypeByName(className);
                if (registryType == null) {
                    // Script might not be compiled yet or deleted.
                    return;
                }

                registry = ScriptableObject.CreateInstance(registryType) as ScriptableObjectInstaller;
                if (registry != null) {
                    AssetDatabase.CreateAsset(registry, assetPath);
                    Debug.Log($"Created registry asset: {assetPath}");
                }
            }

            if (registry == null) {
                return;
            }

            // 2. Update References
            var serializedObj = new SerializedObject(registry);
            var assets = InjectionUtils.FindAssetsByType(type);
            var changed = false;

            foreach (var asset in assets) {
                var fieldName = InjectionUtils.SanitizeName(asset.name);
                var prop = serializedObj.FindProperty(fieldName);
                if (prop == null || prop.objectReferenceValue == asset) {
                    continue;
                }
                prop.objectReferenceValue = asset;
                changed = true;
            }

            if (!changed) {
                return;
            }
            serializedObj.ApplyModifiedProperties();
            EditorUtility.SetDirty(registry);
            AssetDatabase.SaveAssets();
            Debug.Log($"Updated registry references: {assetPath}");
        }

        private static Type GetTypeByName(string name) {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                var type = assembly.GetType(name);
                if (type != null) return type;
            }
            return null;
        }
    }
}
