using UnityEngine;
using YanickSenn.Utils.Control;

namespace YanickSenn.Utils.Extensions {
    public static class ComponentExtensions {
        public static void Destroy(this Component component) {
            Object.Destroy(component);
        }

        public static void DestroyGameObject(this Component component) {
            Object.Destroy(component.gameObject);
        }

        public static Optional<T> ToOptional<T>(this T component) where T : Component {
            return new Optional<T>(component);
        }
    }
}
