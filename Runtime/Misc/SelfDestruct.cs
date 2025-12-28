using System.Collections;
using UnityEngine;
using YanickSenn.Utils.Extensions;

namespace YanickSenn.Utils.Misc
{
    [DisallowMultipleComponent]
    public class SelfDestruct : MonoBehaviour {
        [SerializeField] private float autoDestroyAfter = 3f;

        private void Awake() {
            StartCoroutine(DestroyAfter(3f));
        }

        private IEnumerator DestroyAfter(float time) {
            yield return new WaitForSeconds(time);
            gameObject.Destroy();
        }
    }
}