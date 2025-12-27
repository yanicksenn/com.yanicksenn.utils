using System;
using UnityEngine;

namespace YanickSenn.Utils.Control
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ClassTypeConstraintAttribute : PropertyAttribute
    {
        public Type BaseType { get; }
        public bool AllowAbstract { get; }

        public ClassTypeConstraintAttribute(Type baseType, bool allowAbstract = false)
        {
            BaseType = baseType;
            AllowAbstract = allowAbstract;
        }
    }
}
