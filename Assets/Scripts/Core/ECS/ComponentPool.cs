using System;
using System.Collections.Generic;

namespace Core.ECS {
    
    public sealed class ComponentPool<T> where T : struct
    {
        const int INITIAL = 16;

        T[]   _data  = new T[INITIAL];
        int[] _owner = new int[INITIAL];           // slot → entityId
        readonly Dictionary<int,int> _index = new(); // entityId → slot
        int _count;

        /* ────────── CRUD ────────── */
        public void Add(EntityId e, in T c)
        {
            if (_index.ContainsKey(e.Value))
                throw new InvalidOperationException($"Entity {e} already has {typeof(T).Name}");

            EnsureCap(_count + 1);
            _data [_count] = c;
            _owner[_count] = e.Value;
            _index[e.Value] = _count;
            _count++;
        }

        public bool Has(EntityId e) => _index.ContainsKey(e.Value);

        public ref T Get(EntityId e) => ref _data[_index[e.Value]];

        public void Remove(EntityId e)
        {
            if (!_index.TryGetValue(e.Value, out int slot)) return;
            int last = _count - 1;
            if (slot != last)
            {
                _data [slot] = _data [last];
                _owner[slot] = _owner[last];
                _index[_owner[slot]] = slot;
            }
            _index.Remove(e.Value);
            _count--;
        }

        /* ────────── enumeration helpers ────────── */
        public int  Count            => _count;
        public ref T BySlot(int i)   => ref _data[i];
        public EntityId Owner(int i) => new EntityId(_owner[i]);

        void EnsureCap(int want)
        {
            if (want <= _data.Length) return;
            int cap = Math.Max(want, _data.Length * 2);
            Array.Resize(ref _data , cap);
            Array.Resize(ref _owner, cap);
        }
    }
}