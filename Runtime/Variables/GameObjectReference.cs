using System;
using UnityEngine;
using YanickSenn.Utils;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class GameObjectReference : Reference<GameObject> {
        public GameObjectReference(GameObject value = default) : base(value) { }
    }
}
