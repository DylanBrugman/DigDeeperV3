using Unity.Jobs;

namespace GamePlay.World.Tilemap.GenerationPipeline {
    public interface ITilemapGenerationStep {
        JobHandle Schedule(WorldGenerationContext context, JobHandle dependsOn);
    }
}