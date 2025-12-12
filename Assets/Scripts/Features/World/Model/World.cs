using Unity.Mathematics;
using UnityEngine;

namespace GamePlay.World {
    public sealed class World
    {
        // public TileChunk[] Chunks { get; set; }
        // public LiquidLayer Liquid { get; set; }
        // public GasLayer   Gas     { get; set; }
        public Vector2Int       SizeChunks { get; set; }
        
        public World(Vector2Int sizeChunks) {
            SizeChunks = sizeChunks;
        }

        public World() {
        }
    }
}