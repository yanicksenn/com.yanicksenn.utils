using UnityEngine;

namespace YanickSenn.Utils.Extensions {
    public static class GameObjectExtensions {
        public static void Destroy(this GameObject gameObject) {
            Object.Destroy(gameObject);
        }
    }
}
