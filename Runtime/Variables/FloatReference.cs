using System;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class FloatReference : Reference<float, FloatVariable> { 
        public FloatReference(float value = default) : base(value) { }
    }
}
