using System;
using UnityEngine;
using YanickSenn.Utils;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class MaterialReference : Reference<Material> {
        public MaterialReference(Material value = default) : base(value) { }
    }
}
