// using System;
// using System.Collections.Generic;
// using Systems.ColonistSystem;
// using UnityEngine;
//
// namespace Systems.JobSystem {
//     public class JobExecutingSystem : MonoBehaviour{
//         
//         private List<IJobExecutor> _jobExecutors = new List<IJobExecutor>();
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
//             _jobExecutors.Add(colonist);
//         }
//
//         private void Update() {
//             foreach (var jobExecutor in _jobExecutors) {
//                 // if (jobExecutor.HasJob) {
//                 //     GenerateStep(jobExecutor);
//                 // }
//             }
//         }
//     }
// }