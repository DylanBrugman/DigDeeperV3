using GamePlay.Map.Generator.New.Core.WorldGen;
using Unity.Mathematics;

namespace GamePlay.World {
    public sealed class WorldDTO
    {
        public TileChunk[] Chunks { get; set; }
        // public LiquidLayer Liquid { get; set; }
        // public GasLayer   Gas     { get; set; }
        public int2       SizeChunks { get; set; }
     
        public WorldDTO(WorldGenerationContext context) {
            throw new System.NotImplementedException();
        }
    }
    
    public sealed class LiquidLayer{ public float[] Heights; }
    public sealed class GasLayer   { public byte[] Density; }
}