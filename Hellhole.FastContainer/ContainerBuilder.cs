using System;
using System.Collections.Generic;
using System.Linq;

namespace Hellhole.FastContainer
{
    public class ContainerBuilder
    {
        private readonly List<Registration> _registrations;

        public ContainerBuilder()
        {
            _registrations = new List<Registration>();
        }

        /// <summary>
        /// Register a singleton as a service.
        /// </summary>
        public Registration RegisterInstance(object instance)
        {
            var registration = Registration.ForInstance(instance);
            Register(registration);
            return registration;
        }

        public Registration Register<TImplementation>(LifetimeScope lifetimeScope)
            where TImplementation : class
        {
            var registration = Registration.For<TImplementation>(lifetimeScope);
            Register(registration);
            return registration;
        }

        public Registration Register(Type implementationType, LifetimeScope lifetimeScope)
        {
            var registration = Registration.For(implementationType, lifetimeScope);
            Register(registration);
            return registration;
        }

        public void Register(Registration registration)
        {
            _registrations.Add(registration);
        }

        public Container Build()
        {
            var resolvers = BuildResolvers();
            return new Container(resolvers);
        }

        private Dictionary<Type, Func<object>> BuildResolvers()
        {
            var resolvers = new Dictionary<Type, Func<object>>();
            var registrationsByServiceType = _registrations.ToDictionary(r => r.ServiceType);
            foreach (var registration in _registrations)
            {
                registration.EnsureResolver(registrationsByServiceType);
                resolvers.Add(registration.ServiceType, registration.Resolver);
            }

            return resolvers;
        }
    }
}
