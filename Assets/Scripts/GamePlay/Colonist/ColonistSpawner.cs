using System;
using Core;
using Core.ECS;
using GamePlay.World;
using UnityEngine;

namespace GamePlay.Colonist {
    public class ColonistSpawner : MonoBehaviour, IWorldSpawner {
        private void Awake() {
            ServiceLocator.Register<IWorldSpawner>(this);
        }

        public void SpawnPreScene(WorldData worldData, WorldRuntimeContext worldRuntimeContext, ECSWorld ecsWorld) {
            throw new NotImplementedException();
        }

        public void SpawnPostScene(WorldData worldData, WorldRuntimeContext worldRuntimeContext, ECSWorld ecsWorld) {
            throw new NotImplementedException();
        }

        private void OnDestroy() {
            ServiceLocator.UnregisterInstance<IWorldSpawner>(this);
        }
    }
}