using System;
using System.Collections.Generic;

namespace Hellhole.FastContainer
{
    /// <summary>
    /// IoC container but resolution is precomputed upon Build() for performance, so no dynamic features. Use <see cref="ContainerBuilder"/> to build.
    /// </summary>
    /// <remarks>
    /// No Func<x> factory injection because it requires a cast to type x through a reflection call to a generic Cast method..
    /// </remarks>
    public class Container
        : IDisposable
    {
        private readonly IDictionary<Type, Func<object>> _resolvers;
        private readonly HashSet<IDisposable> _disposables;

        internal Container(IDictionary<Type, Func<object>> resolvers)
        {
            _resolvers = resolvers;
            _disposables = new HashSet<IDisposable>();

        }

        public TService Resolve<TService>()
        {
            if (!_resolvers.ContainsKey(typeof(TService)))
                throw new ContainerException($"No service with type {typeof(TService).FullName} registered in the container.");
            var service = (TService)_resolvers[typeof(TService)].Invoke();
            if (service is IDisposable disposable && !_disposables.Contains(disposable))
                _disposables.Add(disposable);
            return service;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}
