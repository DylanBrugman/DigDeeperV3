// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using GamePlay.World.Tilemap.Generator.TileProcessor;
// using NUnit.Framework;
// using UnityEngine;
//
// namespace Systems.WorldSystem.Generator.TileProcessor {
//     public class LayerGenerationProcessorV2 : ITilemapTileProcessor
//     {
//         public string StepName => "Applying Earth Layers (Tile)";
//         
//         // private void InitializeLayerVoronoiPoints(TilemapGeneratorContext context) {
//         //     if (context.Config.earthLayers != null)
//         //     {
//         //         foreach (var layer in context.Config.earthLayers)
//         //         {
//         //             if (layer.useVoronoi)
//         //             {
//         //                 var points = VoronoiGenerator.GenerateVoronoiPoints(
//         //                     layer.voronoiPoints,
//         //                     new Vector2(context.Config.worldSize.x, context.Config.worldSize.y),
//         //                     context.Config.seed + layer.layerName.GetHashCode()
//         //                 );
//         //                 layerVoronoiPoints[layer] = points;
//         //             }
//         //         }
//         //     }
//         // }
//
//         public void Initialize(TilemapGeneratorContext context) {
//             // InitializeLayerVoronoiPoints(context);
//         }
//
//         public void Process(Tile tile, Tile[,] currentWorldState, TilemapGeneratorContext context)
//         {
//             // Evaluate layers from top to bottom (surface to deep)
//             var sortedLayers = context.Config.earthLayers
//                 .OrderBy(layer => layer.startDepth)
//                 .ToList();
//     
//             foreach (var layer in sortedLayers)
//             {
//                 if (DoesLayerExistAt(layer, tile.position.x, tile.currentDepth, context))
//                 {
//                     // First layer that exists wins
//                     ApplyTileProperties(tile, layer);
//                     return;
//                 }
//             }
//     
//             // Fallback to deepest layer
//             var fallbackLayer = sortedLayers.LastOrDefault();
//             ApplyTileProperties(tile, fallbackLayer);
//         }
//
//         private bool DoesLayerExistAt(EarthLayer layer, int x, int depth, TilemapGeneratorContext context)
//         {
//             // Check base depth range
//             if (depth < layer.startDepth || depth > layer.endDepth) return false;
//     
//             // Calculate layer-specific noise
//             float layerNoise = Mathf.PerlinNoise(
//                 (x + context.Config.seed + layer.layerName.GetHashCode()) * layer.flowIntensity,
//                 (depth + context.Config.seed + layer.layerName.GetHashCode()) * layer.flowIntensity
//             );
//     
//             // Apply depth bias (layers more likely to exist near their center depth)
//             float depthBias = CalculateDepthBias(layer, depth);
//     
//             return (layerNoise + depthBias) > context.Random.Next(0, 10) / 10f;
//         }
//         
//         private float CalculateDepthBias(EarthLayer layer, int depth)
//         {
//             // Calculate how close we are to the layer's preferred depth range
//             float layerCenter = (layer.startDepth + layer.endDepth) / 2f;
//             float layerRange = Mathf.Max(1f, layer.endDepth - layer.startDepth);
//             float distanceFromCenter = Mathf.Abs(depth - layerCenter);
//     
//             // Normalized distance (0 = center, 1 = edge)
//             float normalizedDistance = distanceFromCenter / (layerRange / 2f);
//     
//             // Convert to bias (1.0 at center, 0.0 at edges, can go negative beyond edges)
//             float depthBias = 1f - normalizedDistance;
//     
//             // Scale the bias influence (adjust this InitialValue to control how much depth matters)
//             return depthBias * 0.3f; // 0.3 is the bias strength
//         }
//         
//         // public void Process(Tile tile, Tile[,] currentWorldState, TilemapGeneratorContext context) {
//         //     if (tile.Depth < 5) {
//         //         var sky = new EarthLayer() {
//         //             layerName = "Sky",
//         //             primaryTileType = TileType.Air,
//         //             temperature = 23f,
//         //             pressure = 1f,
//         //             startDepth = 0,
//         //             endDepth = 4,
//         //             flowIntensity = 0.1f,
//         //             flowScale = 0.02f
//         //         };
//         //         ApplyTileProperties(tile, sky);
//         //     } else if (tile.position.y < 5) {
//         //         var lastLayer = GetPossibleEarthLayer(tile.Depth, context);
//         //         ApplyTileProperties(tile, lastLayer);
//         //     }
//         //     
//         //     int randomOffset = context.Random.Next(-2, 2);
//         //     
//         //     var layer = GetPossibleEarthLayer(tile.Depth + randomOffset, context);
//         //     ApplyTileProperties(tile, layer);
//         // }
//
//         private void ApplyTileProperties(Tile tile, EarthLayer layer) {
//             tile.tileType = layer.primaryTileType;
//             tile.temperature = layer.temperature;
//             tile.pressure = layer.pressure;
//
//             // // Apply additional properties if needed
//             // if (layer.secondaryTileTypes != null && layer.secondaryTileTypes.Count > 0) {
//             //     float secondaryNoise = Mathf.PerlinNoise(
//             //         i * layer.secondaryFlowScale + context.Config.seed * 2,
//             //         i1 * layer.secondaryFlowScale + context.Config.seed * 2
//             //     );
//             //     if (secondaryNoise < layer.secondaryMaterialFlow) {
//             //         tile.tileType = layer.secondaryTileTypes[context.Random.Next(layer.secondaryTileTypes.Count)];
//             //     }
//             // }
//         }
//
//         private EarthLayer GetPossibleEarthLayer(int tileDepth, TilemapGeneratorContext context) {
//             return context.Config.earthLayers
//                 .Where(layer => layer.startDepth <= tileDepth && layer.endDepth >= tileDepth)
//                 .OrderBy(layer => layer.startDepth)
//                 .FirstOrDefault(layer => layer.endDepth >= tileDepth && layer.startDepth <= tileDepth);
//         }
//     }
// }