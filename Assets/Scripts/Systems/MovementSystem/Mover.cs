using UnityEngine;

namespace Systems.MovementSystem {
    public struct Mover                 // every moving entity has exactly this
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float   MaxSpeed;

        // MVP locomotion flags
        public bool CanJump;
        public bool CanClimb;
    }
}