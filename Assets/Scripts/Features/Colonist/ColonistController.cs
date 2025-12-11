// using System;
// using UnityEngine;
// using Systems.MovementSystem;
// using Systems.PathfindingSystem;
// using Systems.JobSystem;
//
// namespace Systems.ColonistSystem
// {
//     public sealed class ColonistController : MonoBehaviour
//     {
//         /* -------- inspector wires -------- */
//         public Transform spriteRoot;           // assign sprite pivot or leave null
//
//         /* -------- runtime refs ----------- */
//         public Colonist Colonist { get; private set; }
//
//         /* -------- internal caches -------- */
//         Vector2 _tmpGoal;                      // wander goal
//         float   _hunger;                       // placeholder need
//
//         /* ====================================================================== */
//         #region Unity hooks
//         public void Initialize(Colonist col)   => Colonist = col;
//
//         void Update()
//         {
//             float dt = Time.deltaTime;
//
//             TickAI(dt);
//             TickPathing(dt);
//             TickMovement(dt);
//             TickNeeds(dt);
//             ApplyVisuals();
//         }
//         #endregion
//         /* ====================================================================== */
//
//         /* ------------------------------ AI ----------------------------------- */
//         void TickAI(float dt)
//         {
//             switch (Colonist.AIState)
//             {
//                 case AIState.Dead:
//                     Colonist.Movement.PathfindingPath.Path.Clear();
//                     Colonist.Movement.VelocityComponent = Vector2.zero;
//                     Colonist.CurrentJob?.Interrupt();
//                     Colonist.CurrentJob = null;
//                     return;
//
//                 case AIState.Idle:
//                     if (Colonist.CurrentJob == null)
//                         _tmpGoal = PickRandomPointNearHome();
//                     else
//                         Colonist.AIState = AIState.Working;
//                     break;
//
//                 case AIState.Working:
//                     if (Colonist.CurrentJob == null)
//                         Colonist.AIState = AIState.Idle;
//                     else {
//                         Colonist.CurrentJob.GenerateStep(dt);
//                     }
//                     break;
//             }
//
//             /* if idle OR job wants to move, request a path once */
//             if (!Colonist.Movement.PathfindingPath.PathRequested)
//             {
//                 Vector2 dest = Colonist.CurrentJob?.TargetDestination ?? _tmpGoal;
//                 RequestPath(dest);
//             }
//         }
//
//         /* -------------------------- Path-finding ------------------------------ */
//         void RequestPath(Vector2 goal)
//         {
//             var path = Colonist.Movement.PathfindingPath;
//             path.Path.Clear();
//             path.Path.Enqueue(goal);           // MVP = straight line
//             path.PathRequested = true;
//         }
//
//         void TickPathing(float dt)
//         {
//             var move = Colonist.Movement;
//             var queue= move.PathfindingPath.Path;
//
//             if (queue.Count == 0) return;
//
//             if ((move.PositionComponent - queue.Peek()).sqrMagnitude < 0.05f)
//                 queue.Dequeue();
//         }
//
//         /* --------------------------- Movement -------------------------------- */
//         void TickMovement(float dt)
//         {
//             var m   = Colonist.Movement;
//             var path= m.PathfindingPath.Path;
//
//             /* steer */
//             if (path.Count > 0)
//             {
//                 Vector2 dir = (path.Peek() - m.PositionComponent).normalized;
//                 float velX = Mathf.MoveTowards(m.VelocityComponent.x,
//                                   dir.x * m.MaxVelocity, 40 * dt);
//
//                 float velY = m.VelocityComponent.y;
//                 if (dir.y > 0.5f && m.MaxJumpHeight > 0 && m.IsGrounded)
//                     velY = Mathf.Sqrt(2 * m.MaxJumpHeight * Mathf.Abs(Physics2D.gravity.y));
//                 m.VelocityComponent.Set(velX, velY);
//             }
//
//             /* physics */
//             m.VelocityComponent += Physics2D.gravity * dt;
//             m.PositionComponent += m.VelocityComponent * dt;
//             
//             m.IsGrounded = m.PositionComponent.y <= 0.01f;
//         }
//
//         /* ----------------------------- NeedConfig --------------------------------- */
//         void TickNeeds(float dt)
//         {
//             _hunger += dt * 2f;
//             if (_hunger > 100f) Die();
//         }
//
//         /* --------------------------- Visuals --------------------------------- */
//         void ApplyVisuals()
//         {
//             transform.position = Colonist.Movement.PositionComponent;
//             if (spriteRoot)
//                 spriteRoot.localScale = new Vector3(
//                     Colonist.Movement.VelocityComponent.x >= 0 ? 1 : -1, 1, 1);
//         }
//         
//         Vector2 PickRandomPointNearHome() => (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * 5f;
//         void    Die() => Destroy(gameObject);
//
//         public Job  GetCurrentJob()           => Colonist.CurrentJob;
//         public bool CanExecute(Job job)
//         {
//             foreach (var r in job.Requirements)
//                 if (!r.IsMet(Colonist)) return false;
//             return true;
//         }
//         public void  AssignJob(Job job)       => Colonist.CurrentJob = job;
//     }
// }
// // using System;
// // using System.Collections.Generic;
// // using Core;
// // using DigDeeper.TaskSystem;
// // using UnityEngine;
// //
// // namespace DigDeeper.ColonistSystem {
// //     public class ColonistController : MonoBehaviour
// //     {
// //         [Header("ColonistController Info")]
// //         [SerializeField] private ColonistConfig _colonistConfig;
// //         [SerializeField] private string colonistName;
// //         [SerializeField] private int colonistId;
// //         
// //         // Components
// //         public Colonist Stats { get; private set; }
// //         public ColonistNeeds NeedConfig { get; private set; }
// //         public ColonistAISystem AI { get; private set; }
// //         
// //         // Task Management
// //         private ITask currentTask;
// //         private bool isExecutingTask;
// //         
// //         // Properties
// //         public string Name => colonistName;
// //         public int ID => colonistId;
// //         public ColonistConfig Config => _colonistConfig;
// //         public Vector2Int PositionComponent => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
// //         
// //         // Events
// //         public event Action<ColonistController> OnColonistDied;
// //         public event Action<ITask> OnTaskCompleted;
// //         public event Action<ITask> OnTaskFailed;
// //         
// //         private void Awake()
// //         {
// //             InitializeComponents();
// //         }
// //         
// //         private void Start()
// //         {
// //             if (_colonistConfig != null)
// //             {
// //                 Initialize(_colonistConfig);
// //             }
// //         }
// //         
// //         private void InitializeComponents()
// //         {
// //             Stats = GetComponent<Colonist>();
// //             NeedConfig = GetComponent<ColonistNeeds>();
// //             AI = GetComponent<ColonistAISystem>();
// //             
// //             if (Stats == null) Stats = gameObject.AddComponent<Colonist>();
// //             if (NeedConfig == null) NeedConfig = gameObject.AddComponent<ColonistNeeds>();
// //             if (AI == null) AI = gameObject.AddComponent<ColonistAISystem>();
// //         }
// //         
// //         public void Initialize(Colonist colonist)
// //         {
// //             _colonistConfig = config;
// //             colonistName = config.colonistName;
// //             colonistId = UnityEngine.Random.Range(1000, 9999); // GenerateStep unique ID
// //             
// //             Stats.Initialize(config);
// //             NeedConfig.Initialize(config.baseNeeds);
// //             
// //             // Subscribe to events
// //             Stats.OnHealthChanged += OnHealthChanged;
// //             
// //             EventManager.Instance?.InvokeOnColonistSpawned(this);
// //         }
// //         
// //         public void AssignTask(ITask task)
// //         {
// //             if (currentTask != null && isExecutingTask)
// //             {
// //                 InterruptCurrentTask();
// //             }
// //             
// //             currentTask = task;
// //             task.AssignToColonist(this);
// //         }
// //         
// //         public void StartExecutingCurrentTask()
// //         {
// //             if (currentTask != null && !isExecutingTask)
// //             {
// //                 isExecutingTask = true;
// //                 currentTask.StartExecution();
// //             }
// //         }
// //         
// //         public void StopExecutingCurrentTask()
// //         {
// //             if (currentTask != null && isExecutingTask)
// //             {
// //                 isExecutingTask = false;
// //                 currentTask.StopExecution();
// //             }
// //         }
// //         
// //         public void InterruptCurrentTask()
// //         {
// //             if (currentTask != null)
// //             {
// //                 StopExecutingCurrentTask();
// //                 currentTask.OnInterrupted();
// //                 currentTask = null;
// //             }
// //         }
// //         
// //         public void CompleteCurrentTask()
// //         {
// //             if (currentTask != null)
// //             {
// //                 var completedTask = currentTask;
// //                 StopExecutingCurrentTask();
// //                 currentTask = null;
// //                 
// //                 OnTaskCompleted?.Invoke(completedTask);
// //                 EventManager.Instance?.InvokeOnTaskCompleted(completedTask);
// //             }
// //         }
// //         
// //         public void FailCurrentTask(string reason)
// //         {
// //             if (currentTask != null)
// //             {
// //                 var failedTask = currentTask;
// //                 StopExecutingCurrentTask();
// //                 currentTask = null;
// //                 
// //                 OnTaskFailed?.Invoke(failedTask);
// //                 EventManager.Instance?.InvokeOnTaskFailed(failedTask, reason);
// //             }
// //         }
// //         
// //         public bool HasAssignedTask() => currentTask != null;
// //         public ITask GetCurrentTask() => currentTask;
// //         public bool IsExecutingTask() => isExecutingTask;
// //         
// //         public bool CanPerformTask(ITask task)
// //         {
// //             if (Stats.Health <= 0f) return false;
// //             if (AI.GetCurrentState() == ColonistState.Incapacitated) return false;
// //             
// //             // Check if colonist has required skills
// //             foreach (var requiredSkill in task.GetRequiredSkills())
// //             {
// //                 if (Stats.GetSkillLevel(requiredSkill.Key) < requiredSkill.InitialValue)
// //                 {
// //                     return false;
// //                 }
// //             }
// //             
// //             return true;
// //         }
// //         
// //         public float GetTaskEfficiency(ITask task)
// //         {
// //             float efficiency = 1f;
// //             
// //             // Calculate efficiency based on skills
// //             foreach (var requiredSkill in task.GetRequiredSkills())
// //             {
// //                 var skillLevel = Stats.GetSkillLevel(requiredSkill.Key);
// //                 var skillEfficiency = Mathf.Clamp01(skillLevel / 100f);
// //                 efficiency *= (0.5f + skillEfficiency * 0.5f); // 50% base + 50% skill-based
// //             }
// //             
// //             // Factor in health and stamina
// //             efficiency *= Stats.HealthPercentage;
// //             efficiency *= Mathf.Clamp01(Stats.StaminaPercentage + 0.2f); // Minimum 20% efficiency
// //             
// //             return efficiency;
// //         }
// //         
// //         private void OnHealthChanged(float newHealth)
// //         {
// //             if (newHealth <= 0f)
// //             {
// //                 Die();
// //             }
// //         }
// //         
// //         private void Die()
// //         {
// //             AI.ChangeState(ColonistState.Incapacitated);
// //             InterruptCurrentTask();
// //             OnColonistDied?.Invoke(this);
// //             EventManager.Instance?.InvokeOnColonistDied(this);
// //         }
// //         
// //         public Dictionary<string, object> GetSaveData()
// //         {
// //             return new Dictionary<string, object>
// //             {
// //                 ["name"] = colonistName,
// //                 ["id"] = colonistId,
// //                 ["position"] = PositionComponent,
// //                 ["health"] = Stats.Health,
// //                 ["stamina"] = Stats.Stamina,
// //                 ["skills"] = Stats.GetAllSkills(),
// //                 ["needs"] = NeedConfig.GetAllNeeds(),
// //                 ["state"] = AI.GetCurrentState()
// //             };
// //         }
// //         
// //         public void LoadFromSaveData(Dictionary<string, object> data)
// //         {
// //             // Implementation for loading colonist state from save config
// //             // This would restore position, stats, skills, needs, etc.
// //         }
// //         
// //         private void OnDestroy()
// //         {
// //             if (Stats != null)
// //             {
// //                 Stats.OnHealthChanged -= OnHealthChanged;
// //             }
// //         }
// //     }
// // }