// using System;
// using System.Collections.Generic;
// using DigDeeper.ColonistSystem;
// using Systems.ColonistSystem;
// using Systems.NeedsSystem;
// using Systems.WorldSystem;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// public class ColonistManager : MonoBehaviour {
//     
//     [Header("References")]
//     [SerializeField] private WorldManager worldManager;
//     
//     [SerializeField] private List<ColonistConfig> availableColonistTypes = new List<ColonistConfig>();
//     [SerializeField] private GameObject colonistPrefab;
//     [SerializeField] private Transform colonistParent;
//     
//     private readonly List<Colonist> allColonists = new List<Colonist>();
//     private Dictionary<long, Colonist> _colonistLookup = new Dictionary<long, Colonist>();
//     private long _nextColonistId = 0;
//     
//     public static event Action<Colonist> OnColonistSpawned;
//     public static event Action<Colonist> OnColonistRemoved;
//
//     public void Spawn(ColonistConfig config, Vector2Int position) {
//         TileManager tileManager = worldManager.TileManager;
//         if (tileManager == null) {
//             Debug.LogError("TileManager is not set in WorldManager!");
//             return;
//         }
//
//         if (tileManager.AreTilesOccupied(position, config.height)) {
//             Debug.LogWarning($"Cannot spawn colonistController at {position} position, because any tile within height {config.height} is already occupied!");
//             return;
//         }
//         
//         GameObject colonistObject = Instantiate(colonistPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity, colonistParent);
//         ColonistController colonistController = colonistObject.GetComponent<ColonistController>();
//         String colonistName = RandomColonistName();
//         long colonistId = _nextColonistId++;
//         Colonist colonist = new Colonist(colonistId, config, colonistName);
//         colonistController.Initialize(colonist);
//         Debug.Log( $"Spawned colonistController {colonistName} at {position}");
//         
//         allColonists.Add(colonist);
//         OnColonistSpawned?.Invoke(colonist);
//     }
//     
//     private String RandomColonistName() {
//         // This method should return a random colonist name from a predefined list or generate one
//         // For simplicity, we can return a placeholder name here
//         return "Colonist_" + Random.Range(1, 1000);
//     }
// }
//
// // using System;
// // using System.Collections.Generic;
// // using System.Linq;
// // using Core;
// // using DigDeeper.ColonistSystem;
// // using DigDeeper.TaskSystem;
// // using UnityEngine;
// //
// // namespace Systems.ColonistSystem
// // {
// //     public class ColonistManager : MonoBehaviour
// //     {
// //         [Header("ColonistController Management")]
// //         [SerializeField] private List<ColonistConfig> availableColonistTypes = new List<ColonistConfig>();
// //         [SerializeField] private GameObject colonistPrefab;
// //         [SerializeField] private TransformComponent colonistParent;
// //         [SerializeField] private int maxColonists = 50;
// //         
// //         // ColonistController tracking
// //         private List<ColonistController> allColonists = new List<ColonistController>();
// //         private Dictionary<int, ColonistController> colonistLookup = new Dictionary<int, ColonistController>();
// //         private Queue<ColonistController> availableColonists = new Queue<ColonistController>();
// //         
// //         // Events
// //         public event Action<ColonistController> OnColonistAdded;
// //         public event Action<ColonistController> OnColonistRemoved;
// //         
// //         // Properties
// //         public int ColonistCount => allColonists.Count;
// //         public int AvailableColonistCount => availableColonists.Count;
// //         public List<ColonistController> AllColonists => new List<ColonistController>(allColonists);
// //         
// //         public ColonistController SpawnColonist(Vector2Int position, ColonistConfig config = null)
// //         {
// //             if (allColonists.Count >= maxColonists)
// //             {
// //                 Debug.LogWarning("Maximum colonist limit reached!");
// //                 return null;
// //             }
// //             
// //             if (config == null && availableColonistTypes.Count > 0)
// //             {
// //                 config = availableColonistTypes[UnityEngine.Random.Range(0, availableColonistTypes.Count)];
// //             }
// //             
// //             if (config == null)
// //             {
// //                 Debug.LogError("No colonist config available for spawning!");
// //                 return null;
// //             }
// //             
// //             var colonistObject = Instantiate(colonistPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity, colonistParent);
// //             var colonist = colonistObject.GetComponent<ColonistController>();
// //             
// //             if (colonist != null)
// //             {
// //                 colonist.Initialize(config);
// //                 return colonist;
// //             }
// //             
// //             Debug.LogError("Spawned colonist prefab does not have ColonistController component!");
// //             Destroy(colonistObject);
// //             return null;
// //         }
// //         
// //         private void RegisterColonist(ColonistController colonist)
// //         {
// //             if (!allColonists.Contains(colonist))
// //             {
// //                 allColonists.AddComponent(colonist);
// //                 colonistLookup[colonist.ID] = colonist;
// //                 availableColonists.Enqueue(colonist);
// //                 
// //                 OnColonistAdded?.Invoke(colonist);
// //             }
// //         }
// //         
// //         private void HandleColonistDeath(ColonistController colonist)
// //         {
// //             RemoveColonist(colonist);
// //         }
// //         
// //         private void RemoveColonist(ColonistController colonist)
// //         {
// //             if (allColonists.Contains(colonist))
// //             {
// //                 allColonists.Remove(colonist);
// //                 colonistLookup.Remove(colonist.ID);
// //                 
// //                 // Remove from available queue if present
// //                 var tempQueue = new Queue<ColonistController>();
// //                 while (availableColonists.Count > 0)
// //                 {
// //                     var c = availableColonists.Dequeue();
// //                     if (c != colonist)
// //                     {
// //                         tempQueue.Enqueue(c);
// //                     }
// //                 }
// //                 availableColonists = tempQueue;
// //                 
// //                 OnColonistRemoved?.Invoke(colonist);
// //             }
// //         }
// //         
// //         public ColonistController GetAvailableColonist()
// //         {
// //             while (availableColonists.Count > 0)
// //             {
// //                 var colonist = availableColonists.Dequeue();
// //                 if (colonist != null && !colonist.HasAssignedTask())
// //                 {
// //                     return colonist;
// //                 }
// //             }
// //             return null;
// //         }
// //         
// //         public void ReturnColonistToAvailable(ColonistController colonist)
// //         {
// //             if (colonist != null && !colonist.HasAssignedTask())
// //             {
// //                 availableColonists.Enqueue(colonist);
// //             }
// //         }
// //         
// //         private void HandleTaskCompleted(ITask task)
// //         {
// //             var colonist = task.GetAssignedColonist();
// //             if (colonist != null)
// //             {
// //                 ReturnColonistToAvailable(colonist);
// //             }
// //         }
// //         
// //         public ColonistController GetColonistById(int id)
// //         {
// //             return colonistLookup.ContainsKey(id) ? colonistLookup[id] : null;
// //         }
// //         
// //         public List<ColonistController> GetColonistsWithSkill(SkillType skillType, float minLevel = 0f)
// //         {
// //             return allColonists.Where(c => c.Stats.GetSkillLevel(skillType) >= minLevel).ToList();
// //         }
// //         
// //         public ColonistController GetBestColonistForTask(ITask task)
// //         {
// //             var availableColonists = allColonists.Where(c => !c.HasAssignedTask() && c.CanPerformTask(task)).ToList();
// //             
// //             if (availableColonists.Count == 0) return null;
// //             
// //             // Find the colonist with the highest efficiency for this task
// //             return availableColonists.OrderByDescending(c => c.GetTaskEfficiency(task)).First();
// //         }
// //         
// //         public List<ColonistController> GetColonistsInRadius(Vector2Int center, float radius)
// //         {
// //             return allColonists.Where(c => Vector2.Distance(c.PositionComponent, center) <= radius).ToList();
// //         }
// //         
// //         public void UpdateAllColonists()
// //         {
// //             // This method can be called for any global colonist updates
// //             foreach (var colonist in allColonists)
// //             {
// //                 if (colonist != null)
// //                 {
// //                     // Perform any necessary updates
// //                     // This is handled by individual colonist components in Update()
// //                 }
// //             }
// //         }
// //         
// //         public Dictionary<string, object> GetSaveData()
// //         {
// //             var colonist = new List<Dictionary<string, object>>();
// //             foreach (var colonist in allColonists)
// //             {
// //                 if (colonist != null)
// //                 {
// //                     colonist.AddComponent(colonist.GetSaveData());
// //                 }
// //             }
// //             
// //             return new Dictionary<string, object>
// //             {
// //                 ["colonists"] = colonist,
// //                 ["maxColonists"] = maxColonists
// //             };
// //         }
// //         
// //         public void LoadFromSaveData(Dictionary<string, object> data)
// //         {
// //             // Clear existing colonists
// //             foreach (var colonist in allColonists.ToList())
// //             {
// //                 if (colonist != null)
// //                 {
// //                     Destroy(colonist.gameObject);
// //                 }
// //             }
// //             allColonists.Clear();
// //             colonistLookup.Clear();
// //             availableColonists.Clear();
// //             
// //             // Load colonists from save config
// //             if (data.ContainsKey("colonists") && data["colonists"] is List<Dictionary<string, object>> colonistDataList)
// //             {
// //                 foreach (var colonist in colonistDataList)
// //                 {
// //                     LoadColonistFromData(colonist);
// //                 }
// //             }
// //             
// //             if (data.ContainsKey("maxColonists"))
// //             {
// //                 maxColonists = (int)data["maxColonists"];
// //             }
// //         }
// //         
// //         private void LoadColonistFromData(Dictionary<string, object> data)
// //         {
// //             // Extract position
// //             Vector2Int position = Vector2Int.zero;
// //             if (data.ContainsKey("position"))
// //             {
// //                 position = (Vector2Int)data["position"];
// //             }
// //             
// //             // Create colonist (this is simplified - in production you'd match the correct ColonistConfig)
// //             var colonist = SpawnColonist(position, availableColonistTypes.FirstOrDefault());
// //             if (colonist != null)
// //             {
// //                 colonist.LoadFromSaveData(data);
// //             }
// //         }
// //         
// //         private void OnDestroy()
// //         {
// //             if (EventManager.Instance != null)
// //             {
// //                 EventManager.Instance.OnColonistSpawned -= RegisterColonist;
// //                 EventManager.Instance.OnColonistDied -= HandleColonistDeath;
// //                 EventManager.Instance.OnTaskCompleted -= HandleTaskCompleted;
// //             }
// //         }
// //     }
// // }
// // // ===================================================================
// // // USAGE EXAMPLE
// // // ===================================================================
// //
// // /*
// // USAGE EXAMPLE:
// //
// // 1. Setup in Scene:
// //    - Create empty GameObject named "ColonistManager"
// //    - Attach ColonistManager script
// //    - Create colonist prefab with ColonistController, Colonist, ColonistNeeds, ColonistAISystem components
// //    - Assign prefab to ColonistManager
// //
// // 2. Create ColonistConfig assets:
// //    - Right-click in Project → Create → DigDeeper → ColonistController Config
// //    - Configure starting stats, skills, and needs
// //
// // 3. Spawn colonists:
// //    ```csharp
// //    var colonist = ColonistManager.Instance.SpawnColonist(new Vector2Int(0, 0), _colonistConfig);
// //    ```
// //
// // 4. Assign tasks:
// //    ```csharp
// //    var availableColonist = ColonistManager.Instance.GetAvailableColonist();
// //    if (availableColonist != null)
// //    {
// //        availableColonist.AssignTask(someTask);
// //    }
// //    ```
// //
// // 5. Monitor colonist state:
// //    ```csharp
// //    colonist.AI.OnStateChanged += (state) => Debug.Log($"ColonistController state changed to: {state}");
// //    colonist.Stats.OnHealthChanged += (health) => Debug.Log($"Health: {health}");
// //    ```
// //
// // INTEGRATION NOTES:
// //
// // - This system integrates with the TaskSystem through the ITask interface
// // - Events are managed through EventManager for loose coupling
// // - Save/Load functionality is built-in for persistence
// // - Performance optimized with object pooling and efficient config structures
// // - Extensible design allows easy addition of new needs, skills, and AI behaviors
// //
// // NEXT STEPS:
// //
// // 1. Implement TaskSystem that works with this colonist system
// // 2. Create pathfinding system for AI movement
// // 3. Implement building system for colonist interactions
// // 4. AddComponent visual feedback and UI panels for colonist management
// // */