using System;
using VContainer;

namespace YanickSenn.Utils.VContainer.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public class RegisterLifetimeAttribute : Attribute {
        public Lifetime Lifetime { get; }

        public RegisterLifetimeAttribute(Lifetime lifetime) {
            Lifetime = lifetime;
        }
    }
}
