using System;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class IntReference : Reference<int, IntVariable> {
        public IntReference(int value = default) : base(value) { }
    }
}