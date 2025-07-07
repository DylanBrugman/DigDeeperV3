// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using GamePlay.World.Tilemap;
// using GamePlay.World.Tilemap.Generator;
// using UnityEngine;
//
// namespace Systems.WorldSystem.Generator.MapProcessor.PostProcessor {
//     public class CaveMapProcessor : IMapProcessor
//     {
//         public string StepName => "Generating Caves (Map Pass)";
//
//         public void Process(Tile[,] world, TilemapGeneratorContext context)
//         {
//             var config = context.Config;
//             for (int x = 0; x < config.worldSize.x; x++)
//             {
//                 for (int y = 0; y < config.worldSize.y; y++)
//                 {
//                     var tile = world[x, y];
//                     if (tile.Type == TileType.Air || tile.currentDepth < 20) continue;
//
//                     float caveNoise = Mathf.PerlinNoise(
//                         x * config.caveScale + config.seed,
//                         y * config.caveScale + config.seed
//                     );
//
//                     if (caveNoise > config.caveThreshold)
//                     {
//                         tile.Type = TileType.Air;
//                     }
//                 }
//             }
//         }
//     }
// }