using System;
using System.Collections;
using GamePlay.Map.Generator;

namespace GamePlay.World {
    public interface IWorldGenerationStep
    {
        string StepName { get; }
        IEnumerator GenerateStep(WorldGenerationContext worldGenerationContext, Action<float> onProgress);
    }
}