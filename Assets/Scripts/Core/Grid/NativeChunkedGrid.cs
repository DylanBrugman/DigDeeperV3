// NativeChunkedGrid.cs

using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Core.Grid {
// NativeChunkedGrid.cs
    using System;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Mathematics;

    /// <summary>
    /// A thread-safe, sparse 2-D grid divided into fixed-size square chunks.
    /// Designed for high-performance access from Burst-compiled jobs.
    /// </summary>
    /// <typeparam name="T">An unmanaged value type stored per tile.</typeparam>
    [BurstCompile]
    internal struct NativeChunkedGrid<T> : IDisposable where T : unmanaged {
        // ───────────────────────────────────────── Constants
        public const int ChunkSide = 32; // tiles per chunk edge
        public const int ChunkShift = 5; // 2^5 = 32
        private const int ChunkMask = ChunkSide - 1; // 0b11111
        private const int CellsPerChunk = ChunkSide * ChunkSide;

        // ───────────────────────────────────────── Backing data
        private NativeParallelHashMap<int2, int> _chunkLookup; // maps chunk coord → chunk index
        private NativeList<T> _data; // flat storage for all cells
        private NativeParallelHashSet<int2> _dirtyChunks; // chunks written this frame

        // ───────────────────────────────────────── Construction / Disposal
        public NativeChunkedGrid(int initialChunkCapacity, Allocator allocator) {
            _chunkLookup = new NativeParallelHashMap<int2, int>(initialChunkCapacity, allocator);
            _dirtyChunks = new NativeParallelHashSet<int2>(initialChunkCapacity, allocator);
            _data = new NativeList<T>(initialChunkCapacity * CellsPerChunk, allocator);
            
        }

        public void Dispose() {
            if (_chunkLookup.IsCreated) _chunkLookup.Dispose();
            if (_dirtyChunks.IsCreated) _dirtyChunks.Dispose();
            if (_data.IsCreated) _data.Dispose();
        }

        // ───────────────────────────────────────── Main-thread helpers
        /// <summary>Sets a tile, creating its chunk if necessary (main thread only).</summary>
        public void SetValue(int x, int y, in T value) {
            int2 chunkCoord = new int2(x >> ChunkShift, y >> ChunkShift);

            if (!_chunkLookup.TryGetValue(chunkCoord, out int chunkIndex))
                chunkIndex = CreateChunk(chunkCoord);
            
            int localIndex = (x & ChunkMask) + (y & ChunkMask) * ChunkSide;
            _data[chunkIndex * CellsPerChunk + localIndex] = value;
            _dirtyChunks.Add(chunkCoord);
        }

        /// <summary>Gets a tile; returns default(T) if its chunk is missing (main thread only).</summary>
        public T GetValue(int x, int y) {
            int2 chunkCoord = new int2(x >> ChunkShift, y >> ChunkShift);
            if (!_chunkLookup.TryGetValue(chunkCoord, out int chunkIndex))
                return default;

            int localIndex = (x & ChunkMask) + (y & ChunkMask) * ChunkSide;
            return _data[chunkIndex * CellsPerChunk + localIndex];
        }

        /// <summary>Ensures a chunk exists at the coordinate (main thread only).</summary>
        public bool EnsureChunkExists(int2 chunkCoord) {
            if (_chunkLookup.ContainsKey(chunkCoord)) return false;
            CreateChunk(chunkCoord);
            return true;
        }

        /// <summary>Returns a copy of dirty chunk coords; optionally clears the set.</summary>
        public NativeArray<int2> GetDirtyChunks(Allocator allocator, bool clear = true) {
            var array = _dirtyChunks.ToNativeArray(allocator);
            if (clear) _dirtyChunks.Clear();
            return array;
        }

        // ───────────────────────────────────────── Parallel accessors
        public ParallelWriter AsParallelWriter() => new ParallelWriter {ChunkLookup = _chunkLookup, Data = _data, DirtyChunks = _dirtyChunks.AsParallelWriter()};
        public ParallelReader AsParallelReader() => new ParallelReader {ChunkLookup = _chunkLookup, Data = _data.AsArray()};

        // ───────────────────────────────────────── Internal helpers
        private int CreateChunk(int2 chunkCoord) {
            int newChunkIndex = _data.Length / CellsPerChunk;
            _data.ResizeUninitialized(_data.Length + CellsPerChunk);
            _chunkLookup.Add(chunkCoord, newChunkIndex);
            return newChunkIndex;
        }

        // ══════════════════════════════════════════ PARALLEL WRITER ══════════════════════════════════════════
        [BurstCompile]
        public struct ParallelWriter {
            [ReadOnly] public NativeParallelHashMap<int2, int> ChunkLookup;

            [NativeDisableParallelForRestriction] internal NativeList<T> Data;

            public NativeParallelHashSet<int2>.ParallelWriter DirtyChunks;

            /// <summary>Gets a writable view of a pre-allocated chunk.</summary>
            public NativeChunk<T> GetChunk(int2 chunkCoord) {
                int chunkIndex = ChunkLookup[chunkCoord];
                int startIndex = chunkIndex * CellsPerChunk;

                var array = Data.AsArray(); // alias without copy
                var slice = new NativeSlice<T>(array, startIndex, CellsPerChunk);

                return new NativeChunk<T>(slice, chunkCoord, DirtyChunks, ChunkSide);
            }

            /// <summary>Sets a single tile by world coordinate (chunk must already exist).</summary>
            public void SetValue(int x, int y, in T value) {
                int2 chunkCoord = new int2(x >> ChunkShift, y >> ChunkShift);

                if (ChunkLookup.TryGetValue(chunkCoord, out int chunkIndex)) {
                    int localIndex = (x & ChunkMask) + (y & ChunkMask) * ChunkSide;
                    Data[chunkIndex * CellsPerChunk + localIndex] = value;
                    DirtyChunks.Add(chunkCoord);
                }
            }
        }

        // ══════════════════════════════════════════ PARALLEL READER ══════════════════════════════════════════
        [BurstCompile]
        public struct ParallelReader {
            [ReadOnly] public NativeParallelHashMap<int2, int> ChunkLookup;
            [ReadOnly] public NativeArray<T> Data;

            /// <summary>Gets a read-only view of a pre-allocated chunk.</summary>
            public NativeChunkReadOnly GetChunk(int2 chunkCoord) {
                int chunkIndex = ChunkLookup[chunkCoord];
                int startIndex = chunkIndex * CellsPerChunk;

                var slice = new NativeSlice<T>(Data, startIndex, CellsPerChunk);
                return new NativeChunkReadOnly(slice);
            }
        }

        // ══════════════════════════════════════════ CHUNK VIEWS ══════════════════════════════════════════════
        /// <summary>Writable chunk view for job code.</summary>
        public struct NativeChunk<U> where U : unmanaged {
            [NativeDisableParallelForRestriction] private NativeSlice<U> _cells;
            private readonly int2 _chunkCoord;
            private NativeParallelHashSet<int2>.ParallelWriter _dirtyTracker;
            private readonly int _side;

            internal NativeChunk(NativeSlice<U> cells, int2 coord, NativeParallelHashSet<int2>.ParallelWriter dirty, int side) {
                _cells = cells;
                _chunkCoord = coord;
                _dirtyTracker = dirty;
                _side = side;
            }

            /// <summary>Unsafe reference access for maximum speed. Caller must be in an unsafe context.</summary>
            public unsafe ref U Get(int localX, int localY) {
                int index = localX + localY * _side;
                void* baseP = _cells.GetUnsafePtr();
                void* elemP = (byte*) baseP + index * UnsafeUtility.SizeOf<U>();
                return ref UnsafeUtility.AsRef<U>(elemP);
            }

            /// <summary>Safe value read (copy) – no unsafe context required.</summary>
            public U GetValue(int localX, int localY) => _cells[localX + localY * _side];

            /// <summary>Writes a value and marks the parent chunk dirty.</summary>
            public void Set(int localX, int localY, in U value) {
                _cells[localX + localY * _side] = value;
                _dirtyTracker.Add(_chunkCoord);
            }
        }

        /// <summary>Read-only chunk view (safe, copy-based).</summary>
        public struct NativeChunkReadOnly {
            [ReadOnly] private NativeSlice<T> _cells;
            internal NativeChunkReadOnly(NativeSlice<T> cells) => _cells = cells;

            public T GetValue(int localX, int localY) => _cells[localX + localY * ChunkSide];
        }
    }
}


