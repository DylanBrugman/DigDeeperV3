using Core.ECS;
using ECSImpl.Components;

namespace GamePlay.Movement {
    public class MovementSystem : IEcsSystem {
        
        public int ProcessedEntitiesCount { get; private set; }
        
        public void Tick(ECSWorld ecsWorld, float dt) {
            ProcessedEntitiesCount = 0;
            
            var movers = ecsWorld.All<VelocityComponent, PositionComponent>();

            while (movers.MoveNext()) {
                ref var velComp = ref movers.A;
                ref var posComp = ref movers.B;
                var entityId = movers.EntitId;

                if (!ecsWorld.Has<VelocityComponent>(entityId)) continue;
                
                // Apply movement constraints
                if (velComp.Velocity.x > velComp.MaxVelocity) {
                    velComp.Velocity.x = velComp.MaxVelocity;
                }
                else if (velComp.Velocity.x < -velComp.MaxVelocity) {
                    velComp.Velocity.x = -velComp.MaxVelocity;
                }
                
                // Update position based on velocity
                posComp.Position += velComp.Velocity * dt;
                
                ProcessedEntitiesCount++;
            }
        }
    }
}