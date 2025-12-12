
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Features.World.System;
using GamePlay.World;
using Systems.WorldSystem.Generator;
using Unity.Mathematics;
using UnityEngine;

namespace Features.World.Config {
    [CreateAssetMenu(menuName = "World/Prefab Map")]
    public class WorldGeneratedConfig : ScriptableObject , IWorldSource {
        public Tile[] Tiles;
        public List<PrefabSpawnConfig> Prefabs;
        public int2 WorldSizeChunks { get; set; }
        public int2 SizeChunks { get; }
        
        public IEnumerator Generate(Action<GamePlay.World.World> onDone, Action<float> onProgress, CancellationToken ct) {
            GamePlay.World.World world = new GamePlay.World.World();
            onDone?.Invoke(world);
            yield break;
        }

        public IEnumerator Generate(Action<GamePlay.World.World> onDone) {
            GamePlay.World.World world = new GamePlay.World.World();
            onDone?.Invoke(world);
            yield break;
        }
    }
}