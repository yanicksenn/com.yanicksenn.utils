using System;

namespace YanickSenn.Utils {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, Inherited = false)]
    public class GeneratedAttribute : Attribute { }
}
