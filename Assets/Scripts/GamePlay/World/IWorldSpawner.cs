using Core.ECS;

namespace GamePlay.World {
    public interface IWorldSpawner
    {
        /// Safe before gameplay scene; create ECS entities or pooled objects.
        void SpawnPreScene(WorldData worldData, WorldRuntimeContext worldRuntimeContext, ECSWorld ecsWorld);

        /// Called after SceneManager loads the target scene; safe to Instantiate GOs.
        void SpawnPostScene(WorldData worldData, WorldRuntimeContext worldRuntimeContext, ECSWorld ecsWorld);
    }
}