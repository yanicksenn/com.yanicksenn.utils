using System;

namespace YanickSenn.Utils.RegistryGeneration {
 
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GenerateInjectionRegistryAttribute : Attribute {
        public bool Generate { get; set; } = true;
    }
}