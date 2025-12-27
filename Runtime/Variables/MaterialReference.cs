using System;
using UnityEngine;
using YanickSenn.Utils;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class MaterialReference : Reference<Material, MaterialVariable> {
        public MaterialReference(Material value = default) : base(value) { }
    }
}
