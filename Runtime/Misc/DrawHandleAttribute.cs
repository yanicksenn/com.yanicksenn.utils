using System;
using UnityEngine;

namespace YanickSenn.Utils.Misc
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DrawHandleAttribute : PropertyAttribute
    {
        public bool LocalSpace { get; }

        public DrawHandleAttribute(bool localSpace = true)
        {
            LocalSpace = localSpace;
        }
    }
}
