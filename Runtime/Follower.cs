using System;
using UnityEngine;

namespace Util
{
    [DisallowMultipleComponent]
    public class Follower : MonoBehaviour
    {
        [SerializeField]
        public Anchor anchor;

        private void LateUpdate() {
            transform.position = anchor.transform.position;
            transform.rotation = anchor.transform.rotation;
        }
    }
}