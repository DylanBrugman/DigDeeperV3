// using System;
// using System.Collections;
// using GamePlay.World.Tilemap.GenerationPipeline.Step;
// using GamePlay.World.Tilemap.Generator;
// using Unity.Jobs;
// using UnityEngine;
//
// namespace GamePlay.World.Tilemap.GenerationPipeline {
//     public class TilemapGenerator : IWorldGenerationStep {
//         private TilemapGenerationPipeline _pipeline;
//
//         public string StepName { get; }
//
//         public IEnumerator GenerateStep(WorldGenerationContext worldGenerationContext, Action<float> onProgress) {
//             var pipe = new TilemapGenerationPipeline(
//                 new SurfaceHeightStep(),
//                 new StrataStep(),
//                 new CaveCarveStep(),
//                 new OreVeinStep()
//                 // new ErosionStep(),
//                 // new DecorationStep(),
//                 // new NavBootstrapStep()
//             );
//
//             JobHandle handle = pipe.Build(worldGenerationContext);
//             yield return new WaitUntil(() => handle.IsCompleted);
//             handle.Complete(); // or yield in a coroutine
//         }
//     }
// }