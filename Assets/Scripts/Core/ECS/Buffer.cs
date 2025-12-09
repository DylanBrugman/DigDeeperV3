namespace Core.ECS {
    // Core/ECS/Buffer.cs
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    namespace Core.ECS {
        /// <summary>Lightweight dynamic array for value-type ECS components.</summary>
        /// <remarks>
        /// *   **No GC allocations after the first grow.**  
        /// *   Safe default state – a zero-initialised Buffer acts like an empty one.  
        /// *   Designed to be mutated through <c>ref Buffer&lt;T&gt;</c>; copying the struct
        ///     only copies the *handle*, not the data.
        /// </remarks>
        public struct Buffer<T> {
            // ─────────── fields ───────────
            static readonly T[] Empty = Array.Empty<T>();

            T[] _data;
            int _count;

            // ───────── constructors ─────────
            public Buffer(int capacity) {
                _data = capacity > 0 ? new T[capacity] : Empty;
                _count = 0;
            }

            // ───────── public API ─────────
            public int Count => _count;
            public int Capacity => _data?.Length ?? 0;

            public ref T this[int index] {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref _data![index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(in T value) {
                EnsureCap(_count + 1);
                _data[_count++] = value;
            }

            /// <summary>Removes element at <paramref name="index"/> by swapping in the last slot (O(1)).</summary>
            public void RemoveAtSwapBack(int index) {
                _count--;
                if (index < _count)
                    _data[index] = _data[_count];
                _data[_count] = default!;
            }

            /// <summary>Remove first occurrence (swap-back). Returns <c>true</c> if found.</summary>
            public bool RemoveSwapBack(in T value) {
                int i = IndexOf(value);
                if (i < 0) return false;
                RemoveAtSwapBack(i);
                return true;
            }

            public void Clear() {
                Array.Clear(_data, 0, _count);
                _count = 0;
            }

            public bool Contains(in T value) => IndexOf(value) >= 0;

            public int IndexOf(in T value) {
                var cmp = EqualityComparer<T>.Default;
                for (int i = 0; i < _count; i++)
                    if (cmp.Equals(_data[i], value))
                        return i;
                return -1;
            }

            /// <summary>`Span&lt;T&gt;` view for ultra-fast iteration (`for` / `foreach (ref var x in ...)`).</summary>
            public Span<T> AsSpan() => _data.AsSpan(0, _count);

            // ───────── enumerator (ref-aware) ─────────
            public Enumerator GetEnumerator() => new(this);

            public ref struct Enumerator {
                readonly T[] _data;
                readonly int _count;
                int _i;

                internal Enumerator(in Buffer<T> buf) {
                    _data = buf._data ?? Empty;
                    _count = buf._count;
                    _i = -1;
                }

                public bool MoveNext() => ++_i < _count;
                public ref T Current => ref _data[_i];
            }

            // ───────── internal helpers ─────────
            void EnsureCap(int want) {
                if (_data == null)
                    _data = Empty;

                if (want <= _data.Length) return;

                int newCap = Math.Max(want, _data.Length == 0 ? 4 : _data.Length * 2);
                Array.Resize(ref _data, newCap);
            }
        }
    }
}