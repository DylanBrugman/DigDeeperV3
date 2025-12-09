using System;
using System.Collections.Generic;
using DigDeeper.WorldSystem;
using GamePlay.World.DigDeeper.WorldSystem;
using Systems.WorldSystem.Generator;
using Unity.Mathematics;
using UnityEngine;

namespace Systems.WorldSystem {
    [CreateAssetMenu(fileName = "ECSSystem Generation Config", menuName = "DigDeeper/ECSSystem Generation Config")]
    public class WorldGenerationConfig : ScriptableObject
    {
        [Header("World settings")]
        // public Vector2Int worldSize = new Vector2Int(200, 100);
        public int2 worldSizeChunks = new int2(6, 3); // SIZE in chunks (32×32), not tiles
        
        [Header("Surface Settings")]
        public int surfaceAirHeightMinimal = 20;
        public float SurfaceHeightScale { get; set; }
        
        [Header("Noise Settings")]
        public int seed = 12345;
        public float terrainScale = 0.02f;
        public float terrainAmplitude = 10f;
        public float caveScale = 0.05f;
        public float caveThreshold = 0.6f;
        
        [Header("Earth Layers")]
        public List<EarthLayer> earthLayers = new List<EarthLayer>();
        
        [Header("Biome Settings")]
        public float biomeScale = 0.01f;
        public List<BiomeConfig> biomeConfigs = new List<BiomeConfig>();
        
        [Header("Resource Generation")]
        public bool generateResources = true;
        public float globalResourceMultiplier = 1f;
        public List<ResourceSpawnRule> globalResourceRules = new List<ResourceSpawnRule>();
        
        [Header("Points of Interest")]
        public List<PointOfInterestConfig> pointsOfInterest = new List<PointOfInterestConfig>();
        
        [Header("Enhanced Biome System")]
        public List<BiomeConfig> depthLimitedBiomes = new List<BiomeConfig>();
        
        [Header("Auto-Update Settings")]
        public bool autoUpdateOnChange = true;
        public float updateDelay = 0.5f; // Delay before updating to avoid spam
        
        [Header("Performance Settings")]
        public bool useMultithreading = false; // For future optimization
        public int chunkSize = 64; // Process ecsWorld in chunks for better performance
        
        public float layerScale = 0.3f;

    }
}