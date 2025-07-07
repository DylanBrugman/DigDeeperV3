namespace Systems.MovementSystem {
    // MovementConfig.cs
    using UnityEngine;

    [System.Serializable]
    public class MovementConfig
    {
        [Tooltip("How fast the mover travels along the path in meters/second.")]
        public float MoveSpeed = 5f;

        [Tooltip("The max height the mover can jump to reach an adjacent, higher point.")]
        public int MaxJumpHeight = 2;

        [Tooltip("The max distance the mover can fall without taking damage or getting stuck. This is a constraint for the pathfinder.")]
        public float MaxFallHeight = 5f;

        [Tooltip("Can this mover climb specific surfaces (e.g., ladders)?")]
        public bool CanClimb = false;
    }
}