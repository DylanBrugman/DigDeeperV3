using System;
using Core.Grid;
using Unity.Collections;
using Unity.Mathematics;

namespace GamePlay.World.Tilemap {
    public sealed class TileGrid : ChunkedGrid<Tile> {
        public TileGrid(int2 sizeChunks) : base(sizeChunks) { }
    }
}

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using GamePlay.Map.Generator.New.Core.WorldGen;
// using Unity.Mathematics;
//
// namespace GamePlay.World.Tilemap {
//     
//     public sealed class TileGrid : IDisposable
//     {
//         private readonly Dictionary<int2, TileChunk> _chunks = new();
//         private readonly int2 _worldSizeChunks;
//
//         public TileGrid(int2 worldSizeChunks) {
//             _worldSizeChunks = worldSizeChunks;
//             Preallocate();
//         }
//         
//         public TileGrid(int2 worldSizeChunks, TileChunk[] tileChunks) {
//             _worldSizeChunks = worldSizeChunks;
//             FillFrom(tileChunks);
//         }
//
//         /* ---------- chunk helpers ---------- */
//         public TileChunk GetOrCreateChunk(int2 key)
//         {
//             if (!_chunks.TryGetValue(key, out var tileChunk))
//                 _chunks[key] = tileChunk = new TileChunk();
//             return tileChunk;
//         }
//
//         public IEnumerable<(int2 coord, TileChunk chunk)> AllChunks()
//             => _chunks.Select(kv => (kv.Key, kv.Value));
//
//         /* ---------- tile helpers ---------- */
//         public ref Tile TileAt(int x,int y)
//         {
//             int2 key = new int2(x>>5, y>>5);
//             int lx = x & 31, ly = y & 31;
//             var chunk = GetOrCreateChunk(key);
//             chunk.Dirty = true;
//             return ref chunk.At(lx,ly);
//         }
//
//         public bool TryGetTile(int x,int y, out Tile t)
//         {
//             int2 key = new int2(x>>5, y>>5);
//             if (_chunks.TryGetValue(key, out var c))
//             {
//                 t = c.At(x&31, y&31); return true;
//             }
//             t = default; return false;
//         }
//
//         public IEnumerable<(int2, TileChunk)> DirtyChunks()
//             => _chunks.Where(kv => kv.Value.Dirty).Select(kv => (kv.Key, kv.Value));
//
//         public void ClearDirtyFlags()
//         {
//             foreach (var c in _chunks.Values) c.Dirty = false;
//         }
//
//         /* ---------- bulk helpers ---------- */
//         public void Preallocate()
//         {
//             for (int cy = 0; cy < _worldSizeChunks.y; cy++) {
//                 for (int cx = 0; cx < _worldSizeChunks.x; cx++) {
//                     GetOrCreateChunk(new int2(cx,cy));                                    
//                 }
//             }
//         }
//
//         public void FillFrom(TileChunk[] tileChunks)
//         {
//             int idx=0;
//             for (int cy=0; cy<_worldSizeChunks.y; cy++)
//             for (int cx=0; cx<_worldSizeChunks.x; cx++, idx++)
//             {
//                 var chunk = GetOrCreateChunk(new int2(cx,cy));
//                 tileChunks[idx].NativeTileGrid.CopyTo(chunk.NativeTileGrid,0);
//                 chunk.Dirty = true;
//             }
//         }
//
//         // //Depth is based on solid tiles above
//         // public int GetDepthAt(int x, int y) {
//         //     
//         // }
//
//         public void Dispose() => _chunks.Clear();
//     }
// }