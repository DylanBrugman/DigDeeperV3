using System.Collections.Generic;
using DigDeeper.WorldSystem;
using GamePlay.World.Tilemap;
using Systems.WorldSystem;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace GamePlay.World {
    public class WorldGenerationContext {
        
        // public WorldGenerationConfig Config { get; set; }
        // public int2 WorldSizeChunks => Config.worldSizeChunks;
        // public Random Random { get; private set; }
        // public TileGrid TileGrid { get; private set; }
        // public DepthGrid DepthGrid { get; internal set; }
        // // public Dictionary<BiomeConfig, float> BiomeNoiseOffsets { get; }
        // public byte[] SurfaceHeights { get; internal set; }
        //
        // public WorldGenerationContext(WorldGenerationConfig config, Random random) {
        //     Config = config;
        //     Random = random;
        //     TileGrid = new TileGrid(config.worldSizeChunks);
        //     DepthGrid = new DepthGrid(config.worldSizeChunks);
        //     SurfaceHeights = new byte[config.worldSizeChunks.x * 32];
        // }
        //
        // public int2 GetWorldSizeTiles() => new int2(WorldSizeChunks.x * 32, WorldSizeChunks.y * 32);
    }
}