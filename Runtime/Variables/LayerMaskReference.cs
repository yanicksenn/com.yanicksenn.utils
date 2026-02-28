using System;
using UnityEngine;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class LayerMaskReference : Reference<LayerMask> {
        public LayerMaskReference(LayerMask value = default) : base(value) { }
    }
}