using System;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class UintReference : Reference<uint> {
        public UintReference(uint value = default) : base(value) { }
    }
}
