// ─────────────────────────────────────────────────────────────────────────────
//  ServiceLocator.cs  (Unity-friendly, thread-safe, supports multi-binding)
// ─────────────────────────────────────────────────────────────────────────────
#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;   // Remove/replace if you’re not on Unity

namespace Core
{
    public enum Lifetime { Singleton, Transient }

    public static class ServiceLocator
    {
        // One Type → many registrations
        private static readonly ConcurrentDictionary<Type, List<Registration>> _map
            = new();
        
        private static bool _logDebug = false;
        

        // ─────────────────────────────── Register ──────────────────────────────
        public static void Register<T>(Func<T> factory,
                                       Lifetime lifetime = Lifetime.Singleton)
            where T : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var type = typeof(T);

            var reg = new Registration(
                lifetime,
                () => factory() ?? throw new InvalidOperationException($"Factory for {type.Name} returned null"),
                            lifetime == Lifetime.Singleton ? new Lazy<object>(() => factory(), LazyThreadSafetyMode.ExecutionAndPublication) : null);

            // Append to the list (or create a new one atomically)
            _map.AddOrUpdate(
                type,
                _ => new List<Registration> { reg },
                (_, list) => {
                    lock (list) 
                        list.Add(reg); 
                    return list;
                });

            if (_logDebug) Debug.Log($"[ServiceLocator] Registered {type.Name} ({lifetime}).");
        }

        // Convenience overload for already-created singletons
        public static void Register<T>(T instance) where T : class
            => Register(() => instance, Lifetime.Singleton);

        // ─────────────────────────────── Resolve ───────────────────────────────
        public static T Get<T>() where T : class
            => GetAll<T>().FirstOrDefault()
               ?? throw new KeyNotFoundException(
                   $"No service of type {typeof(T).Name} is registered.");

        public static bool TryGet<T>(out T? service) where T : class
        {
            service = GetAll<T>().FirstOrDefault();
            return service != null;
        }
        
        public static T GetOrThrow<T>() where T : class
        {
            var service = GetAll<T>().FirstOrDefault();
            if (service == null)
                throw new KeyNotFoundException($"No service of type {typeof(T).Name} is registered.");
            return service;
        }

        public static IReadOnlyList<T> GetAll<T>() where T : class
        {
            var target = typeof(T);
            var results = new List<T>();

            foreach (var entry in _map)
            {
                foreach (var reg in entry.Value)
                {
                    object instance = reg.Lifetime == Lifetime.Singleton
                                        ? reg.Singleton!.Value
                                        : reg.Factory();

                    if (target.IsAssignableFrom(instance.GetType()))
                        results.Add((T)instance);
                }
            }

            return results;
        }

        // ─────────────────────────────── Unregister ────────────────────────────
        /// <summary>Removes **all** registrations made for/under <typeparamref name="T"/>.</summary>
        public static void Unregister<T>() where T : class
        {
            if (_map.TryRemove(typeof(T), out var list))
            {
                foreach (var reg in list)
                    DisposeIfNeeded(reg);
                if (_logDebug) Debug.Log($"[ServiceLocator] Unregistered all of {typeof(T).Name}.");
            }
        }

        /// <summary>Remove a specific instance (helpful when you registered many).</summary>
        public static bool UnregisterInstance<T>(T instance) where T : class
        {
            if (instance == null) return false;
            if (!_map.TryGetValue(typeof(T), out var list)) return false;

            lock (list)
            {
                var idx = list.FindIndex(r =>
                    r.Lifetime == Lifetime.Singleton && ReferenceEquals(r.Singleton!.Value, instance)); // only singletons are reference-stable

                if (idx < 0) return false;

                DisposeIfNeeded(list[idx]);
                list.RemoveAt(idx);
                return true;
            }
        }

        // ─────────────────────────────── House-keeping ─────────────────────────
        public static void Clear()
        {
            foreach (var list in _map.Values.SelectMany(l => l))
                DisposeIfNeeded(list);

            _map.Clear();
            if (_logDebug) Debug.Log("[ServiceLocator] Cleared all services.");
        }

        private static void DisposeIfNeeded(Registration reg)
        {
            if (reg.Lifetime == Lifetime.Singleton &&
                reg.Singleton!.IsValueCreated &&
                reg.Singleton.Value is IDisposable d)
            {
                try { d.Dispose(); }
                catch (Exception ex)
                {
                    Debug.LogError($"[ServiceLocator] Dispose failed: {ex}");
                }
            }
        }
        
        // Immutable description of a binding
        private sealed class Registration {
            private Lifetime _lifetime;
            private Func<object> _factory;
            private Lazy<object>? _singleton;
            
            
            
            public Registration(Lifetime lifetime, Func<object> factory, Lazy<object>? singleton)
            {
                _lifetime = lifetime;
                _factory = factory ?? throw new ArgumentNullException(nameof(factory));
                _singleton = singleton;
            }

            public Lifetime Lifetime {
                get => _lifetime;
                set => _lifetime = value;
            }

            public Func<object> Factory {
                get => _factory;
                set => _factory = value;
            }

            public Lazy<object>? Singleton {
                get => _singleton;
                set => _singleton = value;
            }
        }
    }
}
