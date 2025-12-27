using System;

namespace YanickSenn.Utils.Variables {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false)]
    public class GenerateVariableAttribute : Attribute {
        public bool Generate { get; set; } = true;
    }
}
