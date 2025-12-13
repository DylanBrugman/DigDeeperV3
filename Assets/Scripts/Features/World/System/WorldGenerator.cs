

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Features.World.Model;
using GamePlay.World;
using Systems.WorldSystem;
using UnityEngine;

namespace Features.World.System {
    public static class WorldGenerator {
        
        public static async Task<GeneratedWorld> Generate(List<IWorldGenerationStep> worldGenerationSteps, WorldConfig worldConfig) {
            
            WorldGenerationContext context = new (
                // CurrentWorldConfig,
                // new System.Random(CurrentWorldConfig.seed)
            );

            foreach (IWorldGenerationStep worldGenerationStep in worldGenerationSteps) {
                Debug.Log($"Running world generation step: {worldGenerationStep.GetType().Name}");
                await worldGenerationStep.GenerateStep(context);
                Debug.Log($"Finished world generation step: {worldGenerationStep.GetType().Name}");
            }
            return new GeneratedWorld(context);
        }
    }
}