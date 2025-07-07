// namespace Systems.MovementSystem {
//     // MovementSystem.cs
// using UnityEngine;
// using UnityEngine.AI; // Required for NavMesh pathfinding
// using System.Collections.Generic;
//
// public class MovementSystem : MonoBehaviour
// {
//     // --- State Definitions ---
//     private enum State { Idle, Pathfinding, FollowingPath }
//
//     private struct MoverState
//     {
//         public State CurrentState;
//         public List<Vector3> Path;
//         public int CurrentWaypointIndex;
//     }
//
//     // --- Singleton and Pools ---
//     public static MovementSystem Instance { get; private set; }
//
//     private readonly List<Mover> _movers = new();
//     private readonly List<MoverState> _states = new();
//     private readonly Stack<int> _free = new();
//     private readonly List<int> _active = new();
//
//     #region Unity Lifecycle
//     private void Awake()
//     {
//         if (Instance != null) { Destroy(gameObject); return; }
//         Instance = this;
//     }
//
//     private void FixedUpdate()
//     {
//         float dt = Time.fixedDeltaTime;
//
//         foreach (int idx in _active)
//         {
//             Mover mover = _movers[idx];
//             ref MoverState state = ref _states.GetRef(idx); // Using a helper for ref to list element
//
//             switch (state.CurrentState)
//             {
//                 case State.FollowingPath:
//                     FollowPath(mover, ref state, dt);
//                     break;
//
//                 // Idle and Pathfinding states are handled by requests, not the loop
//             }
//         }
//     }
//     #endregion
//
//     #region Public API
//     public int RegisterMover(MovementComponent movementComponent)
//     {
//         int idx = _free.Count > 0 ? _free.Pop() : _movers.Count;
//
//         if (idx == _movers.Count)
//         {
//             _movers.Add(null);
//             _states.Add(default);
//         }
//
//         _movers[idx] = movementComponent;
//         _states[idx] = new MoverState { CurrentState = State.Idle };
//         _active.Add(idx);
//
//         return idx;
//     }
//
//     public void UnregisterMover(int idx)
//     {
//         // Proper O(1) removal would be better, but this is functional.
//         _active.Remove(idx);
//         _free.Push(idx);
//         _movers[idx] = null;
//     }
//
//     public void RequestMoveTo(int moverHandle, Vector3 destination)
//     {
//         ref MoverState state = ref _states.GetRef(moverHandle);
//         Mover mover = _movers[moverHandle];
//
//         state.CurrentState = State.Pathfinding;
//
//         // --- THIS IS WHERE YOU CALL YOUR PATHFINDING SERVICE ---
//         // Using Unity's built-in NavMesh as an example.
//         var path = new NavMeshPath();
//         NavMesh.CalculatePath(mover.transform.position, destination, NavMesh.AllAreas, path);
//
//         if (path.status == NavMeshPathStatus.PathComplete)
//         {
//             state.Path = new List<Vector3>(path.corners);
//             state.CurrentWaypointIndex = 0;
//             state.CurrentState = State.FollowingPath;
//         }
//         else
//         {
//             // Path could not be found.
//             Debug.LogWarning($"Path not found for mover {mover.name} to {destination}");
//             state.CurrentState = State.Idle;
//         }
//     }
//     #endregion
//
//     #region Private Logic
//     private void FollowPath(Mover mover, ref MoverState state, float dt)
//     {
//         if (state.Path == null || state.CurrentWaypointIndex >= state.Path.Count)
//         {
//             state.CurrentState = State.Idle;
//             return;
//         }
//
//         Vector3 currentWaypoint = state.Path[state.CurrentWaypointIndex];
//         Vector3 position = mover.transform.position;
//         
//         // Ignore Y-axis for distance check to prevent issues on slopes
//         Vector3 positionOnPlane = new Vector3(position.x, 0, position.z);
//         Vector3 waypointOnPlane = new Vector3(currentWaypoint.x, 0, currentWaypoint.z);
//
//         // Check if waypoint is reached
//         if (Vector3.Distance(positionOnPlane, waypointOnPlane) < 0.2f)
//         {
//             state.CurrentWaypointIndex++;
//             // Check if that was the last waypoint
//             if (state.CurrentWaypointIndex >= state.Path.Count)
//             {
//                 mover.transform.position = state.Path[state.Path.Count - 1]; // Snap to final destination
//                 state.CurrentState = State.Idle;
//                 return;
//             }
//         }
//
//         // Move towards the current waypoint
//         Vector3 direction = (currentWaypoint - position).normalized;
//         mover.transform.position += direction * mover.Config.MoveSpeed * dt;
//         
//         // Make the mover look towards the direction it's moving
//         if (direction != Vector3.zero)
//         {
//             mover.transform.rotation = Quaternion.LookRotation(direction);
//         }
//     }
//     #endregion
// }
//
// // Helper extension to get a ref to a list element.
// // In newer C# versions, you can use CollectionsMarshal.AsSpan.
// public static class ListExtensions
// {
//     public static ref T GetRef<T>(this List<T> list, int index)
//     {
//         return ref System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list)[index];
//     }
// }
// }