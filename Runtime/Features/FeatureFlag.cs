using UnityEngine;
using YanickSenn.Utils.RegistryGeneration;

namespace YanickSenn.Utils.Features
{
    [CreateAssetMenu(fileName = "FeatureFlag", menuName = "Feature Flag")]
    [GenerateInjectionRegistry]
    public class FeatureFlag : ScriptableObject {
        [SerializeField, TextArea]
        private string description;

        [SerializeField]
        private bool enabled;
        
        public bool IsEnabled => enabled;
    }
}