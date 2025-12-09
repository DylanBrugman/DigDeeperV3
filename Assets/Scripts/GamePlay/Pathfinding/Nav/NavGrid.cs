using System;
using Core.Grid;
using GamePlay.World.Tilemap;
using Unity.Collections;
using Unity.Mathematics;

namespace GamePlay.Pathfinding.Nav {
    public class NavGrid : ChunkedGrid<NavBits> {
        public NavGrid(int2 sizeChunks, bool preallocate = false) : base(sizeChunks) { }
    }
}

// using System.Collections.Generic;
// using System.Linq;
// using GamePlay.World.Tilemap;
// using GamePlay.World.Tilemap.Generator;
// using Unity.Mathematics;
//
// namespace GamePlay.Pathfinding.Nav {
//     /// <summary>
//     /// Instance NavGrid that mirrors TileGrid chunk layout. Rebuild once at load,
//     /// then per‑chunk when terrain changes. No static state.
//     /// </summary>
//     public sealed class NavGrid : System.IDisposable
//     {
//         private readonly Dictionary<int2, NavChunk> _chunks = new();
//         private readonly int2 _sizeChunks;
//
//         public NavGrid(int2 sizeChunks) => _sizeChunks = sizeChunks;
//         public int ChunkSize => NavChunk.Size;
//
//         public NavChunk GetOrCreate(int2 coord)
//         {
//             if (!_chunks.TryGetValue(coord, out var c))
//                 _chunks[coord] = c = new NavChunk();
//             return c;
//         }
//
//         public ref byte FlagsAt(int x,int y)
//         {
//             var key = new int2(x>>5, y>>5);
//             int lx = x & 31, ly = y & 31;
//             return ref GetOrCreate(key).At(lx,ly);
//         }
//
//         public IEnumerable<(int2,NavChunk)> DirtyChunks() => _chunks.Where(kv => kv.Value.Dirty).Select(kv => (kv.Key, kv.Value));
//
//         public void Preallocate()
//         {
//             for (int cy=0; cy<_sizeChunks.y; cy++)
//             for (int cx=0; cx<_sizeChunks.x; cx++)
//                 GetOrCreate(new int2(cx,cy));
//         }
//
//         public void RebuildFrom(TileGrid tiles)
//         {
//             foreach (var (coord, chunk) in tiles.AllChunks())
//             {
//                 var navChunk = GetOrCreate(coord);
//                 for (int y=0; y<NavChunk.Size; y++)
//                 for (int x=0; x<NavChunk.Size; x++)
//                 {
//                     var globalX = (coord.x<<5)+x;
//                     var globalY = (coord.y<<5)+y;
//
//                     ref var here  = ref tiles.TileAt(globalX,  globalY);
//                     ref var below = ref tiles.TileAt(globalX,  globalY-1);
//                     ref var above = ref tiles.TileAt(globalX,  globalY+1);
//
//                     byte floor=0;
//                     if (below.Type != TileType.Air && here.Type == TileType.Air && above.Type == TileType.Air) {
//                         floor |= 1<<0; // Standable                        
//                     }
//
//                     navChunk.At(x,y) = floor;
//                 }
//                 navChunk.Dirty=false;
//             }
//         }
//
//         public void Dispose() => _chunks.Clear();
//     }
// }