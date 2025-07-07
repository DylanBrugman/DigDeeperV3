// using System.Collections.Generic;
// using System.Linq;
// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using GamePlay.World.Tilemap.Generator;
// using GamePlay.World.Tilemap.Generator.TileProcessor; // Assuming this is your namespace for Tile, etc.
// using UnityEngine;
//
// namespace Systems.WorldSystem.Generator.TileProcessor
// {
//     public class LayerGenerationProcessorV4 : ITilemapTileProcessor
//     {
//         public string StepName => "Generating Earth Layers with Overlaps";
//
//         private WorldGenerationConfig config;
//         private int[] surfaceHeights;
//
//         // Cache for pre-calculated upper boundary of each layer for each x-column.
//         // The int[] stores the depth at which the layer starts for that column.
//         private Dictionary<EarthLayer, int[]> layerUpperBoundaries;
//
//         public void Initialize(TilemapGeneratorContext context)
//         {
//             config = context.Config;
//             surfaceHeights = context.SurfaceHeights;
//
//             GenerateUpperBoundaryLayers();
//         }
//
//         /// <summary>
//         /// Pre-calculates the wavy top boundary for each earth layer across the ecsWorld's width.
//         /// This is much more performant than calculating noise for every tile.
//         /// </summary>
//         private void GenerateUpperBoundaryLayers()
//         {
//             layerUpperBoundaries = new Dictionary<EarthLayer, int[]>();
//
//             // Iterate through each configured layer to generate its boundary map.
//             foreach (var layer in config.earthLayers)
//             {
//                 // The boundary array must be sized by the ecsWorld's width (x-axis).
//                 int[] boundaries = new int[config.worldSize.x];
//                 
//                 // *** THE KEY CHANGE IS HERE ***
//                 // We create a unique 'y' coordinate for each layer in the Perlin noise space.
//                 // This ensures each layer samples a different "slice" of the noise, giving it a unique shape.
//                 // Using a large or non-integer InitialValue helps ensure the samples are far apart and uncorrelated.
//                 float layerNoiseY = (config.seed + layer.GetHashCode()) * 0.1f;
//
//                 for (int x = 0; x < config.worldSize.x; x++)
//                 {
//                     // The first coordinate depends on the horizontal position and desired scale.
//                     float noiseX = x * layer.flowScale;
//                     
//                     // Sample the 2D noise field using the unique y-coordinate for this layer.
//                     float noise = Mathf.PerlinNoise(noiseX, layerNoiseY); // noise is in range [0, 1]
//
//                     // The rest of the calculation remains the same.
//                     int boundaryDepth = Mathf.FloorToInt(layer.startDepth + (noise * (layer.endDepth - layer.startDepth)));
//                     
//                     boundaries[x] = boundaryDepth;
//                 }
//                 
//                 layerUpperBoundaries[layer] = boundaries;
//             }
//         }
//
//         public void Process(Tile tile, Tile[,] currentWorldState, TilemapGeneratorContext context)
//         {
//             // NativeTileGrid above the surface are set to Air.
//             if (tile.position.y > surfaceHeights[tile.position.x])
//             {
//                 tile.tileType = TileType.Air;
//                 return;
//             }
//
//             // Calculate the tile's depth relative to the surface.
//             int realDepth = surfaceHeights[tile.position.x] - tile.position.y;
//             if (realDepth < 0) realDepth = 0; // Should not happen with the check above, but good for safety.
//
//             // Find the correct layer for this tile based on its position and depth.
//             EarthLayer winningLayer = DetermineWinningLayer(tile.position.x, realDepth);
//
//             if (winningLayer != null)
//             {
//                 tile.tileType = winningLayer.primaryTileType;
//                 tile.temperature = winningLayer.temperature;
//                 tile.pressure = winningLayer.pressure;
//                 tile.hardness = GetDefaultHardness(tile.tileType);
//                 tile.stability = GetDefaultStability(tile.tileType);
//             }
//             else
//             {
//                 // Fallback for areas below the surface not covered by any defined layer.
//                 // This prevents holes in the ecsWorld. You can change TileType.Stone to any default.
//                 tile.tileType = TileType.Stone; 
//                 tile.hardness = GetDefaultHardness(TileType.Stone);
//                 tile.stability = GetDefaultStability(TileType.Stone);
//                 tile.temperature = 20f; // Example default
//                 tile.pressure = 1f;     // Example default
//             }
//         }
//
//         /// <summary>
//         /// Determines the winning layer by checking from the deepest layer upwards.
//         /// </summary>
//         private EarthLayer DetermineWinningLayer(int x, int realDepth)
//         {
//             // Iterate through layers sorted by depth, from deepest to shallowest.
//             // This ensures that deeper layers correctly override any shallower ones.
//             foreach (var layer in config.earthLayers.OrderByDescending(l => l.startDepth))
//             {
//                 // Check if the tile's depth is at or below this layer's starting boundary.
//                 if (layerUpperBoundaries.TryGetValue(layer, out int[] boundaries) && realDepth >= boundaries[x])
//                 {
//                     // Since we are iterating from deepest to shallowest, the first match is the correct one.
//                     return layer;
//                 }
//             }
//
//             // No layer was found for this depth.
//             return null;
//         }
//
//         // --- Helper Methods ---
//
//         private float GetDefaultHardness(TileType type) 
//         { 
//             switch (type)
//             {
//                 case TileType.Stone: return 80f;
//                 case TileType.Dirt: return 30f;
//                 case TileType.Sand: return 20f;
//                 case TileType.Clay: return 40f;
//                 default: return 50f;
//             }
//         }
//
//         private float GetDefaultStability(TileType type) 
//         { 
//             switch (type)
//             {
//                 case TileType.Stone: return 100f;
//                 case TileType.Dirt: return 70f;
//                 case TileType.Sand: return 40f;
//                 case TileType.Clay: return 60f;
//                 default: return 80f;
//             }
//         }
//     }
// }