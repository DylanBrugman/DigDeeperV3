// using System.Collections.Generic;
// using System.Linq;
// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using GamePlay.World.Tilemap.Generator;
// using GamePlay.World.Tilemap.Generator.TileProcessor;
// using Systems.WorldSystem.Generator;
// using Systems.WorldSystem.Generator.TileProcessor;
// using UnityEngine;
//
// namespace Systems.WorldSystem {
//     public class BiomeAssignmentTilemapTileProcessor : ITilemapTileProcessor
//     {
//         public string StepName => "Assigning Biomes (Tile)";
//
//         public void Initialize(TilemapGeneratorContext context) {
//         }
//
//         public void Process(Tile tile, Tile[,] currentWorldState, TilemapGeneratorContext context)
//         {
//             if (context.Config.depthLimitedBiomes == null || context.Config.depthLimitedBiomes.Count == 0)
//             {
//                 tile.biome = BiomeType.Temperate; // Default fallback
//                 return;
//             }
//
//             var biomeConfig = GetDepthAppropriateBiome(tile.position.x, tile.currentDepth, context);
//
//             if (biomeConfig != null)
//             {
//                 tile.biome = biomeConfig.biomeType;
//                 ApplyBiomeEffects(tile, biomeConfig, context, tile.position.x);
//             }
//             else
//             {
//                 tile.biome = BiomeType.Temperate; // Default fallback
//             }
//         }
//
//         private BiomeConfig GetDepthAppropriateBiome(int x, float depth, TilemapGeneratorContext context)
//         {
//             var validBiomes = new List<(BiomeConfig biome, float strength)>();
//
//             foreach (var biome in context.Config.depthLimitedBiomes)
//             {
//                 if (!context.BiomeNoiseOffsets.ContainsKey(biome)) continue; // Safety check
//
//                 float depthStrength = 1f;
//                 if (biome.limitByDepth)
//                 {
//                     if (depth < biome.minDepth || depth > biome.maxDepth)
//                     {
//                         float falloffDistance = (depth < biome.minDepth) ? (biome.minDepth - depth) : (depth - biome.maxDepth);
//                         depthStrength = Mathf.Clamp01(1f - (falloffDistance * biome.depthFalloff));
//                         if (depthStrength <= 0.001f) continue;
//                     }
//                 }
//
//                 float biomeNoise = Mathf.PerlinNoise(
//                     x * context.Config.biomeScale + context.BiomeNoiseOffsets[biome],
//                     context.Config.seed * 0.05f // Consistent y-offset for 1D-like Perlin bands
//                 );
//
//                 float totalStrength = biomeNoise * depthStrength;
//                 validBiomes.Add((biome, totalStrength));
//             }
//
//             if (validBiomes.Count == 0)
//             {
//                 // Fallback: could be the first biome, a specific default, or null
//                 return context.Config.depthLimitedBiomes.FirstOrDefault(); 
//             }
//
//             return validBiomes.OrderByDescending(b => b.strength).First().biome;
//         }
//
//         private void ApplyBiomeEffects(Tile tile, BiomeConfig biomeConfig, TilemapGeneratorContext context, int x)
//         {
//             tile.temperature += biomeConfig.temperatureModifier;
//             // Lerp current tile color (could be from layer) with biome color
//             tile.TileColor = Color.Lerp(tile.TileColor, biomeConfig.biomeColor, 0.3f); 
//
//             if (tile.currentDepth == 0 && tile.tileType == TileType.Grass && // Only change surface grass
//                 biomeConfig.preferredSurfaceTiles != null && biomeConfig.preferredSurfaceTiles.Count > 0)
//             {
//                 var localRandom = new System.Random(context.Config.seed + tile.position.x * context.WorldSizeChunks.y + tile.position.y + biomeConfig.biomeType.GetHashCode());
//                 if (localRandom.NextDouble() < 0.5) // 50% chance to apply preferred surface tile
//                 {
//                     tile.tileType = biomeConfig.preferredSurfaceTiles[localRandom.Next(biomeConfig.preferredSurfaceTiles.Count)];
//                 }
//             }
//         }
//     }
// }