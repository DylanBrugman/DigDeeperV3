// using Systems.MovementSystem;
// using UnityEngine;
//
// namespace Systems.MovementSystem {
//     // Mover.cs
//     using UnityEngine;
//
//     [RequireComponent(typeof(Rigidbody))] // Good practice for physics interactions
//     public class MovementComponent : MonoBehaviour
//     {
//         [Tooltip("Configuration data for this specific mover's capabilities.")]
//         [field:SerializeField] public MovementConfig Config { get; private set; }
//
//         // The handle for this mover within the MovementSystem
//         public int MovementHandle { get; private set; } = -1;
//
//         private void OnEnable()
//         {
//             if (MovementSystem.Instance != null)
//             {
//                 // Register with the system, passing a reference to ourselves.
//                 // This is cleaner than passing each piece of data individually.
//                 MovementHandle = MovementSystem.Instance.RegisterMover(this);
//             }
//         }
//
//         private void OnDisable()
//         {
//             if (MovementSystem.Instance != null)
//             {
//                 MovementSystem.Instance.UnregisterMover(MovementHandle);
//                 MovementHandle = -1;
//             }
//         }
//
//         /// <summary>
//         /// Commands the mover to find a path and move to a destination.
//         /// </summary>
//         /// <param name="destination">The ecsWorld-space position to move to.</param>
//         public void SetDestination(Vector3 destination)
//         {
//             if (MovementHandle != -1 && MovementSystem.Instance != null)
//             {
//                 MovementSystem.Instance.RequestMoveTo(MovementHandle, destination);
//             }
//         }
//     }
// }