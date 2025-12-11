// using System;
// using System.Collections;
// using System.Collections.Generic;
// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator.New;
// using GamePlay.Map.Generator.New.Core.WorldGen;
// using GamePlay.World.Tilemap.Generator.TileProcessor;
// using Systems.WorldSystem;
// using Systems.WorldSystem.Generator.MapProcessor;
// using Systems.WorldSystem.Generator.MapProcessor.PostProcessor;
// using Systems.WorldSystem.Generator.TileProcessor;
// using Unity.Mathematics;
// using UnityEngine;
// using Random = System.Random;
//
// namespace GamePlay.World.Tilemap.Generator
// {
//     public class TilemapWorldGenerator : IWorldGenerationStep {
//         
//         private WorldGenerationConfig _config;
//         private TilemapGeneratorContext _context;
//         private Dictionary<BiomeConfig, float> _biomeNoiseOffsets;
//         private readonly List<ITilemapTileProcessor> _tileProcessingSteps = new();
//         private readonly List<IMapProcessor> _mapProcessingSteps = new();
//
//         public string StepName => "Tilemap Generation";
//
//         public TilemapWorldGenerator()
//         {
//             // tileProcessingSteps.AddComponent(new BaseTerrainTilemapTileProcessor());
//             _tileProcessingSteps.Add(new LayerGenerationProcessorV4());
//             // tileProcessingSteps.AddComponent(new BiomeAssignmentTilemapTileProcessor());
//
//             _mapProcessingSteps.Add(new CaveMapProcessor());
//             _mapProcessingSteps.Add(new PhysicalPropertiesMapProcessor());
//         }
//
//         public IEnumerator GenerateStep(WorldGenerationContext worldGenerationContext, Action<float> onProgress) {
//             yield return GenerateNewTilemapWorld(worldGenerationContext);
//             onProgress?.Invoke(1f);
//         }
//
//         public IEnumerator GenerateNewTilemapWorld(WorldGenerationContext worldGenerationContext) {
//             Random random = worldGenerationContext.Random;
//             TileGrid tileGrid = worldGenerationContext.TileGrid;
//             var allChunks = tileGrid.AllChunks();
//
//             InitializeSharedData(random);
//             _context = new TilemapGeneratorContext(_config, random, _biomeNoiseOffsets);
//             GenerateSurfaceHeights();
//             InitalizeProcessors();
//             
//             Debug.Log("--- Executing Tile Processors ---");
//             ProcessTiles(tileGrid);
//             
//             for (int x = 0; x < allChunks.GetEnumerator(); x++)
//             {
//                 for (int y = _config.worldSize.y - 1; y >= 0; y--)
//                 {
//                     var tile = new Tile(new Vector2Int(x, y), TileType.Air);
//                     world[x, y] = tile;
//                     tile.currentDepth = CalculateDepth(y, _context.SurfaceHeights[x]);
//
//                     foreach (var step in _tileProcessingSteps)
//                     {
//                         step.Process(tile, world, _context);
//                     }
//                 }
//             }
//
//             Debug.Log("--- Executing Map Processors ---");
//             foreach (var step in _mapProcessingSteps)
//             {
//                 Debug.Log($"Executing map step: {step.StepName}...");
//                 step.Process(world, _context);
//             }
//
//             Debug.Log("ECSSystem generation complete!");
//             
//         }
//
//         private void ProcessTiles(TileGrid tileGrid, WorldGenerationContext _context) {
//             foreach ((int2 coord, TileChunk chunk) in tileGrid.AllChunks()) {
//                 Debug.Log($"Processing chunk at {coord}...");
//                 
//                 for (int x = 0; x < chunk.NativeTileGrid.GetLength(0); x++) {
//                     for (int y = 0; y < chunk.NativeTileGrid.GetLength(1); y++) {
//                         ref Tile tile = ref chunk.At(x, y);
//                         tile.CurrentDepth = tileGrid.TileAt(x, y + 1).CurrentDepth;
//                         foreach (var step in _tileProcessingSteps) {
//                             step.Process(ref tile, _context);
//                         }
//
//                         if (tile.Type == TileType.Air) {
//                             tile.CurrentDepth = 0;
//                         }
//                     }
//                 }
//             }
//         }
//
//         private int CalculateDepth(int y, int surfaceHeight)
//         {
//             // Calculate depth based on the surface height
//             return Mathf.Max(0, surfaceHeight - y);
//         }
//
//         private void InitializeSharedData(System.Random random)
//         {
//             // InitializeLayerVoronoiPoints();
//
//             _biomeNoiseOffsets = new Dictionary<BiomeConfig, float>();
//             if (_config.depthLimitedBiomes != null)
//             {
//                 foreach (var biome in _config.depthLimitedBiomes)
//                 {
//                     _biomeNoiseOffsets[biome] = (float)random.NextDouble() * 1000f;
//                 }
//             }
//         }
//
//         private void GenerateSurfaceHeights() {
//             int[] surfaceHeights = new int[_config.worldSize.x];
//             for (int x = 0; x < _config.worldSize.x; x++)
//             {
//                 float heightNoise = Mathf.PerlinNoise(x * _config.terrainScale, _config.seed * 0.01f);
//                 surfaceHeights[x] = Mathf.RoundToInt(_config.worldSize.y - _config.surfaceAirHeightMinimal - heightNoise * _config.terrainAmplitude);
//                 surfaceHeights[x] = Mathf.Clamp(surfaceHeights[x], 0, _config.worldSize.y - 1);
//             }
//             _context.SurfaceHeights = surfaceHeights;
//         }
//
//         private void InitalizeProcessors() {
//             foreach (var step in _tileProcessingSteps)
//             {
//                 step.Initialize(_context);
//             }
//             foreach (var step in _mapProcessingSteps)
//             {
//                 step.Initialize(_context);
//             }
//         }
//     }
//
//     // public class TilemapWorldGenerator
//     // {
//     //     private WorldGenerationConfig config;
//     //     private System.Random random;
//     //     
//     //     private Dictionary<EarthLayer, List<Vector2>> layerVoronoiPoints;
//     //     private Dictionary<BiomeConfig, float> biomeNoiseOffsets;
//     //     // private Dictionary<BiomeType, BiomeConfig> biomeConfigLookup; // If using original BiomeConfigs extensively
//     //
//     //     private List<ITilemapGenerationStep> generationSteps;
//     //     private TilemapGeneratorContext context;
//     //
//     //     public TilemapWorldGenerator(WorldGenerationConfig generationConfig)
//     //     {
//     //         config = generationConfig;
//     //         random = new System.Random(config.seed);
//     //         // biomeConfigLookup = config.biomeConfigs.ToDictionary(b => b.biomeType, b => b); // If needed
//     //
//     //         InitializeSharedData();
//     //         
//     //         context = new TilemapGeneratorContext(config, random, layerVoronoiPoints, biomeNoiseOffsets);
//     //
//     //         generationSteps = new List<ITilemapGenerationStep>
//     //         {
//     //             new BaseTerrainWorldGenerationStep(),
//     //             new EarthLayerApplicationStep(),
//     //             new BiomeWorldGenerationStep(),
//     //             new CaveWorldGenerationStep(),
//     //             new PointOfInterestPlacementStep(),
//     //             new ResourceWorldGenerationStep(),
//     //             new PhysicalPropertiesCalculationStep()
//     //         };
//     //     }
//     //
//     //     private void InitializeSharedData()
//     //     {
//     //         // Initialize Voronoi points for flowing earth layers
//     //         layerVoronoiPoints = new Dictionary<EarthLayer, List<Vector2>>();
//     //         if (config.earthLayers != null)
//     //         {
//     //             foreach (var layer in config.earthLayers)
//     //             {
//     //                 if (layer.useVoronoi)
//     //                 {
//     //                     var points = VoronoiGenerator.GenerateVoronoiPoints(
//     //                         layer.voronoiPoints,
//     //                         new Vector2(config.worldSize.x, config.worldSize.y),
//     //                         config.seed + layer.layerName.GetHashCode() 
//     //                     );
//     //                     layerVoronoiPoints[layer] = points;
//     //                 }
//     //             }
//     //         }
//     //
//     //         // Initialize biome noise offsets for depth-limited biomes
//     //         biomeNoiseOffsets = new Dictionary<BiomeConfig, float>();
//     //         if (config.depthLimitedBiomes != null)
//     //         {
//     //             // Ensure random instance here is consistent if this needs to be deterministic
//     //             // Using the main 'random' instance for this.
//     //             foreach (var biome in config.depthLimitedBiomes)
//     //             {
//     //                 biomeNoiseOffsets[biome] = (float)random.NextDouble() * 1000f;
//     //             }
//     //         }
//     //     }
//     //     
//     //     public Tile[,] GenerateNewTilemapWorld()
//     //     {
//     //         var ecsWorld = new Tile[config.worldSize.x, config.worldSize.y];
//     //         
//     //         foreach (var step in generationSteps)
//     //         {
//     //             Debug.Log($"Executing step: {step.StepName}...");
//     //             step.GenerateStep(ecsWorld, context);
//     //         }
//     //         
//     //         Debug.Log("ECSSystem generation complete!");
//     //         return ecsWorld;
//     //     }
//     // }
//     //
//     // // --- Concrete Generation Steps ---
//     //
//     // public class BaseTerrainWorldGenerationStep : ITilemapGenerationStep
//     // {
//     //     public string StepName => "Generating Base Terrain";
//     //
//     //     public void GenerateStep(Tile[,] ecsWorld, TilemapGeneratorContext context)
//     //     {
//     //         var config = context.Config;
//     //         for (int x = 0; x < config.worldSize.x; x++)
//     //         {
//     //             float heightNoise = Mathf.PerlinNoise(x * config.terrainScale, config.seed * 0.01f);
//     //             int surfaceHeight = Mathf.RoundToInt(config.surfaceAirHeightMinimal + heightNoise * config.terrainAmplitude);
//     //             surfaceHeight = Mathf.Clamp(surfaceHeight, 0, config.worldSize.y - 1);
//     //             
//     //             for (int y = 0; y < config.worldSize.y; y++)
//     //             {
//     //                 var position = new Vector2Int(x, y);
//     //                 TileType tileType;
//     //                 
//     //                 if (y > surfaceHeight) tileType = TileType.Air;
//     //                 else if (y == surfaceHeight) tileType = TileType.Grass;
//     //                 else if (y > surfaceHeight - 5) tileType = TileType.Dirt;
//     //                 else tileType = TileType.Stone;
//     //                 
//     //                 var tile = new Tile(position, tileType);
//     //                 tile.depth = Mathf.MaxVelocity(0, surfaceHeight - y);
//     //                 ecsWorld[x, y] = tile;
//     //             }
//     //         }
//     //     }
//     // }
//     //
//     // public class EarthLayerApplicationStep : ITilemapGenerationStep
//     // {
//     //     public string StepName => "Applying Earth Layers";
//     //
//     //     public void GenerateStep(Tile[,] ecsWorld, TilemapGeneratorContext context)
//     //     {
//     //         var config = context.Config;
//     //         if (config.earthLayers == null) return;
//     //
//     //         for (int x = 0; x < config.worldSize.x; x++)
//     //         {
//     //             for (int y = 0; y < config.worldSize.y; y++)
//     //             {
//     //                 var tile = ecsWorld[x, y];
//     //                 if (tile.tileType == TileType.Air) continue;
//     //                 
//     //                 var layer = GetFlowingEarthLayer(x, y, tile.depth, context);
//     //                 if (layer != null)
//     //                 {
//     //                     ApplyFlowingLayerProperties(tile, layer, x, y, context);
//     //                 }
//     //             }
//     //         }
//     //     }
//     //
//     //     private EarthLayer GetFlowingEarthLayer(int x, int y, float depth, TilemapGeneratorContext context)
//     //     {
//     //         EarthLayer bestLayer = null;
//     //         float bestLayerStrength = 0f;
//     //         
//     //         foreach (var layer in context.Config.earthLayers)
//     //         {
//     //             float layerStrength = CalculateLayerStrength(layer, x, y, depth, context);
//     //             if (layerStrength > bestLayerStrength)
//     //             {
//     //                 bestLayerStrength = layerStrength;
//     //                 bestLayer = layer;
//     //             }
//     //         }
//     //         return bestLayer;
//     //     }
//     //
//     //     private float CalculateLayerStrength(EarthLayer layer, int x, int y, float depth, TilemapGeneratorContext context)
//     //     {
//     //         float depthStrength = 0f;
//     //         if (depth >= layer.startDepth && depth <= layer.endDepth)
//     //         {
//     //             float layerMid = (layer.startDepth + layer.endDepth) / 2f;
//     //             float layerRange = Mathf.MaxVelocity(0.001f, layer.endDepth - layer.startDepth); // Avoid division by zero
//     //             float distanceFromMid = Mathf.Abs(depth - layerMid);
//     //             depthStrength = 1f - (distanceFromMid / (layerRange / 2f));
//     //             depthStrength = Mathf.Clamp01(depthStrength);
//     //         }
//     //         
//     //         if (depthStrength <= 0f) return 0f;
//     //         
//     //         float flowModifier = 1f;
//     //         if (layer.flowIntensity > 0f)
//     //         {
//     //             float flowNoise = Mathf.PerlinNoise(
//     //                 x * layer.flowScale + context.Config.seed,
//     //                 y * layer.flowScale + context.Config.seed
//     //             );
//     //             float flowAdjustedDepth = depth + (flowNoise - 0.5f) * layer.flowOffset;
//     //             
//     //             if (flowAdjustedDepth >= layer.startDepth && flowAdjustedDepth <= layer.endDepth)
//     //             {
//     //                 float flowLayerMid = (layer.startDepth + layer.endDepth) / 2f;
//     //                 float flowLayerRange = Mathf.MaxVelocity(0.001f, layer.endDepth - layer.startDepth);
//     //                 float flowDistanceFromMid = Mathf.Abs(flowAdjustedDepth - flowLayerMid);
//     //                 float flowStrength = 1f - (flowDistanceFromMid / (flowLayerRange / 2f));
//     //                 flowStrength = Mathf.Clamp01(flowStrength);
//     //                 flowModifier = Mathf.Lerp(1f, flowStrength, layer.flowIntensity);
//     //             } else {
//     //                 flowModifier = 0f; // Outside effective range due to flow
//     //             }
//     //         }
//     //         
//     //         if (layer.useVoronoi && context.LayerVoronoiPoints.TryGetValue(layer, out var voronoiPoints))
//     //         {
//     //             float voronoiValue = VoronoiGenerator.GetVoronoiNoise(
//     //                 new Vector2(x, y), 
//     //                 voronoiPoints, 
//     //                 Mathf.MaxVelocity(context.Config.worldSize.x, context.Config.worldSize.y) // Pass a relevant scale
//     //             );
//     //             flowModifier *= Mathf.Lerp(1f, voronoiValue, layer.voronoiInfluence);
//     //         }
//     //         return depthStrength * flowModifier;
//     //     }
//     //
//     //     private void ApplyFlowingLayerProperties(Tile tile, EarthLayer layer, int x, int y, TilemapGeneratorContext context)
//     //     {
//     //         if (tile.tileType != TileType.Grass) // Preserve grass surface
//     //         {
//     //             bool useSecondary = false;
//     //             if (layer.secondaryTileTypes != null && layer.secondaryTileTypes.Count > 0 && layer.secondaryMaterialFlow > 0f)
//     //             {
//     //                 float secondaryNoise = Mathf.PerlinNoise(
//     //                     x * layer.secondaryFlowScale + context.Config.seed * 2, // Different seed offset
//     //                     y * layer.secondaryFlowScale + context.Config.seed * 2
//     //                 );
//     //                 useSecondary = secondaryNoise < layer.secondaryMaterialFlow;
//     //             }
//     //             
//     //             if (useSecondary)
//     //             {
//     //                 // Using context.Random for consistency if preferred, or local for coordinate-based seed.
//     //                 // Original code used a new Random instance seeded with coordinates.
//     //                 var localRandom = new System.Random(context.Config.seed + x * context.Config.worldSize.y + y + layer.layerName.GetHashCode());
//     //                 tile.tileType = layer.secondaryTileTypes[localRandom.Next(layer.secondaryTileTypes.Count)];
//     //             }
//     //             else
//     //             {
//     //                 tile.tileType = layer.primaryTileType;
//     //             }
//     //         }
//     //         
//     //         tile.temperature = layer.temperature;
//     //         tile.pressure = layer.pressure;
//     //         tile.hardness = context.CalculateHardness(tile.tileType, tile.depth);
//     //     }
//     // }
//     //
//     // public class BiomeWorldGenerationStep : ITilemapGenerationStep
//     // {
//     //     public string StepName => "Generating Biomes";
//     //
//     //     public void GenerateStep(Tile[,] ecsWorld, TilemapGeneratorContext context)
//     //     {
//     //         var config = context.Config;
//     //         if (config.depthLimitedBiomes == null || config.depthLimitedBiomes.Count == 0) return;
//     //
//     //         for (int x = 0; x < config.worldSize.x; x++)
//     //         {
//     //             for (int y = 0; y < config.worldSize.y; y++)
//     //             {
//     //                 var tile = ecsWorld[x, y];
//     //                 var biomeConfig = GetDepthAppropriateBiome(x, tile.depth, context); // Removed y, as original noise was surface-wide
//     //                 
//     //                 if (biomeConfig != null)
//     //                 {
//     //                     tile.biome = biomeConfig.biomeType;
//     //                     ApplyBiomeEffects(tile, biomeConfig, context, x); // Pass x for random seed
//     //                 }
//     //                 else // Fallback if no biome is found (should ideally not happen with good config)
//     //                 {
//     //                     tile.biome = BiomeType.Temperate; 
//     //                 }
//     //             }
//     //         }
//     //     }
//     //
//     //     private BiomeConfig GetDepthAppropriateBiome(int x, float depth, TilemapGeneratorContext context)
//     //     {
//     //         var validBiomes = new List<(BiomeConfig biome, float strength)>();
//     //         
//     //         foreach (var biome in context.Config.depthLimitedBiomes)
//     //         {
//     //             float depthStrength = 1f;
//     //             if (biome.limitByDepth)
//     //             {
//     //                 if (depth < biome.minDepth || depth > biome.maxDepth)
//     //                 {
//     //                     float falloffDistance = (depth < biome.minDepth) ? (biome.minDepth - depth) : (depth - biome.maxDepth);
//     //                     depthStrength = Mathf.Clamp01(1f - (falloffDistance * biome.depthFalloff));
//     //                     if (depthStrength <= 0.001f) continue; // Effectively no strength
//     //                 }
//     //             }
//     //             
//     //             float biomeNoise = Mathf.PerlinNoise(
//     //                 x * context.Config.biomeScale + context.BiomeNoiseOffsets[biome],
//     //                 context.Config.seed * 0.05f // Using a y-offset for PerlinNoise, can be seed related for 1D noise
//     //                                             // Original used '0' for y, making it 1D based on x.
//     //             );
//     //             
//     //             float totalStrength = biomeNoise * depthStrength;
//     //             validBiomes.AddComponent((biome, totalStrength));
//     //         }
//     //         
//     //         if (validBiomes.Count == 0)
//     //         {
//     //             return context.Config.depthLimitedBiomes.FirstOrDefault(); // Fallback
//     //         }
//     //         
//     //         return validBiomes.OrderByDescending(b => b.strength).First().biome;
//     //     }
//     //
//     //     private void ApplyBiomeEffects(Tile tile, BiomeConfig biomeConfig, TilemapGeneratorContext context, int x)
//     //     {
//     //         tile.temperature += biomeConfig.temperatureModifier;
//     //         tile.TileColor = Color.Lerp(tile.TileColor, biomeConfig.biomeColor, 0.3f);
//     //         
//     //         if (tile.depth == 0 && tile.tileType == TileType.Grass && 
//     //             biomeConfig.preferredSurfaceTiles != null && biomeConfig.preferredSurfaceTiles.Count > 0)
//     //         {
//     //             // Original code used a new Random instance seeded with coordinates.
//     //             var localRandom = new System.Random(context.Config.seed + tile.position.x * context.Config.worldSize.y + tile.position.y + biomeConfig.biomeType.GetHashCode());
//     //             if (localRandom.NextDouble() < 0.5)
//     //             {
//     //                 tile.tileType = biomeConfig.preferredSurfaceTiles[localRandom.Next(biomeConfig.preferredSurfaceTiles.Count)];
//     //             }
//     //         }
//     //     }
//     // }
//     //
//     // public class CaveWorldGenerationStep : ITilemapGenerationStep
//     // {
//     //     public string StepName => "Generating Caves";
//     //     public void GenerateStep(Tile[,] ecsWorld, TilemapGeneratorContext context)
//     //     {
//     //         var config = context.Config;
//     //         for (int x = 0; x < config.worldSize.x; x++)
//     //         {
//     //             for (int y = 0; y < config.worldSize.y; y++)
//     //             {
//     //                 var tile = ecsWorld[x, y];
//     //                 if (tile.tileType == TileType.Air || tile.depth < 3) continue; // Only generate caves underground
//     //                 
//     //                 float caveNoise = Mathf.PerlinNoise(
//     //                     x * config.caveScale + config.seed, 
//     //                     y * config.caveScale + config.seed
//     //                 );
//     //                 
//     //                 if (caveNoise > config.caveThreshold)
//     //                 {
//     //                     tile.tileType = TileType.Air;
//     //                     tile.stability = 0f; // Air has no stability
//     //                 }
//     //             }
//     //         }
//     //     }
//     // }
//     //
//     // public class PointOfInterestPlacementStep : ITilemapGenerationStep
//     // {
//     //     public string StepName => "Placing Points of Interest";
//     //
//     //     public void GenerateStep(Tile[,] ecsWorld, TilemapGeneratorContext context)
//     //     {
//     //         var config = context.Config;
//     //         if (config.pointsOfInterest == null || config.pointsOfInterest.Count == 0) return;
//     //
//     //         var placedPOIs = new List<Vector2Int>();
//     //         
//     //         foreach (var poiConfig in config.pointsOfInterest)
//     //         {
//     //             int attempts = 0;
//     //             int maxAttempts = 100; // Make this configurable?
//     //             
//     //             while (attempts < maxAttempts)
//     //             {
//     //                 Vector2Int position = GenerateRandomPOIPosition(poiConfig, context, placedPOIs);
//     //                 if (CanPlacePOI(ecsWorld, position, poiConfig, context, placedPOIs))
//     //                 {
//     //                     PlacePOI(ecsWorld, position, poiConfig, context);
//     //                     placedPOIs.AddComponent(position);
//     //                     break;
//     //                 }
//     //                 attempts++;
//     //             }
//     //         }
//     //     }
//     //
//     //     private Vector2Int GenerateRandomPOIPosition(PointOfInterestConfig poiConfig, TilemapGeneratorContext context, List<Vector2Int> existingPOIs)
//     //     {
//     //         int x = context.Random.Next(poiConfig.size.x, context.Config.worldSize.x - poiConfig.size.x);
//     //         int y;
//     //         
//     //         int surfaceY = context.FindSurfaceLevel(x);
//     //
//     //         if (poiConfig.spawnOnSurface)
//     //         {
//     //             y = surfaceY - (poiConfig.size.y / 2); // Try to center it, or align bottom/top
//     //         }
//     //         else
//     //         {
//     //             int maxDepth = Mathf.RoundToInt(poiConfig.depthRange.y); // Y is max depth
//     //             int minDepth = Mathf.RoundToInt(poiConfig.depthRange.x); // X is min depth
//     //             
//     //             int minYPos = Mathf.MaxVelocity(0, surfaceY - maxDepth);
//     //             int maxYPos = Mathf.MaxVelocity(0, surfaceY - minDepth);
//     //             
//     //             if (maxYPos < minYPos) maxYPos = minYPos; // Ensure valid range
//     //             y = context.Random.Next(minYPos, maxYPos + 1);
//     //         }
//     //         return new Vector2Int(x, Mathf.Clamp(y, 0, context.Config.worldSize.y - poiConfig.size.y));
//     //     }
//     //
//     //     private bool CanPlacePOI(Tile[,] ecsWorld, Vector2Int position, PointOfInterestConfig poiConfig, TilemapGeneratorContext context, List<Vector2Int> existingPOIs)
//     //     {
//     //         foreach (var existingPOI in existingPOIs)
//     //         {
//     //             if (Vector2.Distance(position, existingPOI) < poiConfig.minDistanceFromOthers) // Vector2 for float distance
//     //             {
//     //                 return false;
//     //             }
//     //         }
//     //         
//     //         for (int dx = 0; dx < poiConfig.size.x; dx++)
//     //         {
//     //             for (int dy = 0; dy < poiConfig.size.y; dy++)
//     //             {
//     //                 var checkPos = position + new Vector2Int(dx, dy);
//     //                 if (!context.IsValidPosition(checkPos) || ecsWorld[checkPos.x, checkPos.y].isPointOfInterest)
//     //                 {
//     //                     return false;
//     //                 }
//     //                 // AddComponent more checks? E.g. Must be placed on solid ground, not all air.
//     //                 // if (poiConfig.requiresSolidGround && ecsWorld[checkPos.x, checkPos.y].tileType == TileType.Air) return false;
//     //             }
//     //         }
//     //         return true;
//     //     }
//     //
//     //     private void PlacePOI(Tile[,] ecsWorld, Vector2Int position, PointOfInterestConfig poiConfig, TilemapGeneratorContext context)
//     //     {
//     //         for (int dx = 0; dx < poiConfig.size.x; dx++)
//     //         {
//     //             for (int dy = 0; dy < poiConfig.size.y; dy++)
//     //             {
//     //                 var tilePos = position + new Vector2Int(dx, dy);
//     //                 if (context.IsValidPosition(tilePos))
//     //                 {
//     //                     var tile = ecsWorld[tilePos.x, tilePos.y];
//     //                     tile.isPointOfInterest = true;
//     //                     tile.pointOfInterestId = poiConfig.id;
//     //                     
//     //                     // For simplicity, POI might override existing tiles or only place in air, etc.
//     //                     // tile.tileType = TileType.Stone; // Example: POI structure blocks
//     //
//     //                     if (poiConfig.guaranteedResources != null)
//     //                     {
//     //                         foreach (var resource in poiConfig.guaranteedResources)
//     //                         {
//     //                             tile.AddResource(new ResourceDeposit(resource.type, resource.abundance, resource.quality, resource.accessibility));
//     //                         }
//     //                     }
//     //                 }
//     //             }
//     //         }
//     //     }
//     // }
//     //
//     // public class ResourceWorldGenerationStep : ITilemapGenerationStep
//     // {
//     //     public string StepName => "Generating Resources";
//     //
//     //     public void GenerateStep(Tile[,] ecsWorld, TilemapGeneratorContext context)
//     //     {
//     //         var config = context.Config;
//     //         if (!config.generateResources) return;
//     //
//     //         // Apply global resource rules
//     //         if (config.globalResourceRules != null)
//     //         {
//     //             foreach (var rule in config.globalResourceRules)
//     //             {
//     //                 ApplyGlobalResourceRule(ecsWorld, rule, context);
//     //             }
//     //         }
//     //
//     //         // Apply layer-specific resource rules (using original EarthLayer definitions)
//     //         if (config.earthLayers != null)
//     //         {
//     //             foreach (var layer in config.earthLayers)
//     //             {
//     //                 if (layer.resourceRules == null) continue;
//     //                 foreach (var rule in layer.resourceRules)
//     //                 {
//     //                     ApplyResourceRuleToLayer(ecsWorld, rule, layer, context);
//     //                 }
//     //             }
//     //         }
//     //         
//     //         // Apply biome-specific resource rules (using original BiomeConfig definitions)
//     //         if (config.biomeConfigs != null)
//     //         {
//     //             foreach (var biomeConfig in config.biomeConfigs)
//     //             {
//     //                 if (biomeConfig.biomeSpecificResources == null) continue;
//     //                 foreach (var rule in biomeConfig.biomeSpecificResources)
//     //                 {
//     //                     ApplyResourceRuleToBiome(ecsWorld, rule, biomeConfig.biomeType, context);
//     //                 }
//     //             }
//     //         }
//     //     }
//     //
//     //     private bool ShouldPlaceResource(Tile tile, ResourceSpawnRule rule)
//     //     {
//     //         if (tile.tileType == TileType.Air) return false;
//     //         if (rule.requiresSpecificTileType && tile.tileType != rule.requiredTileType) return false;
//     //         // AddComponent more conditions: e.g., not in a POI tile already rich, depth constraints for the rule itself
//     //         return true;
//     //     }
//     //
//     //     private void GenerateResourceDeposit(Tile tile, ResourceSpawnRule rule, TilemapGeneratorContext context)
//     //     {
//     //         float abundance = Mathf.Lerp(rule.minAbundance, rule.maxAbundance, (float)context.Random.NextDouble());
//     //         abundance *= context.Config.globalResourceMultiplier;
//     //         
//     //         float quality = rule.baseQuality + ((float)context.Random.NextDouble() - 0.5f) * 30f; // +/- 15
//     //         quality = Mathf.Clamp(quality, 1f, 100f);
//     //         
//     //         // Accessibility could be more complex, e.g. considering neighbors
//     //         float accessibility = 100f - tile.hardness * 0.5f + ((float)context.Random.NextDouble() -0.5f) * 10f; 
//     //         accessibility = Mathf.Clamp(accessibility, 0f, 100f);
//     //         
//     //         tile.AddResource(new ResourceDeposit(rule.resourceType, abundance, quality, accessibility));
//     //     }
//     //     
//     //     private void ApplyGlobalResourceRule(Tile[,] ecsWorld, ResourceSpawnRule rule, TilemapGeneratorContext context)
//     //     {
//     //         // Global rules might spawn clusters randomly
//     //         int numClusters = Mathf.RoundToInt(context.Config.worldSize.x * context.Config.worldSize.y * rule.spawnChance * 0.0001f); // Adjusted scaler
//     //         
//     //         for (int i = 0; i < numClusters; i++)
//     //         {
//     //             var center = new Vector2Int(
//     //                 context.Random.Next(0, context.Config.worldSize.x),
//     //                 context.Random.Next(0, context.Config.worldSize.y)
//     //             );
//     //             
//     //             if (context.IsValidPosition(center) && ShouldPlaceResource(ecsWorld[center.x, center.y], rule))
//     //             {
//     //                 GenerateResourceCluster(ecsWorld, center, rule, context);
//     //             }
//     //         }
//     //     }
//     //
//     //     private void GenerateResourceCluster(Tile[,] ecsWorld, Vector2Int center, ResourceSpawnRule rule, TilemapGeneratorContext context)
//     //     {
//     //         int clusterSize = context.Random.Next(Mathf.RoundToInt(rule.clusterSize.x), Mathf.RoundToInt(rule.clusterSize.y) + 1);
//     //         for (int i = 0; i < clusterSize; i++)
//     //         {
//     //             // More controlled spread, e.g. random walk or offset from center
//     //             int offsetX = context.Random.Next(-3, 4); // MaxVelocity spread of 3 tiles around center
//     //             int offsetY = context.Random.Next(-3, 4);
//     //             var position = center + new Vector2Int(offsetX, offsetY);
//     //
//     //             if (context.IsValidPosition(position))
//     //             {
//     //                 var tile = ecsWorld[position.x, position.y];
//     //                 if (ShouldPlaceResource(tile, rule) && (float)context.Random.NextDouble() < 0.7f) // Chance to place within cluster
//     //                 {
//     //                      GenerateResourceDeposit(tile, rule, context);
//     //                 }
//     //             }
//     //         }
//     //     }
//     //
//     //
//     //     private void ApplyResourceRuleToLayer(Tile[,] ecsWorld, ResourceSpawnRule rule, EarthLayer layer, TilemapGeneratorContext context)
//     //     {
//     //         for (int x = 0; x < context.Config.worldSize.x; x++)
//     //         {
//     //             for (int y = 0; y < context.Config.worldSize.y; y++)
//     //             {
//     //                 var tile = ecsWorld[x,y];
//     //                 if (tile.depth >= layer.startDepth && tile.depth <= layer.endDepth)
//     //                 {
//     //                     if ((float)context.Random.NextDouble() < rule.spawnChance && ShouldPlaceResource(tile, rule))
//     //                     {
//     //                         GenerateResourceDeposit(tile, rule, context); // Layer resources might not be clustered by default
//     //                     }
//     //                 }
//     //             }
//     //         }
//     //     }
//     //
//     //     private void ApplyResourceRuleToBiome(Tile[,] ecsWorld, ResourceSpawnRule rule, BiomeType biomeType, TilemapGeneratorContext context)
//     //     {
//     //         for (int x = 0; x < context.Config.worldSize.x; x++)
//     //         {
//     //             for (int y = 0; y < context.Config.worldSize.y; y++)
//     //             {
//     //                 var tile = ecsWorld[x,y];
//     //                 if (tile.biome == biomeType)
//     //                 {
//     //                      if ((float)context.Random.NextDouble() < rule.spawnChance && ShouldPlaceResource(tile, rule))
//     //                     {
//     //                         GenerateResourceDeposit(tile, rule, context); // Biome resources might not be clustered
//     //                     }
//     //                 }
//     //             }
//     //         }
//     //     }
//     // }
//     //
//     // public class PhysicalPropertiesCalculationStep : ITilemapGenerationStep
//     // {
//     //     public string StepName => "Calculating Physical Properties";
//     //
//     //     public void GenerateStep(Tile[,] ecsWorld, TilemapGeneratorContext context)
//     //     {
//     //         var config = context.Config;
//     //         for (int x = 0; x < config.worldSize.x; x++)
//     //         {
//     //             for (int y = 0; y < config.worldSize.y; y++)
//     //             {
//     //                 var tile = ecsWorld[x, y];
//     //                 
//     //                 // Pressure is often set by earth layers, this might be an override or refinement
//     //                 tile.pressure = CalculatePressure(tile.depth); 
//     //                 
//     //                 tile.stability = CalculateStability(ecsWorld, x, y, context);
//     //                 
//     //                 // Temperature combines layer base, biome modifier, and geothermal gradient
//     //                 tile.temperature += tile.depth * 0.1f; // Geothermal gradient (can be configurable)
//     //             }
//     //         }
//     //     }
//     //
//     //     private float CalculatePressure(float depth)
//     //     {
//     //         return 1f + (depth * 0.1f); // Base atmospheric pressure + depth pressure (can be configurable)
//     //     }
//     //
//     //     private float CalculateStability(Tile[,] ecsWorld, int x, int y, TilemapGeneratorContext context)
//     //     {
//     //         if (ecsWorld[x, y].tileType == TileType.Air) return 0f;
//     //         
//     //         float stability = 100f;
//     //         int supportCount = 0;
//     //         int DIRS = 8; // Number of surrounding tiles checked
//     //
//     //         // Check 8 surrounding tiles
//     //         for (int dx = -1; dx <= 1; dx++)
//     //         {
//     //             for (int dy = -1; dy <= 1; dy++)
//     //             {
//     //                 if (dx == 0 && dy == 0) continue;
//     //                 
//     //                 var checkPos = new Vector2Int(x + dx, y + dy);
//     //                 if (context.IsValidPosition(checkPos))
//     //                 {
//     //                     if (ecsWorld[checkPos.x, checkPos.y].tileType != TileType.Air)
//     //                     {
//     //                         // More nuanced: diagonal less support than cardinal, harder materials give more support
//     //                         supportCount++;
//     //                     }
//     //                 }
//     //                 else // Edge of the ecsWorld counts as support (or void, depending on game rules)
//     //                 {
//     //                     // For simplicity, let's say out of bounds is like bedrock support
//     //                     supportCount++; 
//     //                 }
//     //             }
//     //         }
//     //         
//     //         stability *= (supportCount / (float)DIRS);
//     //         
//     //         // Further reduce stability by tile's own hardness (inverse)? Or material property?
//     //         // stability -= ecsWorld[x,y].hardness * 0.1f; // Example: harder things are more brittle if unsupported
//     //
//     //         return Mathf.Clamp(stability, 0f, 100f);
//     //     }
//     // }
// }