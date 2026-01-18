using System;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class BoolReference : Reference<bool> {
        public BoolReference(bool value = default) : base(value) { }
    }
}
