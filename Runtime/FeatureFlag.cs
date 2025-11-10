using UnityEngine;

namespace YanickSenn.Utils
{
    [CreateAssetMenu(fileName = "FeatureFlag", menuName = "Feature Flag")]
    public class FeatureFlag : ScriptableObject {
        [SerializeField, TextArea]
        private string description;

        [SerializeField]
        private bool enabled;
        
        public bool IsEnabled => enabled;
    }
}