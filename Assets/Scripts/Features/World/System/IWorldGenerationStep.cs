using System;
using System.Collections;

namespace GamePlay.World {
    public interface IWorldGenerationStep
    {
        string StepName { get; }
        IEnumerator GenerateStep(WorldGenerationContext worldGenerationContext, Action<float> onProgress);
    }
}