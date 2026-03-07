using System;
using UnityEngine.Splines;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class SplineReference : Reference<Spline> {
        public SplineReference(Spline value = default) : base(value) { }
    }
}
