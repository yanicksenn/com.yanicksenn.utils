using System;
using UnityEngine;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class AnimationCurveReference : Reference<AnimationCurve> {
        public AnimationCurveReference(AnimationCurve value = default) : base(value) { }
    }
}