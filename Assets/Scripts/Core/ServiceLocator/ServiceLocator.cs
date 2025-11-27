using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Game.Core.ServiceLocator
{
    //Implementation of ServiceLocator
    public static class ServiceLocator
    {
        private static readonly ConcurrentDictionary<Type, object> _services = new();

        public static void ClearAll() => _services.Clear();

        public static void Register<TService>(TService instance) where TService : class
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            _services[typeof(TService)] = instance;
        }

        public static bool TryRegister<TService>(TService instance) where TService : class
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return _services.TryAdd(typeof(TService), instance);
        }

        public static void RegisterFactory<TService>(Func<TService> factory) where TService : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _services[typeof(TService)] = new Lazy<TService>(() => factory());
        }

        public static TService Get<TService>() where TService : class
        {
            if (_services.TryGetValue(typeof(TService), out var obj))
            {
                if (obj is Lazy<TService> lazy) return lazy.Value;
                return obj as TService;
            }
            throw new InvalidOperationException($"Service not registered: {typeof(TService).FullName}");
        }

        public static bool TryGet<TService>(out TService service) where TService : class
        {
            service = null;
            if (_services.TryGetValue(typeof(TService), out var obj))
            {
                if (obj is Lazy<TService> lazy) { service = lazy.Value; return true; }
                service = obj as TService;
                return service != null;
            }
            return false;
        }

        public static bool Unregister<TService>() where TService : class
        {
            return _services.TryRemove(typeof(TService), out _);
        }

        public static IEnumerable<Type> RegisteredTypes() => _services.Keys;
    }
}
