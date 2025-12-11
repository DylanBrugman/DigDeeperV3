// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Threading;
// using Core;
// using GamePlay.Map.Generator;
// using GamePlay.World.Tilemap.Generator;
// using Systems.WorldSystem;
// using Systems.WorldSystem.Generator;
// using UnityEngine;
//
// namespace GamePlay.World {
//     public class WorldGenerator : MonoBehaviour, IWorldSource {
//         
//         [SerializeField] private List<WorldGenerationConfig> mapConfigs = new List<WorldGenerationConfig>();
//         [SerializeField] private int configIndex = 0;
//         [SerializeField] private bool forceGenerateNewWorld = false;
//         
//         public WorldGenerationConfig CurrentWorldConfig { get; private set; }
//
//         private List<IWorldGenerationStep> _worldGenerationSteps = new List<IWorldGenerationStep>();
//
//         private void Awake() {
//             ServiceLocator.Register<IWorldSource>(this);
//             ServiceLocator.Register(this);
//         }
//
//         // private void Start() {
//         //     _worldGenerationSteps.Add(new TilemapGenerator());
//         // }
//
//         public IEnumerator Generate(Action<WorldDTO> onDone, Action<float> onProgress, CancellationToken ct) {
//             CurrentWorldConfig = mapConfigs[configIndex];
//             WorldGenerationContext context = new (
//                 CurrentWorldConfig,
//                 new System.Random(CurrentWorldConfig.seed)
//             );
//
//             foreach (IWorldGenerationStep worldGenerationStep in _worldGenerationSteps) {
//                 if (ct.IsCancellationRequested) {
//                     yield break;
//                 }
//
//                 Debug.Log($"Running world generation step: {worldGenerationStep.GetType().Name}");
//                 yield return worldGenerationStep.GenerateStep(context, onProgress);
//                 Debug.Log($"Finished world generation step: {worldGenerationStep.GetType().Name}");
//             }
//             
//             WorldDTO worldDto = new WorldDTO(
//                 context
//             );
//             Debug.Log("World generation completed successfully.");
//             onDone?.Invoke(worldDto);
//         }
//
//         // public void Generate() {
//         //     Generate(mapConfigs[configIndex]);
//         // }
//         //
//         // private void Generate(WorldGenerationConfig config) {
//         //     CurrentWorldConfig = config;
//         //     TilemapGeneratorContext context = new TilemapGeneratorContext(
//         //         config,
//         //         new System.Random(config.seed)
//         //     );
//         //
//         //     StartCoroutine(LoadCoroutine);
//         // }
//
//         private void OnValidate() {
//             if (mapConfigs.Count == 0) {
//                 Debug.LogWarning("No Map Generation Configs assigned. Please add at least one config.");
//             }
//             if (configIndex >= mapConfigs.Count) {
//                 Debug.LogWarning($"Default World Config Index {configIndex} is out of bounds. Resetting to 0.");
//                 configIndex = 0;
//             }
//         }
//     }
// }