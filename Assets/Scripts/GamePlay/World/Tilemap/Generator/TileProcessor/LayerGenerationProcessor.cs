// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using GamePlay.World.Tilemap.Generator;
// using GamePlay.World.Tilemap.Generator.TileProcessor;
// using NUnit.Framework;
// using UnityEngine;
//
// namespace Systems.WorldSystem.Generator.TileProcessor {
//     public class LayerGenerationProcessor : ITilemapTileProcessor
//     {
//         private Dictionary<EarthLayer, List<Vector2>> layerVoronoiPoints = new();
//         
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
//             // Get base noise for this position
//             float layerNoise = Mathf.PerlinNoise(
//                 tile.position.x * context.Config.layerScale + context.Config.seed,
//                 tile.position.y * context.Config.layerScale + context.Config.seed
//             ) - 0.5f; // Center around 0 (-0.5 to +0.5)
//     
//             // Adjust the effective depth based on noise
//             float adjustedDepth = tile.currentDepth + layerNoise * 8;
//     
//             // Find the primary layer for this adjusted depth
//             var primaryLayer = GetPrimaryLayerForDepth(adjustedDepth, context);
//     
//             if (primaryLayer != null) {
//                 ApplyTileProperties(tile, primaryLayer);
//             }
//         }
//
//         private EarthLayer GetPrimaryLayerForDepth(float depth, TilemapGeneratorContext context)
//         {
//             // Sort layers by start depth (like geological stratification)
//             var sortedLayers = context.Config.earthLayers
//                 .OrderBy(l => l.startDepth)
//                 .ToList();
//     
//             // Find the appropriate layer for this depth
//             foreach (var layer in sortedLayers) {
//                 if (depth >= layer.startDepth && depth <= layer.endDepth) {
//                     return layer;
//                 }
//             }
//     
//             // Fallback to deepest layer if we're below all defined layers
//             return sortedLayers.LastOrDefault();
//         }
//
//         // public void Process(Tile tile, Tile[,] currentWorldState, TilemapGeneratorContext context)
//         // {
//         //     var layers = GetPossibleEarthLayers(tile.Depth, context);
//         //
//         //     if (layers.Count == 0) {
//         //         throw new ArgumentException($"No layers found for depth {tile.Depth}");
//         //     }
//         //
//         //     // If only one layer, use it
//         //     if (layers.Count == 1) {
//         //         ApplyTileProperties(tile, layers[0]);
//         //         return;
//         //     }
//         //
//         //     // Use consistent seed for spatial coherence
//         //     float noise = Mathf.PerlinNoise(
//         //         tile.position.x * context.Config.layerScale + context.Config.seed,
//         //         tile.position.y * context.Config.layerScale + context.Config.seed
//         //     );
//         //
//         //     // Proper index calculation with bounds checking
//         //     int layerIndex = Mathf.FloorToInt(noise * layers.Count);
//         //     layerIndex = Mathf.Clamp(layerIndex, 0, layers.Count - 1);
//         //
//         //     EarthLayer layer = layers[layerIndex];
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
//         private List<EarthLayer> GetPossibleEarthLayers(int tileDepth, TilemapGeneratorContext context) {
//             List<EarthLayer> possibleLayers = context.Config.earthLayers
//                 .Where(layer => layer.startDepth <= tileDepth && layer.endDepth >= tileDepth)
//                 .OrderBy(layer => layer.startDepth)
//                 .ToList();
//             if (possibleLayers.Count == 0) {
//                 throw new ArgumentException( 
//                     $"No EarthLayer found for depth {tileDepth}. Ensure layers are defined correctly in the configuration."
//                 );
//             }
//
//             return possibleLayers;
//         }
//
//         // private EarthLayer GetFlowingEarthLayer(int x, int y, float depth, TilemapGeneratorContext context)
//         // {
//         //     EarthLayer bestLayer = null;
//         //     float bestLayerStrength = 0f;
//         //
//         //     if (context.Config.earthLayers == null) return null;
//         //
//         //     foreach (var layer in context.Config.earthLayers)
//         //     {
//         //         float layerStrength = CalculateLayerStrength(layer, x, y, depth, context);
//         //         if (layerStrength > bestLayerStrength)
//         //         {
//         //             bestLayerStrength = layerStrength;
//         //             bestLayer = layer;
//         //         }
//         //     }
//         //     return bestLayer;
//         // }
//         //
//         // private float CalculateLayerStrength(EarthLayer layer, int x, int y, float depth, TilemapGeneratorContext context)
//         // {
//         //     float depthStrength = 0f;
//         //     if (depth >= layer.startDepth && depth <= layer.endDepth)
//         //     {
//         //         float layerMid = (layer.startDepth + layer.endDepth) / 2f;
//         //         float layerRange = Mathf.MaxVelocity(0.001f, layer.endDepth - layer.startDepth);
//         //         float distanceFromMid = Mathf.Abs(depth - layerMid);
//         //         depthStrength = 1f - (distanceFromMid / (layerRange / 2f));
//         //         depthStrength = Mathf.Clamp01(depthStrength);
//         //     }
//         //
//         //     if (depthStrength <= 0f) return 0f;
//         //
//         //     float flowModifier = 1f;
//         //     if (layer.flowIntensity > 0f)
//         //     {
//         //         float flowNoise = Mathf.PerlinNoise(
//         //             x * layer.flowScale + context.Config.seed,
//         //             y * layer.flowScale + context.Config.seed
//         //         );
//         //         float flowAdjustedDepth = depth + (flowNoise - 0.5f) * layer.flowOffset;
//         //
//         //         if (flowAdjustedDepth >= layer.startDepth && flowAdjustedDepth <= layer.endDepth)
//         //         {
//         //             float flowLayerMid = (layer.startDepth + layer.endDepth) / 2f;
//         //             float flowLayerRange = Mathf.MaxVelocity(0.001f, layer.endDepth - layer.startDepth);
//         //             float flowDistanceFromMid = Mathf.Abs(flowAdjustedDepth - flowLayerMid);
//         //             float flowStrength = 1f - (flowDistanceFromMid / (flowLayerRange / 2f));
//         //             flowStrength = Mathf.Clamp01(flowStrength);
//         //             flowModifier = Mathf.Lerp(1f, flowStrength, layer.flowIntensity);
//         //         }
//         //         else
//         //         {
//         //             flowModifier = 0f;
//         //         }
//         //     }
//         //
//         //     if (layer.useVoronoi && context.LayerVoronoiPoints.TryGetValue(layer, out var voronoiPoints))
//         //     {
//         //         float voronoiValue = VoronoiGenerator.GetVoronoiNoise(
//         //             new Vector2(x, y),
//         //             voronoiPoints,
//         //             Mathf.MaxVelocity(context.Config.worldSize.x, context.Config.worldSize.y)
//         //         );
//         //         flowModifier *= Mathf.Lerp(1f, voronoiValue, layer.voronoiInfluence);
//         //     }
//         //     return depthStrength * flowModifier;
//         // }
//         //
//         private void ApplyFlowingLayerProperties(Tile tile, EarthLayer layer, int x, int y, TilemapGeneratorContext context)
//         {
//             // Preserve surface grass, allow other types to be changed by layers.
//             // This logic might need refinement based on desired interaction between base terrain and layers.
//             if (tile.tileType != TileType.Grass || tile.currentDepth > 0) 
//             {
//                 bool useSecondary = false;
//                 if (layer.secondaryTileTypes != null && layer.secondaryTileTypes.Count > 0 && layer.secondaryMaterialFlow > 0f)
//                 {
//                     float secondaryNoise = Mathf.PerlinNoise(
//                         x * layer.secondaryFlowScale + context.Config.seed * 2,
//                         y * layer.secondaryFlowScale + context.Config.seed * 2
//                     );
//                     useSecondary = secondaryNoise < layer.secondaryMaterialFlow;
//                 }
//         
//                 if (useSecondary)
//                 {
//                     // Using a deterministic random choice based on coordinates and layer if context.Random isn't desired here
//                     var localRandom = new System.Random(context.Config.seed + x * context.WorldSizeChunks.y + y + layer.layerName.GetHashCode());
//                     tile.tileType = layer.secondaryTileTypes[localRandom.Next(layer.secondaryTileTypes.Count)];
//                 }
//                 else
//                 {
//                     tile.tileType = layer.primaryTileType;
//                 }
//             }
//         
//             tile.temperature = layer.temperature; // Base temperature from layer
//             tile.pressure = layer.pressure;     // Base pressure from layer
//             tile.hardness = context.CalculateHardness(tile.tileType, tile.currentDepth); // Hardness based on new type and depth
//         }
//     }
// }