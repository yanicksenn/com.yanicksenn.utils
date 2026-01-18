using System;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class IntReference : Reference<int> {
        public IntReference(int value = default) : base(value) { }
    }
}
