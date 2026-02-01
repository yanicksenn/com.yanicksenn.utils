using System.Collections;
using UnityEngine;
using YanickSenn.Utils.Extensions;

namespace YanickSenn.Utils.Misc
{
    [DisallowMultipleComponent]
    public class SelfDestruct : MonoBehaviour {
        [SerializeField] private float autoDestroyAfter = 3f;

        private void Awake() {
            StartCoroutine(DestroyAfter(autoDestroyAfter));
        }

        private IEnumerator DestroyAfter(float time) {
            yield return new WaitForSeconds(time);
            gameObject.Destroy();
        }
    }
}