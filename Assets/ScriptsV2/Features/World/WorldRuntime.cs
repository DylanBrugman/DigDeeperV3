// using GamePlay.Map.Generator.New;
// using GamePlay.Map.Generator.New.Core.WorldGen;
// using GamePlay.Pathfinding.Core.Nav;
// using Unity.Mathematics;
//
// namespace GamePlay.World {
//     /// <summary>
//     /// Central service that owns the *mutable* runtime state: TileGrid, NavGrid, LiquidGrid, etc.
//     /// Gameplay systems access data ONLY through this facade — no more static singletons sprinkled around.
//     /// </summary>
//     public static class WorldRuntime
//     {
//         public static int2 SizeChunks { get; private set; }
//
//         public static void ClearAll()
//         {
//             TileGrid.Clear();
//             
//             NavGrid.Clear();
//             // TODO: LiquidGrid.Clear(); GasGrid.Clear(); when implemented
//         }
//
//         public static void Apply(WorldData worldData)
//         {
//             SizeChunks = worldData.SizeChunks;
//             ApplyTiles(worldData.NativeTileGrid);
//             RebuildNavigation();
//             // TODO: ApplyLiquid(worldData.Liquid); ApplyGas(worldData.Gas);
//         }
//
//         private static void ApplyTiles(TileLayer layer)
//         {
//             TileGrid.Preallocate(SizeChunks);
//             int idx = 0;
//             for (int cy = 0; cy < SizeChunks.y; cy++)
//             for (int cx = 0; cx < SizeChunks.x; cx++, idx++)
//             {
//                 var dst = TileGrid.GetOrCreateChunk(cx, cy);
//                 layer.Chunks[idx].NativeTileGrid.CopyTo(dst.NativeTileGrid, 0);
//                 dst.Dirty = true;
//             }
//         }
//
//         private static void RebuildNavigation()
//         {
//             NavGrid.Clear();
//             NavGrid.Preallocate(SizeChunks);
//             foreach (var (coord, _) in TileGrid.AllChunks())
//                 NavRebuildSystem.RebuildChunk(coord);
//         }
//     }
// }