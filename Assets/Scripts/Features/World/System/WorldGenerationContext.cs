using System;
using System.Collections.Generic;

namespace Features.World.System {
    public class WorldGenerationContext {
        private readonly Dictionary<Type, object> _results = new();
    
        public void Set<T>(T result) => _results[typeof(T)] = result;
    
        public T Get<T>() {
            if (!_results.TryGetValue(typeof(T), out object result)) {
                throw new InvalidOperationException($"Missing dependency: {typeof(T).Name}");
            }
            return (T)result;
        }
    }
    
    
    // public class WorldGenerationContext {
    //     
    //     public WorldGenerationConfig Config { get; set; }
    //     public Vector2Int WorldSizeChunks => Config.worldSizeChunks;
    //     public Random Random { get; private set; }
    //     public World World { get; private set; }
    //
    //     public WorldGenerationContext(WorldGenerationConfig config, Random random, World world) {
    //         Config = config;
    //         Random = random;
    //         World = world;
    //     }
    //
    //     public WorldGenerationContext() {
    //     }

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
    // }
}