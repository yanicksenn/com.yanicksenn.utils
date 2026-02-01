using System;
using System.Reflection;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using YanickSenn.Utils.VContainer.Attributes;
using YanickSenn.Utils.VContainer.Enums;

namespace YanickSenn.Utils.VContainer {
    public class ProviderLifetimeScope : LifetimeScope {
        protected sealed override void Configure(IContainerBuilder builder) {
            ConfigureProvider(builder);
        }

        protected virtual void ConfigureProvider(IContainerBuilder builder) {
            var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods) {
                var providesAttr = method.GetCustomAttribute<ProvidesAttribute>();
                if (providesAttr == null) {
                    continue;
                }

                var lifetimeAttr = method.GetCustomAttribute<RegisterLifetimeAttribute>();
                var lifetime = lifetimeAttr?.Lifetime ?? Lifetime.Singleton;

                var key = providesAttr.Key;

                var returnType = method.ReturnType;

                switch (providesAttr.Type) {
                    case RegistrationType.Standard:
                        InvokeGenericHelper(nameof(RegisterStandardHelper), returnType, new object[] { builder, method, lifetime, key });
                        break;
                    case RegistrationType.Instance:
                        InvokeGenericHelper(nameof(RegisterInstanceHelper), returnType, new object[] { builder, method, key });
                        break;
                    case RegistrationType.Component:
                        InvokeGenericHelper(nameof(RegisterComponentHelper), returnType, new object[] { builder, method, key });
                        break;
                    case RegistrationType.Factory:
                        InvokeGenericHelper(nameof(RegisterFactoryHelper), returnType, new object[] { builder, method, lifetime, key });
                        break;
                }
            }
        }

        private void InvokeGenericHelper(string helperName, Type genericType, object[] args) {
            var helper = typeof(ProviderLifetimeScope).GetMethod(helperName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (helper == null) {
                Debug.LogError($"[ProviderLifetimeScope] Helper method '{helperName}' not found.");
                return;
            }
            var genericHelper = helper.MakeGenericMethod(genericType);
            genericHelper.Invoke(this, args);
        }

        private void RegisterStandardHelper<T>(IContainerBuilder builder, MethodInfo method, Lifetime lifetime, object key) {
            var registration = builder.Register(resolver => (T)InvokeWithResolvedArgs(method, resolver), lifetime);
            if (key != null) {
                registration.Keyed(key);
            }
        }

        private void RegisterInstanceHelper<T>(IContainerBuilder builder, MethodInfo method, object key) {
            if (method.GetParameters().Length > 0) {
                Debug.LogError($"[ProviderLifetimeScope] Method '{method.Name}' marked as Instance cannot have parameters.");
                return;
            }

            var instance = method.Invoke(this, null);
            if (instance != null) {
                var registration = builder.RegisterInstance<T>((T)instance);
                if (key != null) {
                    registration.Keyed(key);
                }
            }
        }

        private void RegisterComponentHelper<T>(IContainerBuilder builder, MethodInfo method, object key) {
            if (method.GetParameters().Length > 0) {
                Debug.LogError($"[ProviderLifetimeScope] Method '{method.Name}' marked as Component cannot have parameters.");
                return;
            }

            var instance = method.Invoke(this, null);
            if (instance is Component) {
                // We cast to T. T might be an interface or the component type itself.
                var registration = builder.RegisterComponent<T>((T)instance);
                if (key != null) {
                    registration.Keyed(key);
                }
            } else {
                Debug.LogError($"[ProviderLifetimeScope] Method '{method.Name}' marked as Component did not return a Component.");
            }
        }

        private void RegisterFactoryHelper<T>(IContainerBuilder builder, MethodInfo method, Lifetime lifetime, object key) {
            var registration = builder.RegisterFactory<T>(resolver => {
                return () => (T)InvokeWithResolvedArgs(method, resolver);
            }, lifetime);
            
            if (key != null) {
                registration.Keyed(key);
            }
        }

        private object InvokeWithResolvedArgs(MethodInfo method, IObjectResolver resolver) {
            var parameters = method.GetParameters();
            var args = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++) {
                if (parameters[i].ParameterType == typeof(IObjectResolver)) {
                    args[i] = resolver;
                } else {
                    args[i] = resolver.Resolve(parameters[i].ParameterType);
                }
            }
            return method.Invoke(this, args);
        }
    }
}