using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YanickSenn.Utils
{
    [Serializable]
    public struct Prefab<T> where T : Component
    {
        [SerializeField]
        private T _asset;

        public T Asset => _asset;

        public Prefab(T asset) {
            _asset = asset;
        }

        public T Instantiate(Transform parent = null)
        {
            return Object.Instantiate(_asset, parent);
        }

        public T Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return Object.Instantiate(_asset, position, rotation, parent);
        }
    }
}
