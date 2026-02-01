using System;
using YanickSenn.Utils.VContainer.Enums;

namespace YanickSenn.Utils.VContainer.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public class ProvidesAttribute : Attribute {
        public RegistrationType Type { get; }
        public object Key { get; }

        public ProvidesAttribute(RegistrationType type, object key = null) {
            Type = type;
            Key = key;
        }

        public ProvidesAttribute(object key = null) : this(RegistrationType.Standard, key) {
        }
    }
}