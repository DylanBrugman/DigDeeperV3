// using System.Collections.Generic;
// using Core.ECS;
// using DigDeeper.ColonistSystem;
// using Systems.ColonistSystem;
// using Systems.JobSystem.Requirements;
// using Systems.WorldSystem.Generator;
// using UnityEngine;
//
// namespace Systems.JobSystem {
//     
//     public enum JobStatus { Pending, Running, Completed, Failed }
//
//     public struct JobComponent : IComponent
//     {
//         public Job Job;
//     }
//     
//     public abstract class Job
//     {
//         public string                Name;
//         public Vector2               TargetDestination;
//         public readonly List<IRequirement> Requirements = new();
//
//         public JobStatus Status { get; private set; } = JobStatus.Pending;
//
//         // public bool CanStart(Colonist c)
//         // {
//         //     foreach (var r in Requirements)
//         //         if (!r.IsMet(c)) return false;
//         //     return true;
//         // }
//
//         /// <summary>Perform *one* step. Return true when finished.</summary>
//         public bool Tick(Colonist c, float dt)
//         {
//             if (Status == JobStatus.Completed) return true;
//             if (Status == JobStatus.Pending)   Status = JobStatus.Running;
//
//             bool done = ExecuteInternal(c, dt);
//             if (done) Status = JobStatus.Completed;
//             return done;
//         }
//
//         protected abstract bool ExecuteInternal(Colonist c, float dt);
//
//         public void Interrupt() => Status = JobStatus.Failed;
//     }
//
//     public class DigJob : Job {
//         
//         public Tile TargetTile { get; }
//
//         public DigJob(Vector2 targetDestination, Tile targetTile) {
//             Name = "Dig";
//             TargetDestination = targetDestination;
//             Requirements.Add(new ToolActionRequirement(ToolAction.Dig));
//             Requirements.Add(new SkillRequirement(SkillType.Digging, 1));
//             TargetTile = targetTile;
//         }
//         
//         protected override bool ExecuteInternal(Colonist c, float dt) {
//             if (c.Movement.IsTargetPositionReached) {
//                 // Perform digging logic here
//                 Debug.Log($"{c.Name} is digging at {TargetDestination}");
//                 return true;
//             }
//             
//             return false;
//         }
//     }
// }