using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hellhole.FastContainer
{
    public class Registration
    {
        internal Type ServiceType { get; private set; }
        internal object Instance { get; private set; }
        internal Type ImplementationType { get; private set; }
        internal LifetimeScope LifetimeScope { get; private set; }

        public static Registration ForInstance(object instance)
        {
            return new Registration
            {
                ServiceType = instance.GetType(),
                ImplementationType = instance.GetType(),
                LifetimeScope = LifetimeScope.Singleton,
                Instance = instance
            };
        }

        public static Registration For(Type implementationType, LifetimeScope lifetimeScope)
        {
            return new Registration
            {
                ServiceType = implementationType,
                LifetimeScope = lifetimeScope,
                ImplementationType = implementationType
            };
        }

        public static Registration For<TImplementation>(LifetimeScope lifetimeScope)
        where TImplementation : class
        {
            return For(typeof(TImplementation), lifetimeScope);
        }

        public Registration As<TService>()
        {
            return As(typeof(TService));
        }

        public Registration As(Type serviceType)
        {
            if (!serviceType.IsAssignableFrom(ImplementationType))
                throw new Exception($"ImplementationType {ImplementationType.FullName} is not assignable to ServiceType {serviceType.FullName}.");

            ServiceType = serviceType;
            return this;
        }

        internal void EnsureResolver(Dictionary<Type, Registration> registrations, HashSet<Type> dependencyChain = null)
        {
            if (Resolver != null)
                return;
            if (dependencyChain == null)
                dependencyChain = new HashSet<Type>();

            if (dependencyChain.Contains(ServiceType))
            {
                var list = string.Join(" > ", dependencyChain.SkipWhile(t => t != ServiceType).Select(t => t.Name));
                throw new Exception($"Circular dependency detected: {list} > {ServiceType.Name}");
            }

            dependencyChain.Add(ServiceType);

            Resolver = CreateResolver(registrations, dependencyChain);
        }

        private Func<object> CreateResolver(Dictionary<Type, Registration> registrations, HashSet<Type> dependencyChain)
        {
            if (Instance != null)
                return () => Instance;

            var constructorFunc = GetConstructorFunc(registrations, dependencyChain);

            if (LifetimeScope == LifetimeScope.Singleton)
            {
                var instance = constructorFunc.Invoke();
                return () => instance;
            }

            return constructorFunc;
        }

        private Func<object> GetConstructorFunc(Dictionary<Type, Registration> registrations, HashSet<Type> dependencyChain)
        {
            var constructor = GetSingleConstructorOrThrow();
            var parameters = constructor.GetParameters();
            var argumentResolvers = BuildConstructorArgumentResolvers(registrations, parameters, dependencyChain);
            var arguments = new object[parameters.Length];
            return () =>
            {
                for (var i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = argumentResolvers[i].Invoke();
                }

                return constructor.Invoke(arguments);
            };
        }

        private Func<object>[] BuildConstructorArgumentResolvers(Dictionary<Type, Registration> registrations, ParameterInfo[] parameters, HashSet<Type> dependencyChain)
        {
            var argumentResolvers = new Func<object>[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;

                ThrowIfDependencyUnresolvable(parameterType, registrations);
                var registration = registrations[parameterType];
                registration.EnsureResolver(registrations, new HashSet<Type>(dependencyChain));
                argumentResolvers[i] = registration.Resolver;
            }

            return argumentResolvers;
        }

        private ConstructorInfo GetSingleConstructorOrThrow()
        {
            var constructors = ImplementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            if (constructors.Length != 1)
                throw new Exception(
                    $"A service implementation must have exactly 1 public constructor to be able to register it. {ImplementationType.Name} has {constructors.Length} public constructors.");

            return constructors[0];
        }

        private void ThrowIfDependencyUnresolvable(Type parameterType, Dictionary<Type, Registration> registrations)
        {
            if (!registrations.ContainsKey(parameterType))
            {
                var dependency = $"\"{GetTypeName(parameterType)}\"";
                var dependent = ImplementationType == ServiceType
                    ? $"\"{GetTypeName(ImplementationType)}\""
                    : $"\"{GetTypeName(ImplementationType)}\" (as service \"{GetTypeName(ServiceType)}\")";

                throw new ContainerException($"No registration found for injecting {dependency} into {dependent}.");
            }
        }

        private string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                var genericArguments = string.Join(", ", type.GetGenericArguments().Select(t => t.Name));
                return $"{type.Name}<{genericArguments}>";
            }

            return type.Name;
        }

        public Func<object> Resolver { get; private set; }
    }
}
