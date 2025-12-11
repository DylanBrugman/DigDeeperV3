using System.Collections.Generic;
using GamePlay.World.Tilemap.Generator;
using UnityEngine;

namespace DigDeeper.WorldSystem {
    [System.Serializable]
    public class PointOfInterestConfig
    {
        public string id;
        public string displayName;
        public Vector2Int size = new Vector2Int(5, 5);
        public TileType[,] tilePattern;
        public List<ResourceDeposit> guaranteedResources = new List<ResourceDeposit>();
        public float spawnWeight = 1f;
        public float minDistanceFromOthers = 20f;
        public bool spawnOnSurface = false;
        public Vector2 depthRange = new Vector2(20f, 80f);
    }
}