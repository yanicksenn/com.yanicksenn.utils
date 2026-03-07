using UnityEngine;
using UnityEngine.Splines;

namespace YanickSenn.Utils.Variables
{
    [CreateAssetMenu(fileName = "SplineVariable", menuName = "Variables/Spline Variable", order = 10)]
    public class SplineVariable : Variable<Spline> { }
}
