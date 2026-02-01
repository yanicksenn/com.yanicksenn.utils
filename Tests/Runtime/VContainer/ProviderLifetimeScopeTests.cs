using System;
using NUnit.Framework;
using UnityEngine;
using VContainer;
using YanickSenn.Utils.VContainer;
using YanickSenn.Utils.VContainer.Attributes;
using YanickSenn.Utils.VContainer.Enums;

namespace YanickSenn.Utils.Tests.VContainer {
    public class ProviderLifetimeScopeTests {

        public class ServiceA {}
        public class ServiceB {
            public ServiceA A { get; }
            public ServiceB(ServiceA a) { A = a; }
        }
        public class ServiceInstance {}
        public class ServiceComponent : MonoBehaviour {}
        public class ServiceFactoryProduct {
            public int Value { get; }
            public ServiceFactoryProduct(int v) { Value = v; }
        }

        public class TestScope : ProviderLifetimeScope {
            [Provides]
            [RegisterLifetime(Lifetime.Singleton)]
            public ServiceA ProvideA() => new ServiceA();

            [Provides]
            public ServiceB ProvideB(ServiceA a) => new ServiceB(a);

            [Provides(RegistrationType.Instance, "InstanceKey")]
            public ServiceInstance ProvideInstance() => new ServiceInstance();

            [Provides(RegistrationType.Component)]
            public ServiceComponent ProvideComponent() {
                var go = new GameObject("ServiceComponent");
                // In a real scenario, this might be a prefab instantiation or finding an object.
                // For test cleanup, we should track this?
                // VContainer destroys components registered as Component when scope is disposed?
                // No, RegisterComponent just injects into it. It doesn't necessarily manage its lifetime unless it's a child scope prefab.
                // But let's just return a component.
                return go.AddComponent<ServiceComponent>();
            }

                        [Provides(RegistrationType.Factory)]
                        public ServiceFactoryProduct ProvideFactory(ServiceA a) => new ServiceFactoryProduct(10);
            
                        [Provides(RegistrationType.Factory)]
                        public string ProvideStringFactory(IObjectResolver resolver) {
                            Assert.IsNotNull(resolver);
                            return "ResolvedString";
                        }
                    }
            
                    [Test]
                    public void Verify_Registrations() {
                        var go = new GameObject("TestScope");
                        var scope = go.AddComponent<TestScope>();
                        
                        // Build the scope.
                        scope.Build();
                        var container = scope.Container;
                        Assert.IsNotNull(container, "Container should be built");
            
                        // 1. Standard Registration & Dependency Injection
                        var serviceB = container.Resolve<ServiceB>();
                        Assert.IsNotNull(serviceB);
                        Assert.IsNotNull(serviceB.A);
                        
                        var serviceA = container.Resolve<ServiceA>();
                        Assert.AreSame(serviceB.A, serviceA); // Singleton check
            
                        // 2. Keyed Registration
                        var keyedInstance = container.Resolve<ServiceInstance>("InstanceKey");
                        Assert.IsNotNull(keyedInstance);
            
                        // 3. Component Registration
                        var component = container.Resolve<ServiceComponent>();
                        Assert.IsNotNull(component);
                        
                        // 4. Factory Registration
                        var factory = container.Resolve<Func<ServiceFactoryProduct>>();
                        Assert.IsNotNull(factory);
                        var product = factory();
                        Assert.IsNotNull(product);
                        Assert.AreEqual(10, product.Value);
            
                        // 5. Factory with IObjectResolver
                        var stringFactory = container.Resolve<Func<string>>();
                        Assert.IsNotNull(stringFactory);
                        Assert.AreEqual("ResolvedString", stringFactory());
                        
                        UnityEngine.Object.DestroyImmediate(go);
                    }    }
}
