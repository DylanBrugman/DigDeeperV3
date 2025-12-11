// using System;
// using System.Collections.Generic;
// using Core;
// using DigDeeper.WorldSystem;
// using Systems.ColonistSystem;
// using Systems.WorldSystem.Generator;
// using UnityEngine;
//
// namespace Systems.WorldSystem
// {
//     public class WorldManager : MonoBehaviour
//     {
//         private static WorldManager _instance;
//         public static WorldManager Instance {
//             get {
//                 if (_instance == null) {
//                     _instance = FindObjectOfType<WorldManager>();
//                     if (_instance == null) {
//                         GameObject obj = new GameObject("WorldManager");
//                         _instance = obj.AddComponent<WorldManager>();
//                     }
//                 }
//                 return _instance;
//             }
//         }
//         
//         [Header("References")]
//         [SerializeField] private TileManager tileManager;
//         
//         [Header("ECSSystem Manager Configuration")]
//         [SerializeField] private List<WorldGenerationConfig> worldConfigs = new List<WorldGenerationConfig>();
//         [SerializeField] private int defaultWorldConfigIndex = 0;
//         [SerializeField] private bool forceGenerateNewWorld = false;
//         
//         // Properties
//         public bool IsInitialized { get; private set; }
//         public WorldGenerationConfig CurrentWorldConfig { get; private set; }
//         public TileManager TileManager => tileManager;
//
//         private Tile[,] tileWorld;
//         
//         // Events
//         public static event Action OnWorldInitialized;
//         
//         private void Awake() {
//             if (_instance == null) {
//                 _instance = this;
//                 DontDestroyOnLoad(gameObject);
//             } else if (_instance != this) {
//                 Destroy(gameObject);
//             }
//         }
//         
//         public void Initialize()
//         {
//             if (IsInitialized) return;
//             
//             // Set default ecsWorld config
//             if (worldConfigs.Count > 0 && defaultWorldConfigIndex < worldConfigs.Count)
//             {
//                 CurrentWorldConfig = worldConfigs[defaultWorldConfigIndex];
//             }
//             
//             IsInitialized = true;
//             OnWorldInitialized?.Invoke();
//         }
//         
//         public void UpdateSystem()
//         {
//         }
//         
//         public void CleanupSystem()
//         {
//         }
//
//         [ContextMenu("GenerateStep ECSSystem")]
//         public void GenerateWorld() {
//             CurrentWorldConfig = worldConfigs[defaultWorldConfigIndex];
//
//             if (tileWorld == null || forceGenerateNewWorld) {
//                 tileWorld = tileManager.GenerateTilemap(CurrentWorldConfig, forceGenerateNewWorld);                
//             }
//             else {
//                 Debug.LogWarning("ECSSystem already generated! Use forceGenerateNewWorld to regenerate.");
//             }
//         }
//
//         private void OnValidate() {
//             // Validate ecsWorld configs
//             if (worldConfigs == null || worldConfigs.Count == 0)
//             {
//                 Debug.LogWarning("WorldManager has no ecsWorld generation configs assigned!");
//             }
//             else if (defaultWorldConfigIndex < 0 || defaultWorldConfigIndex >= worldConfigs.Count)
//             {
//                 Debug.LogWarning($"Default ecsWorld config index {defaultWorldConfigIndex} is out of bounds! Resetting to 0.");
//                 defaultWorldConfigIndex = 0;
//             }
//         }
//     }
// }
//
// /*
// // ===================================================================
// // SETUP INSTRUCTIONS & USAGE
// // ===================================================================
//
// SETUP STEPS:
//
// 1. Create ECSSystem Generation Config:
//    - Right-click → Create → DigDeeper → ECSSystem Generation Config
//    - Configure ecsWorld size, layers, biomes, resources, and POIs
//    - Save in Resources/Config/
//
// 2. Create Tile Prefab:
//    - Create empty GameObject named "Tile"
//    - AddComponent SpriteRenderer component
//    - AddComponent TileComponent script
//    - Save as prefab
//
// 3. Create Tile Sprites:
//    - Create sprites for each TileType (Air, Grass, Dirt, Stone, etc.)
//    - Name them exactly as the enum values
//    - Place in Resources/Sprites/NativeTileGrid/
//
// 4. Setup Manager Hierarchy:
//    - WorldManager (contains TileManager)
//    - TileManager (handles ecsWorld grid and visual tiles)
//
// USAGE EXAMPLES:
//
// GenerateStep ECSSystem:
// ```csharp
// WorldManager.Instance.GenerateTilemap();
// ```
//
// Access NativeTileGrid:
// ```csharp
// var tile = TileManager.Instance.GetTile(new Vector2Int(10, 5));
// var resources = tile.resources;
// ```
//
// Dig Tile:
// ```csharp
// bool success = TileManager.Instance.DigTile(position, digPower);
// ```
//
// Extract Resources:
// ```csharp
// var extracted = TileManager.Instance.ExtractResources(position, extractionPower);
// ```
//
// FEATURES:
//
// ✅ Multi-layered earth structure (surface → core)
// ✅ Multiple resources per tile with quality/abundance
// ✅ Procedural generation with configurable parameters
// ✅ Points of Interest system
// ✅ Biome-based generation
// ✅ Structural integrity and collapse simulation
// ✅ Resource clustering and distribution
// ✅ Visual tile system with resource indicators
// ✅ Save/Load support
// ✅ Performance optimized with chunk system
//
// The system is fully modular and config-driven through ScriptableObjects!
// */