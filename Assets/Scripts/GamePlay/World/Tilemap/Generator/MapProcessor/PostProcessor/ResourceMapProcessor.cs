// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using GamePlay.World.Tilemap.Generator;
// using Systems.WorldSystem.Generator;
// using Systems.WorldSystem.Generator.MapProcessor;
// using UnityEngine;
//
// namespace Systems.WorldSystem {
//     public class ResourceMapProcessor : IMapProcessor
//     {
//         public string StepName => "Generating Resources (Map Pass)";
//
//         public void Process(Tile[,] world, TilemapGeneratorContext context)
//         {
//             var config = context.Config;
//             if (!config.generateResources) return;
//
//             // Apply global resource rules
//             if (config.globalResourceRules != null)
//             {
//                 foreach (var rule in config.globalResourceRules)
//                 {
//                     ApplyGlobalResourceRule(world, rule, context);
//                 }
//             }
//             
//             // // Apply layer-specific resource rules using earthLayers for consistency
//             // if (config.flowingEarthLayers != null)
//             // {
//             //     foreach (var layer in config.flowingEarthLayers) // Changed from config.earthLayers
//             //     {
//             //         if (layer.resourceRules == null) continue;
//             //         foreach (var rule in layer.resourceRules)
//             //         {
//             //             ApplyResourceRuleToLayer(ecsWorld, rule, layer, context);
//             //         }
//             //     }
//             // }
//
//             // Apply biome-specific resource rules using depthLimitedBiomes for consistency
//             if (config.depthLimitedBiomes != null)
//             {
//                 foreach (var biomeConfig in config.depthLimitedBiomes) // Changed from config.biomeConfigs
//                 {
//                     if (biomeConfig.biomeSpecificResources == null) continue;
//                     foreach (var rule in biomeConfig.biomeSpecificResources)
//                     {
//                         ApplyResourceRuleToBiome(world, rule, biomeConfig.biomeType, context);
//                     }
//                 }
//             }
//         }
//
//         private bool ShouldPlaceResource(Tile tile, ResourceSpawnRule rule)
//         {
//             if (tile.tileType == TileType.Air) return false;
//             if (rule.requiresSpecificTileType && tile.tileType != rule.requiredTileType) return false;
//             // AddComponent more conditions: e.g., not in a POI tile already rich, depth constraints for the rule itself
//             return true;
//         }
//
//         private void GenerateResourceDeposit(Tile tile, ResourceSpawnRule rule, TilemapGeneratorContext context)
//         {
//             float abundance = Mathf.Lerp(rule.minAbundance, rule.maxAbundance, (float)context.Random.NextDouble());
//             abundance *= context.Config.globalResourceMultiplier;
//
//             float quality = rule.baseQuality + ((float)context.Random.NextDouble() - 0.5f) * 30f; // +/- 15
//             quality = Mathf.Clamp(quality, 1f, 100f);
//
//             float accessibility = 100f - tile.hardness * 0.5f + ((float)context.Random.NextDouble() - 0.5f) * 10f;
//             accessibility = Mathf.Clamp(accessibility, 0f, 100f);
//
//             tile.AddResource(new ResourceDeposit(rule.resourceType, abundance, quality, accessibility));
//         }
//
//         private void ApplyGlobalResourceRule(Tile[,] world, ResourceSpawnRule rule, TilemapGeneratorContext context)
//         {
//             int numClusters = Mathf.RoundToInt(context.Config.worldSize.x * context.Config.worldSize.y * rule.spawnChance * 0.0001f);
//
//             for (int i = 0; i < numClusters; i++)
//             {
//                 var center = new Vector2Int(
//                     context.Random.Next(0, context.Config.worldSize.x),
//                     context.Random.Next(0, context.Config.worldSize.y)
//                 );
//
//                 if (context.IsValidPosition(center) && ShouldPlaceResource(world[center.x, center.y], rule))
//                 {
//                     GenerateResourceCluster(world, center, rule, context);
//                 }
//             }
//         }
//
//         private void GenerateResourceCluster(Tile[,] world, Vector2Int center, ResourceSpawnRule rule, TilemapGeneratorContext context)
//         {
//             int clusterSize = context.Random.Next(Mathf.RoundToInt(rule.clusterSize.x), Mathf.RoundToInt(rule.clusterSize.y) + 1);
//             for (int i = 0; i < clusterSize; i++)
//             {
//                 int offsetX = context.Random.Next(-3, 4);
//                 int offsetY = context.Random.Next(-3, 4);
//                 var position = center + new Vector2Int(offsetX, offsetY);
//
//                 if (context.IsValidPosition(position))
//                 {
//                     var tile = world[position.x, position.y];
//                     if (ShouldPlaceResource(tile, rule) && (float)context.Random.NextDouble() < 0.7f) 
//                     {
//                         GenerateResourceDeposit(tile, rule, context);
//                     }
//                 }
//             }
//         }
//
//         private void ApplyResourceRuleToLayer(Tile[,] world, ResourceSpawnRule rule, EarthLayer layer, TilemapGeneratorContext context)
//         {
//             for (int x = 0; x < context.Config.worldSize.x; x++)
//             {
//                 for (int y = 0; y < context.Config.worldSize.y; y++)
//                 {
//                     var tile = world[x, y];
//                     // Check if the tile primarily belongs to this layer's depth.
//                     // This is a simplified check; a more accurate way would be to store the dominant layer on the tile.
//                     if (tile.currentDepth >= layer.startDepth && tile.currentDepth <= layer.endDepth)
//                     {
//                         if ((float)context.Random.NextDouble() < rule.spawnChance && ShouldPlaceResource(tile, rule))
//                         {
//                             GenerateResourceDeposit(tile, rule, context);
//                         }
//                     }
//                 }
//             }
//         }
//
//         private void ApplyResourceRuleToBiome(Tile[,] world, ResourceSpawnRule rule, BiomeType biomeType, TilemapGeneratorContext context)
//         {
//             for (int x = 0; x < context.Config.worldSize.x; x++)
//             {
//                 for (int y = 0; y < context.Config.worldSize.y; y++)
//                 {
//                     var tile = world[x, y];
//                     if (tile.biome == biomeType)
//                     {
//                         if ((float)context.Random.NextDouble() < rule.spawnChance && ShouldPlaceResource(tile, rule))
//                         {
//                             GenerateResourceDeposit(tile, rule, context);
//                         }
//                     }
//                 }
//             }
//         }
//     }
// }