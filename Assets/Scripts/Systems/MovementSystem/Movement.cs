// using System;
// using System.Collections.Generic;
// using Systems.PathfindingSystem;
// using UnityEngine;
//
// namespace Systems.MovementSystem {
//     
//     [Serializable]
//     public class Movement {
//         // public float CurrentSpeed { get; set; }
//         // public float MaxSpeed { get; set; }
//         // // public float Acceleration { get; set; }
//         // // public float Deceleration { get; set; }
//         // // public float AngularSpeed { get; set; }
//         // public float JumpForce { get; set; }
//         //
//         // public bool IsMoving => CurrentSpeed > 0f; // Check if the colonist is currently moving
//         // public bool IsJumping { get; set; } // Check if the colonist is currently jumping
//         // public bool IsGrounded { get; set; } // Check if the colonist is currently on the ground
//         //
//         
//         public Vector2 PositionComponent { get; set; } = Vector2.zero; // Current position of the colonist
//         public Vector2 VelocityComponent { get; set; } = Vector2.zero; // Current velocity of the colonist
//         public int MaxJumpHeight { get; set; } = 2; // Maximum height the colonist can jump
//         
//         public float MaxVelocity { get; set; } = 5f; // Maximum speed of the colonist
//         
//         public PathfindingPath PathfindingPath { get; set; } // Pathfinding path for movement
//         
//         public bool TargetPositionChanged { get; set; } // Flag to indicate if the target position has changed
//         public Vector2? TargetPosition { get; set; }
//         
//         public bool IsMoving => VelocityComponent.magnitude > 0f; // Check if the colonist is currently moving
//         public bool IsJumping { get; set; } // Check if the colonist is currently jumping
//         public bool IsGrounded { get; set; } // Check if the colonist is currently on the ground
//         public bool IsFalling => !IsGrounded && VelocityComponent.y < 0f; // Check if the colonist is currently falling
//         public bool IsTargetPositionReached => TargetPosition.HasValue && Vector2.Distance(PositionComponent, TargetPosition.InitialValue) < 0.1f;
//     }
// }