using System.Collections.Generic;
using DigDeeper.WorldSystem;
using GamePlay.World.Tilemap.Generator;
using Systems.WorldSystem;
using Systems.WorldSystem.Generator;
using Unity.Mathematics;
using UnityEngine;

namespace Editor.DigDeeper.WorldSystem {
    public static class WorldConfigPresets
    {
        public static WorldGenerationConfig CreateEarthLikeWorld()
        {
            var config = ScriptableObject.CreateInstance<WorldGenerationConfig>();
            config.name = "Earth-Like ECSSystem";
            config.worldSizeChunks = new int2(10, 5);
            config.surfaceAirHeightMinimal = 20;
            config.seed = 12345;
            config.terrainScale = 0.02f;
            config.terrainAmplitude = 15f;
            config.caveScale = 0.05f;
            config.caveThreshold = 0.6f;
            config.generateResources = true;
            config.globalResourceMultiplier = 1f;
            
            // AddComponent realistic earth layers
            config.earthLayers = new List<EarthLayer>
            {
                new EarthLayer
                {
                    layerName = "Topsoil",
                    startDepth = 0,
                    endDepth = 2,
                    primaryTileType = TileType.Dirt,
                    temperature = 15f,
                    pressure = 1f
                },
                new EarthLayer
                {
                    layerName = "Subsoil",
                    startDepth = 2,
                    endDepth = 8,
                    primaryTileType = TileType.Clay,
                    temperature = 18f,
                    pressure = 1.2f
                },
                new EarthLayer
                {
                    layerName = "Weathered Rock",
                    startDepth = 8,
                    endDepth = 25,
                    primaryTileType = TileType.Stone,
                    temperature = 25f,
                    pressure = 2f
                },
                new EarthLayer
                {
                    layerName = "Solid Rock",
                    startDepth = 25,
                    endDepth = 80,
                    primaryTileType = TileType.Granite,
                    temperature = 45f,
                    pressure = 4f
                },
                new EarthLayer
                {
                    layerName = "Deep Crust",
                    startDepth = 80,
                    endDepth = 150,
                    primaryTileType = TileType.Bedrock,
                    temperature = 80f,
                    pressure = 8f
                }
            };
            
            // AddComponent common resources
            config.globalResourceRules = new List<ResourceSpawnRule>
            {
                new ResourceSpawnRule
                {
                    resourceType = ResourceType.Coal,
                    spawnChance = 0.12f,
                    minAbundance = 30f,
                    maxAbundance = 90f,
                    baseQuality = 50f,
                    clusterSize = new Vector2(5, 12)
                },
                new ResourceSpawnRule
                {
                    resourceType = ResourceType.Iron,
                    spawnChance = 0.08f,
                    minAbundance = 20f,
                    maxAbundance = 70f,
                    baseQuality = 60f,
                    clusterSize = new Vector2(3, 8)
                },
                new ResourceSpawnRule
                {
                    resourceType = ResourceType.Copper,
                    spawnChance = 0.06f,
                    minAbundance = 15f,
                    maxAbundance = 55f,
                    baseQuality = 65f,
                    clusterSize = new Vector2(2, 6)
                },
                new ResourceSpawnRule
                {
                    resourceType = ResourceType.Gold,
                    spawnChance = 0.02f,
                    minAbundance = 5f,
                    maxAbundance = 25f,
                    baseQuality = 85f,
                    clusterSize = new Vector2(1, 3)
                }
            };
            
            return config;
        }
        
        public static WorldGenerationConfig CreateAlienWorld()
        {
            var config = ScriptableObject.CreateInstance<WorldGenerationConfig>();
            config.name = "Alien ECSSystem";
            config.worldSizeChunks = new int2(8, 4);
            config.surfaceAirHeightMinimal = 15;
            config.seed = 54321;
            config.terrainScale = 0.03f;
            config.terrainAmplitude = 20f;
            config.caveScale = 0.08f;
            config.caveThreshold = 0.5f;
            config.generateResources = true;
            config.globalResourceMultiplier = 1.5f;
            
            // Alien-like layers
            config.earthLayers = new List<EarthLayer>
            {
                new EarthLayer
                {
                    layerName = "Crystalline Surface",
                    startDepth = 0,
                    endDepth = 5,
                    primaryTileType = TileType.Ice,
                    temperature = -10f,
                    pressure = 0.8f
                },
                new EarthLayer
                {
                    layerName = "Mineral Deposits",
                    startDepth = 5,
                    endDepth = 30,
                    primaryTileType = TileType.Marble,
                    temperature = 5f,
                    pressure = 1.5f
                },
                new EarthLayer
                {
                    layerName = "Molten Core",
                    startDepth = 30,
                    endDepth = 120,
                    primaryTileType = TileType.Lava,
                    temperature = 200f,
                    pressure = 10f
                }
            };
            
            // Exotic resources
            config.globalResourceRules = new List<ResourceSpawnRule>
            {
                new ResourceSpawnRule
                {
                    resourceType = ResourceType.Uranium,
                    spawnChance = 0.15f,
                    minAbundance = 40f,
                    maxAbundance = 80f,
                    baseQuality = 70f,
                    clusterSize = new Vector2(3, 10)
                },
                new ResourceSpawnRule
                {
                    resourceType = ResourceType.Diamond,
                    spawnChance = 0.08f,
                    minAbundance = 20f,
                    maxAbundance = 60f,
                    baseQuality = 90f,
                    clusterSize = new Vector2(2, 5)
                },
                new ResourceSpawnRule
                {
                    resourceType = ResourceType.Rare_Earth,
                    spawnChance = 0.12f,
                    minAbundance = 30f,
                    maxAbundance = 70f,
                    baseQuality = 80f,
                    clusterSize = new Vector2(4, 8)
                }
            };
            
            return config;
        }
        
        public static WorldGenerationConfig CreateTestWorld()
        {
            var config = ScriptableObject.CreateInstance<WorldGenerationConfig>();
            config.name = "Test ECSSystem (Small)";
            config.worldSizeChunks = new int2(3, 2);
            config.surfaceAirHeightMinimal = 10;
            config.seed = 11111;
            config.terrainScale = 0.05f;
            config.terrainAmplitude = 8f;
            config.caveScale = 0.1f;
            config.caveThreshold = 0.7f;
            config.generateResources = true;
            config.globalResourceMultiplier = 2f; // More resources for testing
            
            // Simple layer structure
            config.earthLayers = new List<EarthLayer>
            {
                new EarthLayer
                {
                    layerName = "Surface",
                    startDepth = 0,
                    endDepth = 8,
                    primaryTileType = TileType.Dirt,
                    temperature = 20f,
                    pressure = 1f
                },
                new EarthLayer
                {
                    layerName = "Rock",
                    startDepth = 8,
                    endDepth = 50,
                    primaryTileType = TileType.Stone,
                    temperature = 30f,
                    pressure = 2f
                }
            };
            
            // High resource density for testing
            config.globalResourceRules = new List<ResourceSpawnRule>
            {
                new ResourceSpawnRule
                {
                    resourceType = ResourceType.Coal,
                    spawnChance = 0.3f,
                    minAbundance = 50f,
                    maxAbundance = 100f,
                    baseQuality = 70f,
                    clusterSize = new Vector2(3, 6)
                },
                new ResourceSpawnRule
                {
                    resourceType = ResourceType.Iron,
                    spawnChance = 0.25f,
                    minAbundance = 40f,
                    maxAbundance = 80f,
                    baseQuality = 60f,
                    clusterSize = new Vector2(2, 5)
                }
            };
            
            return config;
        }
    }
}