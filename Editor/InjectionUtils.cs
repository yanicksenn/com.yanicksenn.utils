using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace YanickSenn.Utils.Editor {
    public static class InjectionUtils {
        
        public static string SanitizeName(string name) {
            if (string.IsNullOrEmpty(name)) return name;

            var sb = new System.Text.StringBuilder();
            var capitalizeNext = true;
            foreach (var c in name) {
                if (char.IsLetterOrDigit(c)) {
                    sb.Append(capitalizeNext ? char.ToUpper(c) : c);
                    capitalizeNext = false;
                } else {
                    capitalizeNext = true;
                }
            }
            return sb.ToString();
        }

        public static List<ScriptableObject> FindAssetsByType(Type type) {
            // Using t:TypeFullName handles derived types too, which is usually desired.
            var guids = AssetDatabase.FindAssets($"t:{type.FullName}");
            var assets = new List<ScriptableObject>();
            foreach (var guid in guids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(path, type) as ScriptableObject;
                if (asset != null) {
                    assets.Add(asset);
                }
            }
            // Deterministic order is crucial for code gen stability
            return assets.OrderBy(a => a.name).ToList();
        }
    }
}
