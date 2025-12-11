using System;
using Core;
using Core.ECS;
using UnityEngine;

namespace GamePlay.World {
    public class TilemapSpawner : MonoBehaviour, IWorldSpawner {
        private void Awake() {
            ServiceLocator.Register<IWorldSpawner>(this);
        }

        public void SpawnPreScene(WorldData worldData, WorldRuntimeContext worldRuntimeContext, ECSWorld ecsWorld) {
            
        }

        public void SpawnPostScene(WorldData worldData, WorldRuntimeContext worldRuntimeContext, ECSWorld ecsWorld) {
            throw new System.NotImplementedException();
        }

        private void OnDestroy() {
            ServiceLocator.UnregisterInstance<IWorldSpawner>(this);
        }
    }
}