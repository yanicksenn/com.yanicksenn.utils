using System;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class BoolReference : Reference<bool, BoolVariable> {
        public BoolReference(bool value = default) : base(value) { }
    }
}
