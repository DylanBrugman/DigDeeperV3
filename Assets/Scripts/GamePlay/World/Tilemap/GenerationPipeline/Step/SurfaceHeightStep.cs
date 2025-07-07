using Core.Grid;
using Systems.WorldSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace GamePlay.World.Tilemap.GenerationPipeline.Step {
    
    public sealed class SurfaceHeightStep : ITilemapGenerationStep {
        public JobHandle Schedule(WorldGenerationContext context, JobHandle dependsOn)
        {
            WorldGenerationConfig config = context.Config;
            float surfaceHeightScale = config.SurfaceHeightScale;
            int surfaceHeightMaximal = context.GetWorldSizeTiles().y - config.surfaceAirHeightMinimal;
            var surfaceHeights = context.SurfaceHeights;

            int randomOffset = context.Random.Next(1000);
            for (int i = 0; i < surfaceHeights.Length; i++) {
                float n = noise.cnoise(new float2(i, randomOffset));
                surfaceHeights[i] = (byte) (surfaceHeightMaximal - math.floor(n * surfaceHeightScale));
            }
            
            JobHandle jobHandle = new CalculateDepthJob {
                SurfaceHeights = surfaceHeights,
                DepthGridWriter = context.DepthGrid.AsParallelWriter(),
                WorldSizeChunks = context.WorldSizeChunks
            }.Schedule(surfaceHeights.Length, 64, dependsOn);
            return dependsOn;
        }
    }

    [BurstCompile]
    public struct CalculateDepthJob {
        [ReadOnly] public byte[] SurfaceHeights;
        [NativeDisableParallelForRestriction] public DepthGrid.GridWriter<int> DepthGridWriter;
        public int2 WorldSizeChunks;

        public void Execute(int index) {
            int x = index % WorldSizeChunks.x;
            int y = index / WorldSizeChunks.x;
            int surfaceHeight = SurfaceHeights[index];
            for (int dy = 0; dy < WorldSizeChunks.y * 32; dy++) {
                if (dy < surfaceHeight) {
                    DepthGridWriter.Set(x, dy, 0);
                } else {
                    DepthGridWriter.Set(x, dy, dy - surfaceHeight);
                }
            }
        }
    }
}