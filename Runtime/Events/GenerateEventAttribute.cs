using System;

namespace YanickSenn.Utils.Events {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false)]
    public class GenerateEventAttribute : Attribute {
        public bool Generate { get; set; } = true;
    }
}