// using System;
// using System.Runtime.CompilerServices;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Mathematics;
//
// /// <summary>
// /// A thread-safe, 2D sparse grid divided into 32x32 chunks.
// /// Follows the ParallelWriter pattern for safe parallel job modifications.
// /// </summary>
// /// <typeparam name="T">An unmanaged value type.</typeparam>
// [BurstCompile]
// public struct NativeChunkedGrid<T> : IDisposable where T : unmanaged {
//     public const int ChunkSide = 32;
//     private const int CellsPerChunk = ChunkSide * ChunkSide;
//
//     // (chunkCoord → chunkIndex in _data)
//     // This is read-only in parallel contexts.
//     private NativeParallelHashMap<int2, int> _chunkLookup;
//
//     // Contiguous list of all cell data for all chunks.
//     private NativeList<T> _data;
//
//     // Set of chunks that have been modified.
//     private NativeParallelHashSet<int2> _dirtyChunks;
//
//     // ==========================================================================
//     // Construction / Disposal
//     // ==========================================================================
//
//     public NativeChunkedGrid(int2 chunkCount, Allocator allocator, bool preallocate = false) {
//         int estimatedChunks = chunkCount.x * chunkCount.y;
//
//         _chunkLookup = new NativeParallelHashMap<int2, int>(estimatedChunks, allocator);
//         _dirtyChunks = new NativeParallelHashSet<int2>(estimatedChunks, allocator);
//         _data = new NativeList<T>(estimatedChunks * CellsPerChunk, allocator);
//
//         if (preallocate) {
//             for (int chunkY = 0; chunkY < chunkCount.y; ++chunkY)
//             for (int chunkX = 0; chunkX < chunkCount.x; ++chunkX)
//                 CreateChunk(new int2(chunkX, chunkY));
//         }
//     }
//
//     public void Dispose() {
//         // Check IsCreated as a safety measure.
//         if (_chunkLookup.IsCreated) _chunkLookup.Dispose();
//         if (_dirtyChunks.IsCreated) _dirtyChunks.Dispose();
//         if (_data.IsCreated) _data.Dispose();
//     }
//
//     // ==========================================================================
//     // Main Thread API
//     // ==========================================================================
//
//     /// <summary>
//     /// [Main Thread Only] Gets a value. Returns default(T) if the chunk doesn't exist.
//     /// </summary>
//     public T GetValue(int x, int y) {
//         int2 chunkCoord = new int2(x >> 5, y >> 5);
//         if (!_chunkLookup.TryGetValue(chunkCoord, out int chunkIndex)) {
//             return default;
//         }
//
//         int localIndex = (x & 31) + (y & 31) * ChunkSide;
//         return _data[chunkIndex * CellsPerChunk + localIndex];
//     }
//
//     /// <summary>
//     /// [Main Thread Only] Sets a value, creating the chunk if it doesn't exist.
//     /// </summary>
//     public void SetValue(int x, int y, in T value) {
//         int2 chunkCoord = new int2(x >> 5, y >> 5);
//         if (!_chunkLookup.TryGetValue(chunkCoord, out int chunkIndex)) {
//             // This is the part that is NOT thread-safe, so it must stay on the main thread.
//             chunkIndex = CreateChunk(chunkCoord);
//         }
//
//         int localIndex = (x & 31) + (y & 31) * ChunkSide;
//         _data[chunkIndex * CellsPerChunk + localIndex] = value;
//         _dirtyChunks.Add(chunkCoord);
//     }
//
//     /// <summary>
//     /// [Main Thread Only] Ensures a chunk exists. Call this before a job to pre-allocate memory.
//     /// </summary>
//     /// <returns>True if a new chunk was created, false if it already existed.</returns>
//     public bool EnsureChunkExists(int2 chunkCoord) {
//         if (_chunkLookup.ContainsKey(chunkCoord)) {
//             return false;
//         }
//
//         CreateChunk(chunkCoord);
//         return true;
//     }
//
//     /// <summary>
//     /// Returns a writer that can be used safely in parallel jobs.
//     /// </summary>
//     public ParallelWriter AsParallelWriter() {
//         return new ParallelWriter {
//             // Pass the collections by value.
//             ChunkLookup = _chunkLookup, Data = _data, DirtyChunks = _dirtyChunks.AsParallelWriter()
//         };
//     }
//
//     /// <summary>
//     /// [Main Thread Only] Gets a copy of the dirty chunk coordinates and optionally clears the list.
//     /// </summary>
//     public NativeArray<int2> GetDirtyChunks(Allocator allocator, bool clearList = true) {
//         var array = _dirtyChunks.ToNativeArray(allocator);
//         if (clearList) {
//             _dirtyChunks.Clear();
//         }
//
//         return array;
//     }
//
//     private int CreateChunk(int2 chunkCoord) {
//         int newChunkIndex = _data.Length / CellsPerChunk;
//         _data.ResizeUninitialized(_data.Length + CellsPerChunk); // More performant if we overwrite anyway
//         _chunkLookup.Add(chunkCoord, newChunkIndex);
//         return newChunkIndex;
//     }
//
//     // ==========================================================================
//     // Parallel Writer Struct
//     // ==========================================================================
//
//     /// <summary>
//     /// A parallel-safe writer for the NativeChunkedGrid.
//     /// Can read and write to existing chunks, but cannot create new ones.
//     /// </summary>
//     [BurstCompile]
//     public struct ParallelWriter
//     {
//         [ReadOnly]
//         public NativeParallelHashMap<int2, int> ChunkLookup;
//
//         [NativeDisableParallelForRestriction]
//         private NativeList<T> Data;
//         public NativeParallelHashSet<int2>.ParallelWriter DirtyChunks;
//
//         /// <summary>
//         /// [Job Safe] Gets a NativeChunk handle. This method is unsafe because
//         /// it returns a pointer-based unsafe struct.
//         /// </summary>
//         // ⭐ Add the 'unsafe' keyword here
//         public unsafe NativeChunk<T> GetChunk(int2 chunkCoord)
//         {
//             
//             int chunkIndex = ChunkLookup[chunkCoord];
//             int startIndex = chunkIndex * CellsPerChunk;
//             
//             var slice = Data.AsParallelReader().Slice(startIndex, CellsPerChunk);
//
//             return new NativeChunk<T>(slice, chunkCoord, DirtyChunks, ChunkSide);
//         }
//
//         public void SetValue(int x, int y, in T value)
//         {
//             int2 chunkCoord = new int2(x >> 5, y >> 5);
//             if (ChunkLookup.TryGetValue(chunkCoord, out int chunkIndex))
//             {
//                 int localIndex = (x & (ChunkSide - 1)) + (y & (ChunkSide - 1)) * ChunkSide;
//                 Data[chunkIndex * CellsPerChunk + localIndex] = value;
//                 DirtyChunks.Add(chunkCoord);
//             }
//         }
//     }
// }
//
//
// // using System;
// // using System.Linq;
// //
// // namespace GamePlay.World.Tilemap {
// //     using Unity.Collections;
// //     using Unity.Mathematics;
// //
// //     public abstract class NativeChunkedGrid<T> : IDisposable where T : struct
// //     {
// //         const int SIZE = 32;
// //         public readonly int2 SizeChunks;
// //
// //         // hash-map <chunkKey, NativeArray<T>>
// //         NativeParallelHashMap<int2, NativeArray<T>> _chunks;
// //
// //         public NativeChunkedGrid(int2 sizeChunks, Allocator alloc, bool preallocate = false)
// //         {
// //             SizeChunks = sizeChunks;
// //             _chunks = new NativeParallelHashMap<int2, NativeArray<T>>(
// //                 sizeChunks.x * sizeChunks.y / 4 + 16, alloc);
// //
// //             if (preallocate)
// //                 for (int cy = 0; cy < sizeChunks.y; cy++)
// //                 for (int cx = 0; cx < sizeChunks.x; cx++)
// //                     _chunks.TryAdd(new int2(cx, cy), new NativeArray<T>(SIZE * SIZE, alloc, NativeArrayOptions.ClearMemory));
// //         }
// //
// //         // called ONLY from the main thread (allocating is not Burst-safe)
// //         public ref T GetWrite(int x, int y)
// //         {
// //             int2 key = new int2(x>>5, y>>5);
// //             if (!_chunks.TryGetValue(key, out var arr))
// //             {
// //                 arr = new NativeArray<T>(SIZE*SIZE, Allocator.Persistent, NativeArrayOptions.ClearMemory);
// //                 _chunks.TryAdd(key, arr);
// //             }
// //             return ref arr.ElementAt((x&31) + (y&31)*SIZE);
// //         }
// //
// //         // read-only ref safe in Burst jobs
// //         public ref readonly T Get(int x, int y)
// //         {
// //             int2 key = new int2(x>>5, y>>5);
// //             if (_chunks.TryGetValue(key, out var arr))
// //                 return ref arr.ElementAt((x&31)+(y&31)*SIZE);
// //
// //             return ref Chunk<T>.EmptyCell;          // default(T)
// //         }
// //         
// //         public  GetDirtyChunks() {
// //         }
// //
// //         public void Dispose()
// //         {
// //             var it = _chunks.GetEnumerator();
// //             while (it.MoveNext())
// //                 it.Current.Value.Dispose();
// //             _chunks.Dispose();
// //         }
// //     }
// //
// // }