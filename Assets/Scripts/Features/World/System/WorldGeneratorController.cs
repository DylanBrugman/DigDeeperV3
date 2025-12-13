using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Features.World.Model;
using GamePlay.World.Tilemap.GenerationPipeline;
using Systems.WorldSystem;
using UnityEngine;

namespace Features.World.System {
    public class WorldGeneratorController : MonoBehaviour {
        
        [SerializeField] private WorldConfig _worldConfig;
        [SerializeField] private bool _forceGenerateNewWorld;

        private List<IWorldGenerationStep> worldGenerationSteps = new List<IWorldGenerationStep>();

        private void Awake() {
            worldGenerationSteps.Add(new TilemapGenerator());
        }
        
        private void Start() {
            Generate();
        }

        public async Task Generate() {
            GeneratedWorld generatedWorld = await WorldGenerator.Generate(worldGenerationSteps, _worldConfig);
        }

        private void OnValidate() {
            if (_forceGenerateNewWorld) {
                Generate();
            }
        }
    }
}