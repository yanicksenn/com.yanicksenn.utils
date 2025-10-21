using System;
using UnityEngine;

namespace YanickSenn.Utils {

    [DisallowMultipleComponent, ExecuteAlways]
    public class TransformFollower : MonoBehaviour {
        public Transform transformToFollow;
        public Vector3 offset;
        public bool followY;

        private void LateUpdate() {
            if (transformToFollow == null) {
                return;
            }

            var newPos = transformToFollow.position + offset;
            transform.position = new Vector3(
                newPos.x, 
                followY ? newPos.y : transform.position.y,
                newPos.z
            );
        }
    }
}