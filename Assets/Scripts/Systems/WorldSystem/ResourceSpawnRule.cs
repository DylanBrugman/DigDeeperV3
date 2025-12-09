using GamePlay.World.Tilemap.Generator;
using UnityEngine;

namespace DigDeeper.WorldSystem {
    [System.Serializable]
    public class ResourceSpawnRule
    {
        public ResourceType resourceType;
        [Range(0, 1)] public float spawnChance;
        [Range(0, 100)] public float minAbundance;
        [Range(0, 100)] public float maxAbundance;
        [Range(0, 100)] public float baseQuality;
        public Vector2 clusterSize = new Vector2(3, 8); // Min/MaxVelocity cluster size
        public bool requiresSpecificTileType = false;
        public TileType requiredTileType = TileType.Stone;
    }
}