// using System;
// using System.Collections.Generic;
// using Core;
// using DigDeeper.ColonistSystem;
// using Systems.InventorySystem;
// using Systems.JobSystem;
// using Systems.MovementSystem;
// using Systems.NeedsSystem;
// using UnityEngine;
//
// namespace Systems.ColonistSystem {
//     [Serializable]
//     public class Colonist {
//         // Reference to the ColonistConfig
//         public ColonistConfig Config;
//
//         // Basic Properties
//         public long EntitId { get; private set; }
//         public string Name { get; set; }
//         public Sprite Portrait { get; set; }
//         
//         
//         public AIState AIState { get; set; }
//         public ColonistStats ColonistStats { get; set; }
//         public Dictionary<SkillType, SkillConfig> Skills = new Dictionary<SkillType, SkillConfig>();
//         public List<NeedConfig> NeedsComponent = new List<NeedConfig>();
//         public Movement Movement { get; set; }
//         public InventoryComponent InventoryComponent { get; set; }
//         public Job CurrentJob { get; set; }
//         
//         public Colonist(long id, ColonistConfig config, string colonistName) {
//             if (config == null) throw new ArgumentNullException(nameof(config));
//
//             EntitId = id;
//             Config = config;
//             Name = config.name;
//             ColonistStats = new ColonistStats(
//                 config
//             );
//             Movement = new Movement {
//                 MaxSpeed = config.BaseSpeed,
//                 CurrentSpeed = 0f,
//             };
//
//             InventoryComponent = new InventoryComponent(Int32.MaxValue);
//         }
//
//         public bool HasJob => CurrentJob != null;
//
//         // public void GenerateStep(Job job) {
//         //     if (job == null) throw new ArgumentNullException(nameof(job));
//         //
//         //     if (!CanExecute(job)) {
//         //         return;
//         //     }
//         //
//         //     // GenerateStep the job logic here
//         //     Debug.Log($"Executing job {job.Name} for colonist {Name}.");
//         // }
//     }
//
//     public enum AIState {
//         Idle,
//         Working,
//         Resting,
//         Eating,
//         Drinking,
//         Sleeping,
//         Dead
//     }
// }