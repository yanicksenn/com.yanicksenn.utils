using System.Collections.Generic;
using UnityEngine;
using YanickSenn.Utils.RegistryGeneration;

namespace YanickSenn.Utils.RuntimeSets
{
    [CreateAssetMenu(menuName = "Runtime Set", fileName = "RuntimeSet")]
    [GenerateInjectionRegistry]
    public class RuntimeSet : ScriptableObject {
        private readonly HashSet<GameObject> _elements = new();
        
        public void Add(GameObject element) => _elements.Add(element);
        public void Remove(GameObject element) => _elements.Remove(element);
        public IEnumerable<GameObject> Elements() => _elements;
    }
}