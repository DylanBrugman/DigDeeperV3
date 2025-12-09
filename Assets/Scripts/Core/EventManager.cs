// using System;
// using DigDeeper.ColonistSystem;
// using DigDeeper.TaskSystem;
// using UnityEngine;
//
// namespace Core {
//     public class EventManager : MonoBehaviour
//     {
//         public static EventManager Instance { get; private set; }
//         
//         // ColonistController Events
//         public event Action<ColonistController> OnColonistSpawned;
//         public event Action<ColonistController> OnColonistDied;
//         public event Action<ColonistController> OnColonistIncapacitated;
//         public event Action<ColonistController> OnColonistStatusChanged;
//         
//         // Task Events
//         public event Action<ITask> OnTaskCompleted;
//         public event Action<ITask, string> OnTaskFailed;
//         public event Action<ITask> OnTaskAssigned;
//         
//         // SkillConfig Events
//         public event Action<SkillType, float> OnSkillLevelUp;
//         
//         private void Awake()
//         {
//             if (Instance == null)
//             {
//                 Instance = this;
//                 DontDestroyOnLoad(gameObject);
//             }
//             else
//             {
//                 Destroy(gameObject);
//             }
//         }
//
//         public void InvokeOnColonistSpawned(ColonistController colonist) {
//             OnColonistSpawned?.Invoke(colonist);
//         }
//
//         public void InvokeOnTaskCompleted(ITask completedTask) {
//             OnTaskCompleted?.Invoke(completedTask);
//         }
//
//         public void InvokeOnTaskFailed(ITask failedTask, string reason) {
//             OnTaskFailed?.Invoke(failedTask, reason);
//         }
//
//         public void InvokeOnColonistDied(ColonistController colonist) {
//             OnColonistDied?.Invoke(colonist);
//         }
//
//         public void IvokeOnColonistIncapacitated(ColonistController colonist) {
//             OnColonistIncapacitated?.Invoke(colonist);
//         }
//
//         public void InvokeOnSkillLevelUp(SkillType type, float level) {
//             OnSkillLevelUp?.Invoke(type, level);
//         }
//     }
// }