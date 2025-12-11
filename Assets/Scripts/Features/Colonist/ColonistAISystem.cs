// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Systems.ColonistSystem {
//     
//     //Brain of the colonist, handles decision making and task management
//     public class ColonistAISystem : MonoBehaviour {
//         
//         // Components
//         private ColonistController _colonistController;
//         
//         [Header("AI Configuration")]
//         public float requestJobInterval = 1f;
//         
//         private List<Colonist> _colonists = new List<Colonist>();
//
//         private void OnEnable() {
//             ColonistManager.OnColonistSpawned += OnColonistSpawned;
//         }
//
//         private void OnDisable() {
//             ColonistManager.OnColonistSpawned -= OnColonistSpawned;
//         }
//
//         private void OnColonistSpawned(Colonist colonist) {
//             _colonists.Add(colonist);
//         }
//
//         private void Update() {
//             
//         }
//     }
// }
//
// // using System;
// // using System.Collections.Generic;
// // using DigDeeper.ColonistSystem;
// // using UnityEngine;
// //
// // namespace Systems.ColonistSystem {
// //     public class ColonistAISystem : MonoBehaviour
// //     {
// //         [Header("AI Configuration")]
// //         [SerializeField] private float pathfindingUpdateInterval = 0.5f;
// //         
// //         // Components
// //         private ColonistController _colonist;
// //         private Colonist _data;
// //         
// //         // Timers
// //         private float lastDecisionTime;
// //         private float lastPathUpdate;
// //         
// //         // Events
// //         public event Action<ColonistState> OnStateChanged;
// //         
// //         private void Awake()
// //         {
// //             _data = GetComponent<Colonist>();
// //         }
// //         
// //         private void Start()
// //         {
// //             // Subscribe to need events
// //             _colonist.OnNeedBecameCritical += HandleCriticalNeed;
// //         }
// //         
// //         private void Update()
// //         {
// //             UpdateDecisionMaking();
// //             UpdateMovement();
// //             UpdatePathfinding();
// //         }
// //         
// //         private void UpdateDecisionMaking()
// //         {
// //             if (Time.time - lastDecisionTime >= decisionInterval)
// //             {
// //                 MakeDecision();
// //                 lastDecisionTime = Time.time;
// //             }
// //         }
// //         
// //         private void MakeDecision()
// //         {
// //             // Priority decision making
// //             if (_data.CurrentHealth <= 20f)
// //             {
// //                 ChangeState(ColonistState.Fleeing);
// //                 return;
// //             }
// //             
// //             if (needs.HasCriticalNeeds)
// //             {
// //                 HandleCriticalNeeds();
// //                 return;
// //             }
// //             
// //             if (colonist.HasAssignedTask())
// //             {
// //                 if (currentState == ColonistState.Idle)
// //                 {
// //                     ChangeState(ColonistState.MovingToTask);
// //                 }
// //             }
// //             else
// //             {
// //                 if (currentState != ColonistState.Idle)
// //                 {
// //                     ChangeState(ColonistState.Idle);
// //                 }
// //             }
// //         }
// //         
// //         private void HandleCriticalNeeds()
// //         {
// //             var criticalNeed = needs.GetMostCriticalNeed();
// //             if (criticalNeed != null)
// //             {
// //                 switch (criticalNeed.type)
// //                 {
// //                     case NeedType.Hunger:
// //                         ChangeState(ColonistState.Eating);
// //                         break;
// //                     case NeedType.Energy:
// //                         ChangeState(ColonistState.Sleeping);
// //                         break;
// //                     default:
// //                         ChangeState(ColonistState.Resting);
// //                         break;
// //                 }
// //             }
// //         }
// //         
// //         private void HandleCriticalNeed(NeedConfig need)
// //         {
// //             // Interrupt current task if need becomes critical
// //             if (colonist.HasAssignedTask() && currentState == ColonistState.ExecutingTask)
// //             {
// //                 colonist.InterruptCurrentTask();
// //             }
// //         }
// //         
// //         private void UpdateMovement()
// //         {
// //             if (currentState == ColonistState.MovingToTask && currentPath.Count > 0)
// //             {
// //                 MoveAlongPath();
// //             }
// //         }
// //         
// //         private void UpdatePathfinding()
// //         {
// //             if (Time.time - lastPathUpdate >= pathfindingUpdateInterval)
// //             {
// //                 if (currentState == ColonistState.MovingToTask && colonist.HasAssignedTask())
// //                 {
// //                     var task = colonist.GetCurrentTask();
// //                     if (task != null)
// //                     {
// //                         FindPathTo(task.GetTargetPosition());
// //                     }
// //                 }
// //                 lastPathUpdate = Time.time;
// //             }
// //         }
// //         
// //         private void MoveAlongPath()
// //         {
// //             if (currentPath.Count == 0) return;
// //             
// //             var nextPosition = currentPath.Peek();
// //             var worldPosition = new Vector3(nextPosition.x, nextPosition.y, 0);
// //             
// //             // Move towards next position
// //             transform.position = Vector3.MoveTowards(transform.position, worldPosition, _data.Speed * Time.deltaTime);
// //             
// //             // Check if reached next position
// //             if (Vector3.Distance(transform.position, worldPosition) < 0.1f)
// //             {
// //                 currentPath.Dequeue();
// //                 
// //                 // If reached final destination
// //                 if (currentPath.Count == 0)
// //                 {
// //                     ChangeState(ColonistState.ExecutingTask);
// //                 }
// //             }
// //         }
// //         
// //         private void FindPathTo(Vector2Int target)
// //         {
// //             if (target == currentTarget) return;
// //             
// //             currentTarget = target;
// //             
// //             // Simple pathfinding - in production, use A* or similar
// //             var currentPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
// //             currentPath.Clear();
// //             
// //             // For now, simple direct path (replace with proper pathfinding)
// //             var direction = target - currentPos;
// //             var steps = Mathf.MaxVelocity(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
// //             
// //             for (int i = 1; i <= steps; i++)
// //             {
// //                 var stepX = currentPos.x + Mathf.RoundToInt((float)direction.x / steps * i);
// //                 var stepY = currentPos.y + Mathf.RoundToInt((float)direction.y / steps * i);
// //                 currentPath.Enqueue(new Vector2Int(stepX, stepY));
// //             }
// //         }
// //         
// //         public void ChangeState(ColonistState newState)
// //         {
// //             if (currentState == newState) return;
// //             
// //             ExitState(currentState);
// //             currentState = newState;
// //             EnterState(newState);
// //             
// //             OnStateChanged?.Invoke(newState);
// //         }
// //         
// //         private void EnterState(ColonistState state)
// //         {
// //             switch (state)
// //             {
// //                 case ColonistState.ExecutingTask:
// //                     colonist.StartExecutingCurrentTask();
// //                     break;
// //                 case ColonistState.Eating:
// //                     StartEating();
// //                     break;
// //                 case ColonistState.Sleeping:
// //                     StartSleeping();
// //                     break;
// //                 case ColonistState.Resting:
// //                     StartResting();
// //                     break;
// //             }
// //         }
// //         
// //         private void ExitState(ColonistState state)
// //         {
// //             switch (state)
// //             {
// //                 case ColonistState.ExecutingTask:
// //                     colonist.StopExecutingCurrentTask();
// //                     break;
// //                 case ColonistState.Eating:
// //                     StopEating();
// //                     break;
// //                 case ColonistState.Sleeping:
// //                     StopSleeping();
// //                     break;
// //                 case ColonistState.Resting:
// //                     StopResting();
// //                     break;
// //             }
// //         }
// //         
// //         private void StartEating()
// //         {
// //             // Find nearest food source and consume
// //             needs.SatisfyNeed(NeedType.Hunger, 50f * Time.deltaTime);
// //         }
// //         
// //         private void StopEating() { }
// //         
// //         private void StartSleeping()
// //         {
// //             // Find bed or sleeping area
// //             needs.SatisfyNeed(NeedType.Energy, 30f * Time.deltaTime);
// //         }
// //         
// //         private void StopSleeping() { }
// //         
// //         private void StartResting()
// //         {
// //             // Generic rest activity
// //             needs.SatisfyNeed(NeedType.Comfort, 20f * Time.deltaTime);
// //         }
// //         
// //         private void StopResting() { }
// //         
// //         public ColonistState GetCurrentState() => currentState;
// //         
// //         private void OnDestroy()
// //         {
// //             if (needs != null)
// //             {
// //                 needs.OnNeedBecameCritical -= HandleCriticalNeed;
// //             }
// //         }
// //     }
// // }