// using System.Collections.Generic;
// using DigDeeper.ColonistSystem;
// using Systems.ColonistSystem;
// using UnityEngine;
//
// namespace DigDeeper.TaskSystem {
//     public interface ITask
//     {
//         int TaskId { get; }
//         string TaskName { get; }
//         TaskPriority Priority { get; set; }
//         Vector2Int GetTargetPosition();
//         Dictionary<SkillType, float> GetRequiredSkills();
//         float GetEstimatedDuration();
//         bool CanBePerformedBy(ColonistController colonistController);
//         void AssignToColonist(ColonistController colonistController);
//         ColonistController GetAssignedColonist();
//         void StartExecution();
//         void StopExecution();
//         void OnInterrupted();
//         bool IsCompleted();
//         bool HasFailed();
//         string GetFailureReason();
//     }
// }