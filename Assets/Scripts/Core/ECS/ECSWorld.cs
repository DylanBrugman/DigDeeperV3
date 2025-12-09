// Core/ECSWorld.cs

using System;
using System.Collections.Generic;

namespace Core.ECS {
    public sealed class ECSWorld {
        readonly Dictionary<Type, object> _pools = new();

        /* ───── entity ───── */
        public EntityId CreateEntity() => EntityId.New();

        /* ───── component helpers ───── */
        ComponentPool<T> Pool<T>() where T : struct {
            if (!_pools.TryGetValue(typeof(T), out var pool)) {
                _pools[typeof(T)] = pool = new ComponentPool<T>();
            }

            return (ComponentPool<T>) pool;
        }

        public void Add<T>(EntityId e, in T c) where T : struct => Pool<T>().Add(e, c);
        public bool Has<T>(EntityId e) where T : struct => Pool<T>().Has(e);
        public ref T Get<T>(EntityId e) where T : struct => ref Pool<T>().Get(e);
        public void Remove<T>(EntityId e) where T : struct => Pool<T>().Remove(e);

        /* ───── ref-enumeration ───── */
        public Enumerator<T> All<T>() where T : struct => new Enumerator<T>(Pool<T>());

        public ref struct Enumerator<T> where T : struct {
            readonly ComponentPool<T> _pool;
            int _index;

            public Enumerator(ComponentPool<T> pool) {
                _pool = pool;
                _index = -1;
            }

            public bool MoveNext() => ++_index < _pool.Count;

            public EntityId EntityId => _pool.Owner(_index);
            
            /// <summary>
            /// Gets the component at the current index.
            /// 
            /// Use ref to avoid copying the component.
            /// Use ref for writing to the component.
            /// Use without ref for only reading the component.
            /// You can not write without ref, because it wouldn't write back to the pool.
            /// </summary>
            public ref T Component => ref _pool.BySlot(_index);
        }

        /* ───────────── 2-component query ───────────── */
        public Enumerator2<TA, TB> All<TA, TB>() where TA : struct where TB : struct => new Enumerator2<TA, TB>(this, Pool<TA>(), Pool<TB>());

        public ref struct Enumerator2<TA, TB>
            where TA : struct where TB : struct {
            readonly ECSWorld _ecsWorld;
            readonly ComponentPool<TA> _a;
            readonly ComponentPool<TB> _b;
            int _i;

            public Enumerator2(ECSWorld ecsWorld, ComponentPool<TA> a, ComponentPool<TB> b) {
                _ecsWorld = ecsWorld;
                _a = a;
                _b = b;
                _i = -1;
            }

            public bool MoveNext() {
                while (++_i < _a.Count)
                    if (_b.Has(_a.Owner(_i))) // only where both exist
                        return true;
                return false;
            }

            public EntityId EntitId => _a.Owner(_i);
            public ref TA A => ref _a.BySlot(_i);
            public ref TB B => ref _b.Get(_a.Owner(_i)); // second pool by lookup
        }

        /* ───────────── 3-component query (optional) ───────────── */
        public Enumerator3<TA, TB, TC> All<TA, TB, TC>()
            where TA : struct where TB : struct where TC : struct
            => new Enumerator3<TA, TB, TC>(this, Pool<TA>(), Pool<TB>(), Pool<TC>());

        public ref struct Enumerator3<TA, TB, TC>
            where TA : struct where TB : struct where TC : struct {
            readonly ECSWorld _w;
            readonly ComponentPool<TA> _a;
            readonly ComponentPool<TB> _b;
            readonly ComponentPool<TC> _c;
            int _i;

            public Enumerator3(ECSWorld w, ComponentPool<TA> a, ComponentPool<TB> b, ComponentPool<TC> c) {
                _w = w;
                _a = a;
                _b = b;
                _c = c;
                _i = -1;
            }

            public bool MoveNext() {
                while (++_i < _a.Count) {
                    var id = _a.Owner(_i);
                    if (_b.Has(id) && _c.Has(id)) return true;
                }

                return false;
            }

            public EntityId Id => _a.Owner(_i);
            public ref TA A => ref _a.BySlot(_i);
            public ref TB B => ref _b.Get(Id);
            public ref TC C => ref _c.Get(Id);
        }
    }
}