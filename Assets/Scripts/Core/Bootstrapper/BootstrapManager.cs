// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Core;
// using DigDeeper.ColonistSystem;
// using DigDeeper.WorldSystem;
// using Systems.ColonistSystem;
// using Systems.WorldSystem;
// using UnityEngine;
//
// namespace Core
// {
//     public class BootstrapManager : MonoBehaviour
//     {
//         public static BootstrapManager Instance { get; private set; }
//         
//         [Header("Bootstrap Configuration")]
//         [SerializeField] private bool initializeOnAwake = true;
//         [SerializeField] private bool showDebugLogs = true;
//         
//         [Header("Manager Prefabs")]
//         [SerializeField] private GameObject gameManagerPrefab;
//         [SerializeField] private GameObject eventManagerPrefab;
//         [SerializeField] private GameObject timeManagerPrefab;
//         [SerializeField] private GameObject colonistManagerPrefab;
//         [SerializeField] private GameObject taskManagerPrefab;
//         [SerializeField] private GameObject resourceManagerPrefab;
//         [SerializeField] private GameObject buildingManagerPrefab;
//         [SerializeField] private GameObject worldManagerPrefab;
//         [SerializeField] private GameObject uiManagerPrefab;
//         [SerializeField] private GameObject saveManagerPrefab;
//         [SerializeField] private GameObject tileManagerPrefab;
//         
//         [Header("Scene References")]
//         [SerializeField] private string mainMenuSceneName = "MainMenu";
//         [SerializeField] private string gameSceneName = "GameScene";
//         [SerializeField] private string loadingSceneName = "LoadingScene";
//         
//         // Initialization tracking
//         private List<ISystemManager> systemManagers = new List<ISystemManager>();
//         private bool isInitialized = false;
//         private bool isInitializing = false;
//         
//         // Events
//         public static event Action OnBootstrapComplete;
//         public static event Action<string> OnSystemInitialized;
//         
//         private void Awake()
//         {
//             if (Instance == null)
//             {
//                 Instance = this;
//                 DontDestroyOnLoad(gameObject);
//                 
//                 if (initializeOnAwake)
//                 {
//                     StartCoroutine(InitializeAllSystems());
//                 }
//             }
//             else
//             {
//                 Destroy(gameObject);
//             }
//         }
//         
//         public IEnumerator InitializeAllSystems()
//         {
//             if (isInitialized || isInitializing)
//             {
//                 DebugLog("Bootstrap already initialized or initializing");
//                 yield break;
//             }
//             
//             isInitializing = true;
//             DebugLog("Starting bootstrap initialization...");
//             
//             // // Initialize core systems in order
//             yield return InitializeSystem("EventManager", eventManagerPrefab, () => EventManager.Instance);
//             yield return InitializeSystem("GameManager", gameManagerPrefab, () => GameManager.Instance);
//             // yield return InitializeSystem("TimeManager", timeManagerPrefab, () => TimeManager.Instance);
//             // yield return InitializeSystem("SaveManager", saveManagerPrefab, () => SaveManager.Instance);
//             //
//             // // Initialize game systems
//             // yield return InitializeSystem("ResourceManager", resourceManagerPrefab, () => ResourceManager.Instance);
//             yield return InitializeSystem("WorldManager", worldManagerPrefab, () => worldManagerPrefab.GetComponent<WorldManager>());
//             yield return InitializeSystem("TileManager", tileManagerPrefab, () => tileManagerPrefab.GetComponent<TileManager>());
//             yield return InitializeSystem("ColonistManager", colonistManagerPrefab, () => ColonistManager.Instance);
//             // yield return InitializeSystem("TaskManager", taskManagerPrefab, () => TaskManager.Instance);
//             // yield return InitializeSystem("BuildingManager", buildingManagerPrefab, () => BuildingManager.Instance);
//             //
//             // // Initialize UI last
//             // yield return InitializeSystem("UIManager", uiManagerPrefab, () => UIManager.Instance);
//             
//             // Wait one frame for all systems to fully initialize
//             yield return null;
//             
//             // Initialize all system managers
//             foreach (var manager in systemManagers)
//             {
//                 if (manager != null)
//                 {
//                     manager.Initialize();
//                     DebugLog($"Initialized system: {manager.GetType().Name}");
//                     OnSystemInitialized?.Invoke(manager.GetType().Name);
//                     yield return null;
//                 }
//             }
//             
//             isInitialized = true;
//             isInitializing = false;
//             
//             DebugLog("Bootstrap initialization complete!");
//             OnBootstrapComplete?.Invoke();
//         }
//         
//         private IEnumerator InitializeSystem<T>(string systemName, GameObject prefab, Func<T> instanceGetter) where T : MonoBehaviour
//         {
//             DebugLog($"Initializing {systemName}...");
//             
//             // Check if system already exists
//             var existingInstance = instanceGetter();
//             if (existingInstance == null && prefab != null)
//             {
//                 // Create the system
//                 var systemObject = Instantiate(prefab);
//                 systemObject.name = systemName;
//                 DontDestroyOnLoad(systemObject);
//                 
//                 // Wait for the instance to be set
//                 yield return new WaitUntil(() => instanceGetter() != null);
//                 existingInstance = instanceGetter();
//             }
//             
//             // AddComponent to system managers if it implements ISystemManager
//             if (existingInstance is ISystemManager systemManager)
//             {
//                 if (!systemManagers.Contains(systemManager))
//                 {
//                     systemManagers.AddComponent(systemManager);
//                 }
//             }
//             
//             DebugLog($"{systemName} ready");
//         }
//         
//         public static void EnsureBootstrap()
//         {
//             if (Instance == null)
//             {
//                 // Look for existing bootstrap in scene
//                 var existingBootstrap = FindObjectOfType<BootstrapManager>();
//                 if (existingBootstrap != null)
//                 {
//                     return;
//                 }
//                 
//                 // Create bootstrap from resources
//                 var bootstrapPrefab = Resources.Load<GameObject>("Managers/BootstrapManager");
//                 if (bootstrapPrefab != null)
//                 {
//                     var bootstrap = Instantiate(bootstrapPrefab);
//                     bootstrap.name = "BootstrapManager";
//                 }
//                 else
//                 {
//                     // Create minimal bootstrap
//                     var bootstrapObject = new GameObject("BootstrapManager");
//                     bootstrapObject.AddComponent<BootstrapManager>();
//                 }
//             }
//         }
//         
//         public bool IsSystemReady<T>() where T : MonoBehaviour
//         {
//             return systemManagers.Exists(manager => manager is T);
//         }
//         
//         public T GetSystem<T>() where T : MonoBehaviour, ISystemManager
//         {
//             return systemManagers.Find(manager => manager is T) as T;
//         }
//         
//         private void DebugLog(string message)
//         {
//             if (showDebugLogs)
//             {
//                 Debug.Log($"[Bootstrap] {message}");
//             }
//         }
//         
//         public bool IsFullyInitialized => isInitialized;
//     }
//
// }
//
// /*
// // ===================================================================
// // SETUP INSTRUCTIONS
// // ===================================================================
//
// FOLDER STRUCTURE:
// Assets/
// ├── Resources/
// │   ├── Managers/
// │   │   └── BootstrapManager.prefab
// │   └── Config/
// │       └── GameSettings.asset
// ├── Scenes/
// │   ├── MainMenu.unity
// │   ├── LoadingScene.unity
// │   └── GameScene.unity
// └── Scripts/Core/
//     └── [All the above scripts]
//
// SETUP STEPS:
//
// 1. Create BootstrapManager Prefab:
//    - Create empty GameObject named "BootstrapManager"
//    - AddComponent BootstrapManager script
//    - Assign all manager prefabs in inspector
//    - Save as prefab in Resources/Managers/
//
// 2. Create Manager Prefabs:
//    - Create prefabs for each manager (EventManager, ColonistManager, etc.)
//    - Each should have their respective manager script attached
//
// 3. Create GameSettings Asset:
//    - Right-click → Create → DigDeeper → Game Settings
//    - Configure initial settings
//    - Save in Resources/Config/
//
// 4. Setup Scenes:
//    - MainMenu: AddComponent SceneInitializer to any GameObject
//    - LoadingScene: Create UI with LoadingScreenController
//    - GameScene: AddComponent SceneInitializer to any GameObject
//
// 5. Build Settings:
//    - AddComponent all scenes to build settings in order:
//      0: MainMenu
//      1: LoadingScene  
//      2: GameScene
//
// USAGE:
//
// Starting from any scene:
// - SceneInitializer automatically ensures all managers exist
// - BootstrapManager creates missing managers from prefabs
// - All systems initialize in proper order
// - Can start from GameScene directly for testing
//
// Loading scenes:
// ```csharp
// SceneController.Instance.LoadScene("GameScene", useLoadingScreen: true);
// ```
//
// Game state management:
// ```csharp
// GameManager.Instance.StartNewGame();
// GameManager.Instance.PauseGame();
// GameManager.Instance.SaveGame("save1");
// ```
//
// The system is bulletproof - you can start from ANY scene and it will work!
// */