using System;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class FloatReference : Reference<float> {
        public FloatReference(float value = default) : base(value) { }
    }
}
