using System.Threading.Tasks;
using Features.World.System;

namespace GamePlay.World.Tilemap.GenerationPipeline {
    public class TilemapGenerator : IWorldGenerationStep {
        public string StepName { get; }

        public Task GenerateStep(WorldGenerationContext worldGenerationContext) {
            
            return Task.CompletedTask;
        }
        //
        // public IEnumerator GenerateStep(WorldGenerationContext worldGenerationContext) {
        //     var pipe = new TilemapGenerationPipeline(
        //         new SurfaceHeightStep(),
        //         new StrataStep(),
        //         new CaveCarveStep(),
        //         new OreVeinStep()
        //         // new ErosionStep(),
        //         // new DecorationStep(),
        //         // new NavBootstrapStep()
        //     );
        //
        //     JobHandle handle = pipe.Build(worldGenerationContext);
        //     yield return new WaitUntil(() => handle.IsCompleted);
        //     handle.Complete(); // or yield in a coroutine
        // }
    }
}