using Core.Grid;
using GamePlay.Map.Generator.New.Core.WorldGen;
using GamePlay.World.Tilemap.Generator;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace GamePlay.World.Tilemap.GenerationPipeline.Step {
    public sealed class CaveCarveStep : ITilemapGenerationStep
    {
        public JobHandle Schedule(WorldGenerationContext context, JobHandle deps)
        {
            int chunkCount = context.SizeChunks.x * context.SizeChunks.y;
            return new CaveJob
            {
                NativeTileGrid      = context.TileGrid.Native,
                SizeChunks = context.SizeChunks
            }.Schedule(chunkCount, 64, deps);
        }
    }
    
    [BurstCompile]
    struct CaveJob : IJobParallelFor
    {
        public NativeChunkedGrid<Tile> NativeTileGrid;
        public int2 SizeChunks;

        public void Execute(int chunkIndex)
        {
            int2 c = new int2(chunkIndex % SizeChunks.x, chunkIndex / SizeChunks.x);
            int baseX = c.x * TileChunk.Size;
            int baseY = c.y * TileChunk.Size;

            for (int ly = 0; ly < TileChunk.Size; ly++)
            for (int lx = 0; lx < TileChunk.Size; lx++)
            {
                float3 p = new float3(baseX+lx, baseY+ly, 0) * 0.05f;
                float density = noise.cnoise(p) * 0.5f + noise.cnoise(p*2) * 0.25f;
                if (density > 0.2f) continue;     // leave solid
                
                
                NativeTileGrid.(baseX + lx, baseY + ly, );
                ref var tile = ref NativeTileGrid.Get(baseX + lx, baseY + ly);
                tile.Type = TileType.Air;
            }
        }
    }
}