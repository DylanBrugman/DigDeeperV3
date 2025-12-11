using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using GamePlay.World.Tilemap;
using Systems.WorldSystem.Generator;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay.World {
    [CreateAssetMenu(menuName = "World/Prefab Map")]
    public class WorldGeneratedConfig : ScriptableObject , IWorldSource {
        public Tile[] Tiles;
        public List<PrefabSpawnConfig> Prefabs;
        public int2 WorldSizeChunks { get; set; }
        public int2 SizeChunks { get; }
        
        public IEnumerator Generate(Action<WorldDTO> onDone, Action<float> onProgress, CancellationToken ct) {
            throw new NotImplementedException();
        }
    }
}