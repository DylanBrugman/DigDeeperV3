using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.World.Tilemap.GenerationPipeline.Step;
using GamePlay.World.Tilemap.Generator;
using Unity.Jobs;
using UnityEngine;

namespace GamePlay.World.Tilemap.GenerationPipeline {
    public sealed class TilemapGenerationPipeline {
        readonly ITilemapGenerationStep[] _steps;
        public TilemapGenerationPipeline(params ITilemapGenerationStep[] steps) => _steps = steps;

        public JobHandle Build(WorldGenerationContext worldGenerationContext) {
            JobHandle handle = default;
            foreach (var s in _steps) {
                handle = s.Schedule(worldGenerationContext, handle);
            }

            return handle;
        }
    }
}