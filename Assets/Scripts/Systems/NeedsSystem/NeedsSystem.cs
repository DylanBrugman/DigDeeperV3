// using System;
// using Systems.ColonistSystem;
// using UnityEngine;
//
// namespace Systems.NeedsSystem {
//     public class NeedsSystem : MonoBehaviour {
//         
//         
//
//         public void RegisterNeeds(object owner, Needs needs) {
//             
//         }
//         
//         public void UnregisterNeeds(Needs needs) {
//             if (needs == null) throw new ArgumentNullException(nameof(needs));
//         }
//         
//         public void UpdateNeeds(Needs needs) {
//             if (needs == null) throw new ArgumentNullException(nameof(needs));
//             
//             // Update logic for needs
//             foreach (var need in needs.AllNeeds) {
//                 if (need == null) continue;
//
//                 // Example: Decrease need InitialValue over time
//                 need.InitialValue -= Time.deltaTime * need.DecayRatePerMinute;
//
//                 // Clamp the InitialValue to ensure it doesn't go below zero
//                 if (need.InitialValue < 0) {
//                     need.InitialValue = 0;
//                 }
//
//                 // Check if the need is critical
//                 if (need.InitialValue <= need.CriticalThreshold) {
//                     // Debug.LogWarning($"NeedConfig {need.Name} is critical for {needs.ColonistData.Name}");
//                     // Handle critical need logic here, e.g., alerting the player or triggering an event
//                 }
//             }
//         }
//     }
// }