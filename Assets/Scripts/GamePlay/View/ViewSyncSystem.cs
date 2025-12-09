using Core.ECS;
using ECSImpl.Components;

namespace ECSImpl.Systems {
    public class ViewSyncSystem : IEcsSystem {
        
        public int ProcessedEntitiesCount { get; private set; }

        public void Tick(ECSWorld ecsWorld, float dt) {
            ProcessedEntitiesCount = 0;
            
            var transform = ecsWorld.All<TransformComponent>();

            while (transform.MoveNext()) {
                var transformComponent = transform.Component;
                var entityId = transform.EntityId;
                
                if (!ecsWorld.Has<PositionComponent>(entityId)) continue;
                var position = ecsWorld.Get<PositionComponent>(entityId);
                
                // Update the transform's position based on the PositionComponent component
                transformComponent.Transform.position = new UnityEngine.Vector3(position.Position.x, position.Position.y, 0f);
                
                ProcessedEntitiesCount++;
            }
        }
    }
}