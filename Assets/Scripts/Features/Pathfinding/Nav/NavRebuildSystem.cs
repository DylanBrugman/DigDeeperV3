// using Core;
// using GamePlay.Map.Generator.New.Core.WorldGen;
// using GamePlay.World;
// using GamePlay.World.Tilemap;
// using GamePlay.World.Tilemap.Generator;
// using Unity.Collections;
// using Unity.Mathematics;
// using UnityEngine;
//
// // using Tile = GamePlay.Map.NativeTileGrid.Tile;
// // using TileType = GamePlay.Map.NativeTileGrid.TileType;
//
// namespace GamePlay.Pathfinding.Nav {
//     /// <summary>
//     /// Recomputes NavBits for any <see cref="TileChunk"/> that changed this frame.
//     /// Uses simple rules: Standable, Climbable, Water. Extend as needed.
//     /// </summary>
//     public sealed class NavRebuildSystem {
//         // flag constants reused from Tile.Flags (ladder, rope)
//         private const byte HasLadder = 1 << 1;
//         private const byte HasRope = 1 << 2;
//
//         public int ProcessedEntitiesCount { get; private set; }
//
//         private WorldRuntimeContext _worldRuntimeContext;
//
//         public void Tick(float dt) {
//             ProcessedEntitiesCount = 0;
//
//             if (!ServiceLocator.TryGet(out _worldRuntimeContext)) {
//                 Debug.LogError("NavRebuildSystem: WorldRuntimeContext missing, skipping.");
//                 return;
//             }
//
//             var dirtyChunks = _worldRuntimeContext.NavGrid.GetDirtyChunks(Allocator.TempJob, clear: true);
//             foreach (var coord in dirtyChunks) {
//                 RebuildChunk(coord);
//                 ProcessedEntitiesCount++;
//             }
//
//             dirtyChunks.Dispose();
//         }
//
//         public static void RebuildChunk(int2 chunk) {
//             // NavGrid navGrid;
//             WorldRuntimeContext worldRuntimeContext = ServiceLocator.GetOrThrow<WorldRuntimeContext>();
//
//             NavGrid navGrid = worldRuntimeContext.NavGrid;
//             TileGrid tileGrid = worldRuntimeContext.TileGrid;
//             
//             int baseX = chunk.x * NavGrid.CHUNK_SIZE;
//             int baseY = chunk.y * NavGrid.CHUNK_SIZE;
//
//             for (int ly = 0; ly < NavGrid.CHUNK_SIZE; ly++) {
//                 for (int lx = 0; lx < NavGrid.CHUNK_SIZE; lx++) {
//                     int worldX = baseX + lx;
//                     int worldY = baseY + ly;
//
//                     Tile here = tileGrid.Get(worldX, worldY);
//                     Tile below = tileGrid.Get(worldX, worldY - 1);
//                     Tile above = tileGrid.Get(worldX, worldY + 1);
//
//                     NavBits curNavBits = navGrid.Get(worldX, worldY);
//                     NavBits newNavBits = curNavBits;
//
//                     // Standable: solid below, current & above empty
//                     if (below.Type != TileType.Air &&
//                         here.Type == TileType.Air &&
//                         above.Type == TileType.Air) {
//                         newNavBits = NavBits.Standable;                        
//                     }
//
//                     // Climbable: ladder/rope flag on current tile
//                     if ((here.Flags & (HasLadder | HasRope)) != 0)
//                         newNavBits |= NavBits.Climbable;
//                     
//                     // Water (simple) – treat Air tiles with liquid height > 0.5 as water
//                     // if (LiquidGrid.HeightAt(worldX, worldY) > 0.5f)
//                     //     f |= (byte)NavBits.Water;
//                     
//                     navGrid.Set(lx, ly, newNavBits);
//                 }
//             }
//         }
//     }
// }