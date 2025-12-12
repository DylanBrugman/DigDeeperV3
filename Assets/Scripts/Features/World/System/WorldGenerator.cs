using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using GamePlay.World;
using Systems.WorldSystem;
using UnityEngine;

namespace Features.World.System {
    public class WorldGenerator : IWorldSource {
        
        [SerializeField] private List<WorldGenerationConfig> mapConfigs = new List<WorldGenerationConfig>();
        [SerializeField] private int configIndex = 0;
        [SerializeField] private bool forceGenerateNewWorld = false;
        
        public WorldGenerationConfig CurrentWorldConfig { get; private set; }

        private List<IWorldGenerationStep> _worldGenerationSteps = new List<IWorldGenerationStep>();

        private void Awake() {
            ServiceLocator.Register<IWorldSource>(this);
            ServiceLocator.Register(this);
        }

        // private void Start() {
        //     _worldGenerationSteps.Add(new TilemapGenerator());
        // }

        public IEnumerator Generate(Action<GamePlay.World.World> onDone) {
            CurrentWorldConfig = mapConfigs[configIndex];
            
            WorldGenerationContext context = new (
                // CurrentWorldConfig,
                // new System.Random(CurrentWorldConfig.seed)
            );

            foreach (IWorldGenerationStep worldGenerationStep in _worldGenerationSteps) {
                Debug.Log($"Running world generation step: {worldGenerationStep.GetType().Name}");
                yield return worldGenerationStep.GenerateStep(context);
                Debug.Log($"Finished world generation step: {worldGenerationStep.GetType().Name}");
            }
            
            GamePlay.World.World world = new GamePlay.World.World();
            Debug.Log("World generation completed successfully.");
            onDone?.Invoke(world);
        }

        private void OnValidate() {
            if (mapConfigs.Count == 0) {
                Debug.LogWarning("No Map Generation Configs assigned. Please add at least one config.");
            }
            if (configIndex >= mapConfigs.Count) {
                Debug.LogWarning($"Default World Config Index {configIndex} is out of bounds. Resetting to 0.");
                configIndex = 0;
            }
        }
    }
}