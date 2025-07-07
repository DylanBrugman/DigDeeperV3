// using System;
// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using GamePlay.World.Tilemap.Generator;
// using Systems.WorldSystem.Generator;
// using Systems.WorldSystem.Generator.MapProcessor;
// using UnityEngine;
//
// namespace Systems.WorldSystem {
//     public class PhysicalPropertiesMapProcessor : IMapProcessor
//     {
//         public string StepName => "Calculating Physical Properties (Map Pass)";
//
//         public void Process(Tile[,] world, TilemapGeneratorContext context)
//         {
//             var config = context.Config;
//             for (int x = 0; x < config.worldSize.x; x++)
//             {
//                 for (int y = 0; y < config.worldSize.y; y++)
//                 {
//                     var tile = world[x, y];
//
//                     // Pressure might have been set by layers; this could refine or override.
//                     // The original code always set it here, let's keep that.
//                     tile.pressure = CalculatePressure(tile.currentDepth);
//
//                     // Stability depends on neighbors, must be a map pass.
//                     tile.stability = CalculateStability(world, x, y, context);
//
//                     // Temperature might have base from layer, modifier from biome. AddComponent geothermal gradient.
//                     tile.temperature += tile.currentDepth * 0.1f; // Example: 0.1 degree C per unit depth
//                     
//                     tile.matter = DetermineMatterType(tile.tileType, tile.temperature, tile.pressure);
//                 }
//             }
//         }
//
//         private Matter DetermineMatterType(TileType tileType, float tileTemperature, float tilePressure) {
//             if (tileType == TileType.Lava || tileType == TileType.Water)
//             {
//                 return Matter.Liquid;
//             } if (tileType == TileType.Air) {
//                 return Matter.Gas;
//             }
//             
//             return Matter.Solid;
//         }
//
//         private float CalculatePressure(float depth)
//         {
//             // Example: 1 ATM at surface, increases with depth
//             return 1f + (depth * 0.1f); 
//         }
//
//         private float CalculateStability(Tile[,] world, int x, int y, TilemapGeneratorContext context)
//         {
//             if (world[x, y].tileType == TileType.Air) return 0f;
//
//             float stability = 100f; // Start with max stability
//             int supportCount = 0;
//             const int MAX_NEIGHBORS = 8;
//
//             for (int dx = -1; dx <= 1; dx++)
//             {
//                 for (int dy = -1; dy <= 1; dy++)
//                 {
//                     if (dx == 0 && dy == 0) continue;
//
//                     var checkPos = new Vector2Int(x + dx, y + dy);
//                     if (context.IsValidPosition(checkPos))
//                     {
//                         // Consider different support strengths: cardinal vs diagonal, material of neighbor
//                         if (world[checkPos.x, checkPos.y].tileType != TileType.Air)
//                         {
//                             supportCount++;
//                         }
//                     }
//                     else // Edge of the ecsWorld often considered infinitely stable support
//                     {
//                         supportCount++;
//                     }
//                 }
//             }
//             
//             stability *= (supportCount / (float)MAX_NEIGHBORS);
//
//             // Optional: Reduce stability based on tile's own properties (e.g. hardness, intrinsic brittleness)
//             // stability -= ecsWorld[x,y].hardness * 0.05f; 
//
//             return Mathf.Clamp(stability, 0f, 100f);
//         }
//     }
// }