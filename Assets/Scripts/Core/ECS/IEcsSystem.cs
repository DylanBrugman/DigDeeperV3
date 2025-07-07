namespace Core.ECS {
    public interface IEcsSystem {
        void Tick(ECSWorld ecsWorld, float dt);
        
        int ProcessedEntitiesCount { get; }
    }
}