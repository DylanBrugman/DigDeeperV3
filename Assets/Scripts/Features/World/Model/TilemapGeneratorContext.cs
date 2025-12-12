using System.Collections.Generic;
using DigDeeper.WorldSystem;
using GamePlay.World.Tilemap;
using Systems.WorldSystem;
using Systems.WorldSystem.Generator;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;
using Tile = Systems.WorldSystem.Generator.Tile;
using TileType = GamePlay.World.Tilemap.Generator.TileType;

namespace GamePlay.World {
    public class TilemapGeneratorContext {
        public WorldGenerationConfig Config { get; }
        public Random Random { get; }
        public Dictionary<BiomeConfig, float> BiomeNoiseOffsets { get; }
        public Vector2Int WorldSizeChunks;
        public int[] SurfaceHeights { get; internal set; }

        public TilemapGeneratorContext(WorldGenerationConfig config, Random random, Dictionary<BiomeConfig, float> biomeNoiseOffsets) {
            Config = config;
            Random = random;
            BiomeNoiseOffsets = biomeNoiseOffsets;
            WorldSizeChunks = config.worldSizeChunks;
        }

        public TilemapGeneratorContext(WorldGenerationConfig config, Random random) {
            Config = config;
            Random = random;
        }

        public bool IsValidPosition(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < WorldSizeChunks.x && pos.y >= 0 && pos.y < WorldSizeChunks.y;
        }
        
        public int FindSurfaceLevel(int x, Tile[,] world)
        {
            for (int y = WorldSizeChunks.y - 1; y >= 0; y--) // Start from top and go down
            {
                // Check against Air, assuming Air is "sky" and we want the first non-Air tile from the top.
                // If ecsWorld is generated from bottom up, this needs to be adjusted.
                // Given surfaceHeight logic, y=0 is bottom of map, y=worldSize.y-1 is top.
                // So we search from Config.worldSize.y - 1 downwards for the first solid tile.
                // This is typically for surface level *after* initial generation.
                // The SurfaceHeights array is for the *initial* surface.
                if (world[x,y].tileType != TileType.Air)
                {
                    return y;
                }
            }
            return 0; // Fallback: bottom of the ecsWorld
        }


        public float CalculateHardness(TileType tileType, float depth)
        {
            // Example: Implement global hardness calculation logic if needed
            // This was originally in EarthLayerApplicationStep, moved here if it's a global utility
            // For now, let's assume it's specific to how EarthLayers define hardness and keep it there or make it part of TileType definition
            float baseHardness = GetBaseHardness(tileType);
            return baseHardness + depth * 0.05f; // Hardness increases slightly with depth
        }
        
        private float GetBaseHardness(TileType type) // Example
        {
            switch (type)
            {
                case TileType.Dirt: return 20f;
                case TileType.Stone: return 50f;
                case TileType.Grass: return 15f;
                default: return 30f;
            }
        }
    }
}