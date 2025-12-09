using Core.ECS;
using Unity.Mathematics;

namespace GamePlay.Movement {
    public struct VelocityComponent : IComponent {
        public float2 Velocity;
        public float MaxVelocity;
    }
}