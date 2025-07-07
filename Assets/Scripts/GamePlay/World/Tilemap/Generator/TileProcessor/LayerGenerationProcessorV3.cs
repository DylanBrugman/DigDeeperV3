// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using GamePlay.World.Tilemap.Generator;
// using GamePlay.World.Tilemap.Generator.TileProcessor;
// using NUnit.Framework;
// using Systems.WorldSystem.Generator.TileProcessor;
// using UnityEngine;
// namespace Systems.WorldSystem.Generator
// {
//     // Make sure this class inherits from ITilemapTileProcessor
//     public class LayerGenerationProcessorV3 : ITilemapTileProcessor
//     {
//         public string StepName => "Generating Earth Layers";
//
//         private WorldGenerationConfig config;
//         private System.Random random;
//         private int[] surfaceHeights; // We'll need this from context
//
//         public void Initialize(TilemapGeneratorContext context)
//         {
//             config = context.Config;
//             random = context.Random;
//             surfaceHeights = context.SurfaceHeights; // Get surface heights
//         }
//
//         public void Process(Tile tile, Tile[,] currentWorldState, TilemapGeneratorContext context)
//         {
//             // Skip processing for air tiles or tiles above the surface (which should be air/grass handled elsewhere)
//             if (tile.position.y > surfaceHeights[tile.position.x])
//             {
//                 // This tile is above the surface, so it's air.
//                 // We'll let a separate processor (e.g., BaseTerrainTilemapTileProcessor) handle surface layers.
//                 return;
//             } // Store depth in metadata for other processors
//
//             EarthLayer appliedLayer = GetLayerForDepth(tile.currentDepth, tile.position.x, tile.position.y);
//
//             if (appliedLayer != null)
//             {
//                 // Apply primary or secondary tile type based on secondary material flow
//                 bool useSecondary = false;
//                 if (appliedLayer.secondaryTileTypes != null && appliedLayer.secondaryTileTypes.Count > 0 && appliedLayer.secondaryMaterialFlow > 0f)
//                 {
//                     float secondaryNoise = Mathf.PerlinNoise(
//                         tile.position.x * appliedLayer.secondaryFlowScale + config.seed * 2f,
//                         tile.position.y * appliedLayer.secondaryFlowScale + config.seed * 2f
//                     );
//                     if (secondaryNoise < appliedLayer.secondaryMaterialFlow)
//                     {
//                         useSecondary = true;
//                     }
//                 }
//
//                 if (useSecondary)
//                 {
//                     // Select a random secondary tile type
//                     tile.tileType = appliedLayer.secondaryTileTypes[random.Next(appliedLayer.secondaryTileTypes.Count)];
//                 }
//                 else
//                 {
//                     tile.tileType = appliedLayer.primaryTileType;
//                 }
//
//                 tile.temperature = appliedLayer.temperature;
//                 tile.pressure = appliedLayer.pressure;
//                 // Hardness and stability can be derived from tileType or configured per layer
//                 // For now, let's just set them to default based on tile type for demonstration
//                 tile.hardness = GetDefaultHardness(tile.tileType);
//                 tile.stability = GetDefaultStability(tile.tileType);
//             }
//             else
//             {
//                 // Fallback for tiles not covered by any layer (e.g., if layers don't cover full depth)
//                 // This might indicate an issue with layer configuration or if the tile is above the configured layers.
//                 // For now, let's default to Stone if it's underground and not assigned.
//                 if (tile.position.y <= surfaceHeights[tile.position.x] && tile.tileType == TileType.None)
//                 {
//                     tile.tileType = TileType.Stone;
//                     tile.hardness = GetDefaultHardness(TileType.Stone);
//                     tile.stability = GetDefaultStability(TileType.Stone);
//                     tile.temperature = 20f;
//                     tile.pressure = 1f;
//                 }
//             }
//         }
//
//         private EarthLayer GetLayerForDepth(int depth, int x, int y)
//         {
//             EarthLayer bestLayer = null;
//             float highestInfluence = 0f;
//
//             foreach (var layer in config.earthLayers.OrderByDescending(l => l.startDepth)) // Process deeper layers first if there are overlaps
//             {
//                 // Check if depth is within the layer's primary range
//                 if (depth >= layer.startDepth && depth <= layer.endDepth)
//                 {
//                     float layerInfluence = 1f; // Base influence
//
//                     // Apply flow intensity (Perlin noise for wavy layers)
//                     if (layer.flowIntensity > 0f)
//                     {
//                         float flowNoise = Mathf.PerlinNoise(
//                             x * layer.flowScale + config.seed, // X-coordinate influence
//                             y * layer.flowScale + config.seed // Y-coordinate influence
//                         );
//                         // Adjust depth based on noise, and then calculate influence within the layer
//                         float adjustedDepth = depth + (flowNoise - 0.5f) * layer.flowOffset;
//
//                         if (adjustedDepth < layer.startDepth || adjustedDepth > layer.endDepth)
//                         {
//                             layerInfluence = 0f; // Outside effective range due to flow
//                         }
//                         else
//                         {
//                             // Calculate strength within the adjusted layer bounds
//                             float layerMid = (layer.startDepth + layer.endDepth) / 2f;
//                             float layerRange = Mathf.Max(0.001f, layer.endDepth - layer.startDepth);
//                             float distanceFromMid = Mathf.Abs(adjustedDepth - layerMid);
//                             float flowStrength = 1f - (distanceFromMid / (layerRange / 2f));
//                             layerInfluence = Mathf.Lerp(1f, flowStrength, layer.flowIntensity);
//                         }
//                     }
//
//                     // Apply Voronoi influence for more cellular/blobby structures
//                     if (layer.useVoronoi)
//                     {
//                         // You'll need a Voronoi generator. Let's assume you have one:
//                         // float voronoiValue = VoronoiGenerator.GetVoronoiNoise(new Vector2(x, y), context.LayerVoronoiPoints[layer], config.worldSize.x);
//                         // layerInfluence *= Mathf.Lerp(1f, voronoiValue, layer.voronoiInfluence);
//                         // For now, let's skip Voronoi if the helper isn't provided
//                         Debug.LogWarning("Voronoi generation not implemented in LayerGenerationProcessorV2 for this example.");
//                     }
//
//                     if (layerInfluence > highestInfluence)
//                     {
//                         highestInfluence = layerInfluence;
//                         bestLayer = layer;
//                     }
//                 }
//             }
//             return bestLayer;
//         }
//
//         // Helper to get default hardness based on TileType
//         private float GetDefaultHardness(TileType type)
//         {
//             switch (type)
//             {
//                 case TileType.Air: return 0f;
//                 case TileType.Grass: return 10f;
//                 case TileType.Dirt: return 20f;
//                 case TileType.Clay: return 30f;
//                 case TileType.Sand: return 15f;
//                 case TileType.Stone: return 50f;
//                 case TileType.Limestone: return 45f;
//                 case TileType.Granite: return 70f;
//                 case TileType.Marble: return 60f;
//                 case TileType.Bedrock: return 100f;
//                 case TileType.Lava: return 0f; // Not hard, but impassable
//                 case TileType.Water: return 0f;
//                 case TileType.Ice: return 25f;
//                 default: return 50f;
//             }
//         }
//
//         // Helper to get default stability based on TileType
//         private float GetDefaultStability(TileType type)
//         {
//             switch (type)
//             {
//                 case TileType.Air: return 0f;
//                 case TileType.Grass: return 80f;
//                 case TileType.Dirt: return 70f;
//                 case TileType.Clay: return 75f;
//                 case TileType.Sand: return 60f;
//                 case TileType.Stone: return 90f;
//                 case TileType.Limestone: return 85f;
//                 case TileType.Granite: return 95f;
//                 case TileType.Marble: return 90f;
//                 case TileType.Bedrock: return 100f;
//                 case TileType.Lava: return 10f; // Destroys things, but exists
//                 case TileType.Water: return 0f;
//                 case TileType.Ice: return 50f;
//                 default: return 80f;
//             }
//         }
//     }
// }