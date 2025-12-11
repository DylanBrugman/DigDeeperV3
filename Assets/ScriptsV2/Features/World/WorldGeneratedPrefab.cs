using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using GamePlay.Map.Generator.New.Core.WorldGen;
using GamePlay.World.Tilemap;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay.World {
    [CreateAssetMenu(menuName = "World/Prefab Map")]
    public class WorldGeneratedPrefab : ScriptableObject , IWorldSource {
        public Tile[] Tiles;
        public List<PrefabSpawnInfo> Prefabs;
        public int2 WorldSizeChunks { get; set; }
        public int2 SizeChunks { get; }
        
        public IEnumerator LoadCoroutine(Action<WorldData> onDone, Action<float> onProgress, CancellationToken ct) {
            throw new NotImplementedException();
        }
    }
}