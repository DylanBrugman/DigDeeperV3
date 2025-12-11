// using System;
// using System.Collections.Generic;
// using DigDeeper.ColonistSystem;
// using Systems.ColonistSystem;
// using UnityEngine;
//
// namespace Systems.JobSystem {
//     public class JobManager : MonoBehaviour {
//
//         [SerializeField] private ColonistManager colonistManager;
//
//         private List<Job> _jobs = new List<Job>();
//         
//         public void AddJob(Job job) {
//             if (job == null) {
//                 throw new ArgumentNullException(nameof(job), "Job cannot be null");
//             }
//             
//             _jobs.Add(job);
//         }
//         
//         public void RemoveJob(Job job) {
//             if (job == null) {
//                 throw new ArgumentNullException(nameof(job), "Job cannot be null");
//             }
//             
//             if (!_jobs.Remove(job)) {
//                 Debug.LogWarning($"Job {job} not found in the job list.");
//             }
//             
//             
//         }
//         
//         public void RequestJob(Job job) {
//             
//         }
//
//         public void CompleteJob(Job job) {
//             
//         }
//
//         public void AssignJob(ColonistController colonistController) {
//         }
//     }
// }