// using System;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Mathematics;
//
// namespace Core.Grid {
//     public class ChunkedGrid<T> : IDisposable where T : unmanaged {
//         private readonly NativeChunkedGrid<T> _core;
//
//         /// Map width / height in chunks.
//         public int2 WorldChunks { get; }
//
//         /// Map width / height in tiles.
//         public int2 WorldTiles { get; }
//         
//         public int ChunkSize => NativeChunkedGrid<T>.ChunkSide;
//         public static int CHUNK_SIZE => NativeChunkedGrid<T>.ChunkSide;
//
//         // ───────────────────────────────────────── Construction
//         public ChunkedGrid(int2 worldChunks,
//             Allocator allocator = Allocator.Persistent) {
//             if (worldChunks.x <= 0 || worldChunks.y <= 0)
//                 throw new ArgumentException("worldChunks must be positive");
//
//             WorldChunks = worldChunks;
//             WorldTiles = worldChunks * NativeChunkedGrid<T>.ChunkSide;
//
//             int totalChunks = worldChunks.x * worldChunks.y;
//             _core = new NativeChunkedGrid<T>(totalChunks, allocator);
//
//             // Pre-allocate every chunk so jobs never need to create one.
//             for (int cy = 0; cy < worldChunks.y; ++cy) {
//                 for (int cx = 0; cx < worldChunks.x; ++cx) {
//                     _core.EnsureChunkExists(new int2(cx, cy));
//                 }
//             }
//         }
//
//         // ───────────────────────────────────────── Bounds helpers
//         private bool InBounds(int x, int y)
//             => (uint) x < (uint) WorldTiles.x && (uint) y < (uint) WorldTiles.y;
//
//         private void ThrowIfOOB(int x, int y) {
//             if (!InBounds(x, y))
//                 throw new ArgumentOutOfRangeException(
//                     $"Tile ({x},{y}) lies outside map bounds {WorldTiles}.");
//         }
//
//         // ───────────────────────────────────────── Public API
//         public void Set(int x, int y, in T value) {
//             ThrowIfOOB(x, y);
//             _core.SetValue(x, y, in value); // chunk already exists
//         }
//
//         public T Get(int x, int y) {
//             ThrowIfOOB(x, y);
//             return _core.GetValue(x, y);
//         }
//
//         /// Dirty-chunk list (main-thread only).
//         public NativeArray<int2> GetDirtyChunks(Allocator a, bool clear = true)
//             => _core.GetDirtyChunks(a, clear);
//
//         // ───────────────────────────────────────── Cleanup
//         public void Dispose() => _core.Dispose();
//
//         public GridWriter<T> AsParallelWriter()
//             => new GridWriter<T> {_inner = _core.AsParallelWriter()};
//
//         public GridReader<T> AsParallelReader()
//             => new GridReader<T> {_inner = _core.AsParallelReader()};
//
//         public struct GridReader<T> where T : unmanaged {
//             internal NativeChunkedGrid<T>.ParallelReader _inner;
//
//             [BurstCompile]
//             internal NativeChunkedGrid<T>.NativeChunkReadOnly GetChunk(int2 chunkCoord) => _inner.GetChunk(chunkCoord);
//         }
//
//         public struct GridWriter<T> where T : unmanaged {
//             // keep the real writer hidden
//             internal NativeChunkedGrid<T>.ParallelWriter _inner;
//
//             // forward only the operations you allow users to perform
//             [BurstCompile]
//             internal NativeChunkedGrid<T>.NativeChunk<T> GetChunk(int2 chunkCoord) => _inner.GetChunk(chunkCoord);
//
//             [BurstCompile]
//             public void SetValue(int x, int y, in T value) => _inner.SetValue(x, y, in value);
//         }
//     }
// }
// // using System;
// // using System.Collections.Generic;
// // using System.Linq;
// // using Unity.Mathematics;
// //
// // namespace GamePlay.World.Tilemap
// // {
// //     public class ChunkedGrid<T> : IDisposable where T : struct
// //     {
// //         private readonly Dictionary<int2, Chunk<T>> _chunks = new();
// //         private readonly int2 _sizeChunks;          // world bounds hint
// //         private readonly bool _preallocate;
// //
// //         public ChunkedGrid(int2 sizeChunks, bool preallocate = false)
// //         {
// //             _sizeChunks  = sizeChunks;
// //             _preallocate = preallocate;
// //             if (preallocate) PreallocateAll();
// //         }
// //
// //         /* fast cell access ----------------------------------------------------*/
// //         public ref T CellAt(int x, int y)
// //         {
// //             int2 key = new int2(x >> 5, y >> 5);
// //             int lx = x & 31, ly = y & 31;
// //
// //             if (!_chunks.TryGetValue(key, out var c))
// //                 _chunks[key] = c = new Chunk<T>();            // allocate on write
// //
// //             c.Dirty = true;
// //             return ref c.At(lx, ly);
// //         }
// //
// //         public bool TryGetCell(int x, int y, out T value)
// //         {
// //             int2 key = new int2(x >> 5, y >> 5);
// //             if (_chunks.TryGetValue(key, out var c))
// //             {
// //                 value = c.At(x & 31, y & 31);
// //                 return true;
// //             }
// //             value = default;
// //             return false;
// //         }
// //
// //         /* dirty-chunk enumeration --------------------------------------------*/
// //         public IEnumerable<(int2 coord, Chunk<T> chunk)> DirtyChunks()
// //             => _chunks.Where(kv => kv.Value.Dirty)
// //                       .Select(kv => (kv.Key, kv.Value));
// //
// //         public void ClearDirtyFlags()
// //         {
// //             foreach (var c in _chunks.Values) c.Dirty = false;
// //         }
// //
// //         /* bulk helpers -------------------------------------------------------*/
// //         private void PreallocateAll()
// //         {
// //             for (int cy = 0; cy < _sizeChunks.y; cy++)
// //             for (int cx = 0; cx < _sizeChunks.x; cx++)
// //                 _chunks[new int2(cx, cy)] = new Chunk<T>();
// //         }
// //
// //         public void FillFrom(Chunk<T>[] srcChunks)
// //         {
// //             int idx = 0;
// //             for (int cy = 0; cy < _sizeChunks.y; cy++)
// //             for (int cx = 0; cx < _sizeChunks.x; cx++, idx++)
// //             {
// //                 var dst = GetOrCreateChunk(new int2(cx, cy));
// //                 srcChunks[idx].Cells.CopyTo(dst.Cells, 0);
// //                 dst.Dirty = true;
// //             }
// //         }
// //
// //         /* optional reclaim of empty chunks -----------------------------------*/
// //         public void MaybeRemoveEmptyChunk(int2 key, Func<T, bool> isZero)
// //         {
// //             if (_chunks.TryGetValue(key, out var c) && c.IsEmpty(isZero))
// //                 _chunks.Remove(key);
// //         }
// //
// //         /* helpers ------------------------------------------------------------*/
// //         private Chunk<T> GetOrCreateChunk(int2 key)
// //         {
// //             if (!_chunks.TryGetValue(key, out var c))
// //                 _chunks[key] = c = new Chunk<T>();
// //             return c;
// //         }
// //
// //         public void Dispose() => _chunks.Clear();
// //     }
// // }
// //
// //
// // // using System.Collections.Generic;
// // // using System.Linq;
// // // using Unity.Mathematics;
// // //
// // // namespace GamePlay.World.Tilemap {
// // //     
// // //     public class ChunkedGrid<T> where T : struct {
// // //         private readonly Dictionary<int2, Chunk<T>> _chunks = new();
// // //         private readonly int2 _sizeChunks; // bounds hint
// // //
// // //         public ChunkedGrid(int2 sizeChunks) {
// // //             _sizeChunks = sizeChunks;
// // //             Preallocate();
// // //         }
// // //
// // //         /* helpers ------------------------------------------------------------ */
// // //
// // //         private Chunk<T> GetOrCreateChunk(int2 key) {
// // //             if (!_chunks.TryGetValue(key, out var c))
// // //                 _chunks[key] = c = new Chunk<T>();
// // //             return c;
// // //         }
// // //
// // //         public ref T CellAt(int x, int y) {
// // //             int2 key = new int2(x >> 5, y >> 5);
// // //             int lx = x & 31;
// // //             int ly = y & 31;
// // //             var chunk = GetOrCreateChunk(key);
// // //             chunk.Dirty = true;
// // //             return ref chunk.At(lx, ly);
// // //         }
// // //
// // //         public IEnumerable<(int2, Chunk<T>)> DirtyChunks()
// // //             => _chunks.Where(kv => kv.Value.Dirty)
// // //                 .Select(kv => (kv.Key, kv.Value));
// // //
// // //         /* bulk ops ----------------------------------------------------------- */
// // //
// // //         private void Preallocate() {
// // //             for (int cy = 0; cy < _sizeChunks.y; cy++)
// // //             for (int cx = 0; cx < _sizeChunks.x; cx++)
// // //                 GetOrCreateChunk(new int2(cx, cy));
// // //         }
// // //
// // //         public void ClearDirtyFlags() {
// // //             foreach (var c in _chunks.Values) c.Dirty = false;
// // //         }
// // //
// // //         public void Dispose() => _chunks.Clear();
// // //     }
// // // }